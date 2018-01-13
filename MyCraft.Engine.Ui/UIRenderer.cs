using MyCraft.Engine.Ui.Components;
using MyCraft.Util;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Ui
{
    public class UIRenderer
    {

        private ShaderLoader _shaderLoader;

        private int _uiVertexShaderId;
        private int _uiShaderProgramId;
        private int _uiFragmentShaderId;
        private bool _compiledShaders;

        private int _positionId;

        public UIRenderer(ShaderLoader shaderLoader)
        {
            _shaderLoader = shaderLoader;
        }

        public void Draw(UiComponent uiComponent)
        {
            if (!_compiledShaders)
            {
                CompileAndLoadShaders();
                _compiledShaders = true;
            }

            GL.UseProgram(_uiShaderProgramId); // use the ui shader

            GL.BindTexture(TextureTarget.Texture2D, uiComponent.TextureId);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

            GL.EnableVertexAttribArray(6);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uiComponent.VertexBufferId);
            GL.VertexAttribPointer(
               6,
               2,                  // size
               VertexAttribPointerType.Float,           // type
               false,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );

            GL.EnableVertexAttribArray(7);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uiComponent.TextureBufferId);
            GL.VertexAttribPointer(
               7,
               2,                  // size
               VertexAttribPointerType.Float,           // type
               false,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );
            GL.Uniform2(_positionId, uiComponent.Position);

            GL.DrawArrays(PrimitiveType.Triangles, 0, uiComponent.Vertices.Length);

            GL.DisableVertexAttribArray(6);
            GL.DisableVertexAttribArray(7);
        }

        private void CompileAndLoadShaders()
        {
            _uiVertexShaderId = _shaderLoader.LoadShader(@"Shaders\ui.vert", ShaderType.VertexShader);
            _uiFragmentShaderId = _shaderLoader.LoadShader(@"Shaders\ui.frag", ShaderType.FragmentShader);
            _uiShaderProgramId = GL.CreateProgram();
            GL.AttachShader(_uiShaderProgramId, _uiFragmentShaderId);
            GL.AttachShader(_uiShaderProgramId, _uiVertexShaderId);
            GL.LinkProgram(_uiShaderProgramId);
            Console.Write(string.Format("link program: {0}", GL.GetProgramInfoLog(_uiShaderProgramId)));
            _positionId = GL.GetUniformLocation(_uiShaderProgramId, "pos");
        }
    }
}
