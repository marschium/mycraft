using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Core.GameObjects.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyCraft.Engine.Ui
{
    public class TextUtil
    {
        private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890. ";
        private static Dictionary<char, int> characterLookup = characters.Select((s, i) => new { character = s, index = i }).ToDictionary(x => x.character, y => y.index);
        private const int TextureSize = 64;
        private static float _textureCoordWidth;

        private static readonly Vector2[] _charVertices = new Vector2[6]
            {
                new Vector2(0f, 0f),
                new Vector2(64f, 0f),
                new Vector2(64f, 64f),
                new Vector2(0f, 0f),
                new Vector2(64f, 64f),
                new Vector2(0f, 64f),
            };

        private static readonly Vector2[] _charTextureUvCoords = new Vector2[6]
            {                
                new Vector2(0f,0f),
                new Vector2(1f,0f),
                new Vector2(1f,1f),
                new Vector2(0f,0f),
                new Vector2(1f,1f),
                new Vector2(0f,1f),
            };

        private Lazy<int> textureAtalsId;
        private int _atlasHeight;
        private int _atlasWidth;

        public TextUtil()
        {
            textureAtalsId = new Lazy<int>(LoadCharacterAtlas);
        }

        public Text GetText(string text, int textSize = 32, int verticalPadding = 2)
        {
            var textureId = textureAtalsId.Value;

            var textVertices = new List<Vector2>();
            var textTextureCoords = new List<Vector2>();
            var verticalOffset = 0;
            var horizontalOffset = 0;
            for(int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    verticalOffset -= textSize + verticalPadding;
                    horizontalOffset = 0;
                    continue;
                }

                // TODO draw symbol for missing chars
                var success = characterLookup.TryGetValue(text[i], out var textureOffset);
                if (success)
                {
                    horizontalOffset += textSize;
                    var vertices = _charVertices.Select(v => new Vector2(v.X == 0 ? horizontalOffset : horizontalOffset + textSize, v.Y == 0 ? verticalOffset : textSize + verticalOffset)).ToArray();
                    var coords = _charTextureUvCoords.Select(v => new Vector2(v.X == 0 ? _textureCoordWidth * textureOffset : (_textureCoordWidth * textureOffset) + _textureCoordWidth, v.Y)).ToArray();
                    textVertices.AddRange(vertices);
                    textTextureCoords.AddRange(coords);
                }
            }

            var tex = new Texture
            {
                Coords = textTextureCoords.ToArray(),
                TextureId = textureId
            };

            return new Text
            {
                Vertices = textVertices.ToArray(),
                Texture = tex
            };
        } 
        
        private int LoadCharacterAtlas()
        {
            int texId;
            var bmp = new Bitmap("characters.png");
            _atlasHeight = bmp.Height;
            _atlasWidth = bmp.Width;
            _textureCoordWidth = 1f / (_atlasWidth / TextureSize);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GenTextures(1, out texId);
            GL.BindTexture(TextureTarget.Texture2D, texId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            return texId;
        }
    }
}
