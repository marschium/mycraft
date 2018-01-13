using MyCraft.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects.Components;

namespace MyCraft.Engine.GameObjects
{
    public class GameObjectModelRenderer
    {
        private ShaderLoader _shaderLoader;
        private bool _shadersLoaded;
        private int _terrainVertexShaderId;
        private int _terrainFragmentShaderId;
        private int _terrainShaderProgramId;
        private int _mvpId;

        // TODO abstract/generic render
        public GameObjectModelRenderer(ShaderLoader loader)
        {
            _shaderLoader = loader;
        }

        // TODO maybe make camera part of the the render info
        public void Render(ModelComponent model, ICamera camera, FrameRenderInfo renderInfo)
        {
            if (model != null && !model.Invisible)
            {
                if (!_shadersLoaded)
                {
                    CompileAndLoadShaders();
                    _shadersLoaded = true;
                }

                GL.UseProgram(_terrainShaderProgramId);
                GL.BindTexture(TextureTarget.Texture2D, model.Texture.TextureId);

                //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

                GL.EnableVertexAttribArray(4);
                GL.BindBuffer(BufferTarget.ArrayBuffer, model.VertexBufferId);
                GL.VertexAttribPointer(
                   4,                  // attribute 0. No particular reason for 0, but must match the layout in the shader.
                   3,                  // size
                   VertexAttribPointerType.Float,           // type
                   false,           // normalized?
                   0,                  // stride
                   0            // array buffer offset
                );

                GL.EnableVertexAttribArray(5);
                GL.BindBuffer(BufferTarget.ArrayBuffer, model.Texture.TextureBufferId);
                GL.VertexAttribPointer(
                   5,                  // attribute 0. No particular reason for 0, but must match the layout in the shader.
                   2,                  // size
                   VertexAttribPointerType.Float,           // type
                   false,           // normalized?
                   0,                  // stride
                   0            // array buffer offset
                );

                var modelMatrix = Matrix4.Identity;
                Matrix4.CreateTranslation(
                    model.Position.X,
                    model.Position.Y, 
                    model.Position.Z,
                    out modelMatrix);
                //var xRotationMatrix = Matrix4.Identity;
                //Matrix4.CreateRotationX((float)model.Parent.VerticalRotation, out xRotationMatrix);
                var yRotationMatrix = Matrix4.Identity;
                Matrix4.CreateRotationY((float)model.Parent.HorizontalRotation, out yRotationMatrix);
                modelMatrix = yRotationMatrix * modelMatrix;

                var modelViewProjectionMatrix = modelMatrix * camera.LookAt * camera.Projection; // Opentk has row-orientated matrices so its m*v*p and not p*v*m
                GL.UniformMatrix4(renderInfo.MvpId, false, ref modelViewProjectionMatrix);
                GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);

                GL.DisableVertexAttribArray(4);
                GL.DisableVertexAttribArray(5);
            }
        }

        private void CompileAndLoadShaders()
        {
            // shaders for terrain
            _terrainVertexShaderId = _shaderLoader.LoadShader(@"Shaders\gameObj.vert", ShaderType.VertexShader);
            _terrainFragmentShaderId = _shaderLoader.LoadShader(@"Shaders\gameObj.frag", ShaderType.FragmentShader);
            _terrainShaderProgramId = GL.CreateProgram();
            GL.AttachShader(_terrainShaderProgramId, _terrainFragmentShaderId);
            GL.AttachShader(_terrainShaderProgramId, _terrainVertexShaderId);
            GL.LinkProgram(_terrainShaderProgramId);
            Console.Write(string.Format("link program: {0}", GL.GetProgramInfoLog(_terrainShaderProgramId)));

            // Get an id for the MVP so it can be passed to the shader
            _mvpId = GL.GetUniformLocation(_terrainShaderProgramId, "mvp");
        }
    }
}
