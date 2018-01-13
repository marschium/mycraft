using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using MyCraft.Engine.Core.GameObjects.Components;

namespace MyCraft.Assets
{
    public class TextureLoader
    {
        // TODO this class should probably take a texture name in the constructor or something and use a cache internally

        private IDictionary<string, int> _textureIds;
        private TextureInfo _textureInfo;
        private IDictionary<string, Tuple<int, int>> _textureFileSizes;

        public TextureLoader(TextureInfo textureInfo)
        {
            _textureInfo = textureInfo;
            _textureIds = new Dictionary<string, int>();
            _textureFileSizes = new Dictionary<string, Tuple<int, int>>();
        }

        public int GetTextureId(string textureName)
        {
            if (!_textureIds.ContainsKey(textureName))
            {
                var id = LoadTexture(textureName);
                _textureIds[textureName] = id;
            }
            return _textureIds[textureName];
        }

        /// <summary>
        /// Loads the texture atlas and returns the texture id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int LoadTexture(string path)
        {
            var bmp = new Bitmap(path);
            _textureFileSizes[path] = new Tuple<int, int>(bmp.Width, bmp.Height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GenTextures(1, out int texId);
            GL.BindTexture(TextureTarget.Texture2D, texId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            return texId;
        }

        public Texture GetTexture(string file, TextureName name, Vector2[] uv)
        {
            var coords = GetScaledUVCoords(file, name, uv);
            return new Texture
            {
                Coords = coords,
                TextureId = _textureIds[file]
            };
        }

        public Vector2[] GetScaledUVCoords(string file, TextureName name, Vector2[] uv)
        {
            var minMax = GetTextureMinMaxCoords(file, name);
            Vector2[] o = new Vector2[uv.Length];
            for (int i = 0; i < uv.Length; i++)
            {
                o[i] = new Vector2(uv[i].X == 0 ? minMax.Item1.X : minMax.Item2.X, uv[i].Y == 0 ? minMax.Item1.Y : minMax.Item2.Y);
            }
            return o;
        }

        private Tuple<Vector2, Vector2> GetTextureMinMaxCoords(string textureFile, TextureName name)
        {
            if (!_textureIds.ContainsKey(textureFile))
            {
                var id = LoadTexture(textureFile);
                _textureIds[textureFile] = id;
            }

            var pos = _textureInfo.TexutrePositions[textureFile][name.ToString()];
            float w = 1f / (_textureFileSizes[textureFile].Item1 / _textureInfo.TextureSize[textureFile]);
            float h = 1f / (_textureFileSizes[textureFile].Item2 / _textureInfo.TextureSize[textureFile]);

            var lower = new Vector2(w * pos.X, h * pos.Y);
            var upper = new Vector2(lower.X + w, lower.Y + h);

            return new Tuple<Vector2, Vector2>(lower,upper);
        }
    }
}
