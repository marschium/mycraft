using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.World;
using MyCraft.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine
{
    public class ChunkRenderer
    {

        private bool _compiledShaders;
        private int _terrainVertexShaderId;
        private int _terrainFragmentShaderId;
        private int _terrainShaderProgramId;
        private ShaderLoader _shaderLoader;
        private TextureLoader _TextureLoader;

        private int fogId;
        private int fogColourId;
        private int  mvpId;
        private int minLightId;

        public ChunkRenderer(ShaderLoader shaderLoader, TextureLoader TextureLoader)
        {
            _shaderLoader = shaderLoader;
            _TextureLoader = TextureLoader;
        }

        public void DrawModel(ChunkModel model, Vector3 position, FrameRenderInfo renderInfo)
        {
            if (!_compiledShaders)
            {
                CompileAndLoadShaders();
                _compiledShaders = true;
            }

            GL.UseProgram(_terrainShaderProgramId); // use the terrain shader
            GL.BindTexture(TextureTarget.Texture2D, _TextureLoader.GetTextureId("atlas.png")); // TODO model should contain texture id

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, model.VertexBufferId);
            GL.VertexAttribPointer(
               0,                  // must match the layout in the shader.
               3,                  // size
               VertexAttribPointerType.Float,           // type
               false,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );

            // Setup texture buffer.
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, model.TextureBufferId);
            GL.VertexAttribPointer(
               1,                  // must match the layout in the shader.
               2,                  // size
               VertexAttribPointerType.Float,           // type
               false,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );

            // Setup lighting buffer.
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, model.LightDataBufferId);
            GL.VertexAttribPointer(
               2,                  // must match the layout in the shader.
               3,                  // size
               VertexAttribPointerType.Float,           // type
               false,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );

            GL.Uniform1(fogId, renderInfo.Fog);
            GL.Uniform3(fogColourId, renderInfo.FogColour);
            GL.Uniform3(minLightId, ChunkLighting.MinShaderLightLevel);
            
            var modelMatrix = Matrix4.Identity;
            Matrix4.CreateTranslation(position.X, 0, position.Z, out modelMatrix);
            var modelViewProjectionMatrix = modelMatrix * Camera.MainCamera.LookAt * Camera.MainCamera.Projection; // Opentk has row-orientated matrices so its m*v*p and not p*v*m
            GL.UniformMatrix4(mvpId, false, ref modelViewProjectionMatrix);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.VerticesCount);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(3);
        }

        private void CompileAndLoadShaders()
        {
            // shaders for terrain
            _terrainVertexShaderId = _shaderLoader.LoadShader(@"Shaders\chunk.vert", ShaderType.VertexShader);
            _terrainFragmentShaderId = _shaderLoader.LoadShader(@"Shaders\chunk.frag", ShaderType.FragmentShader);
            _terrainShaderProgramId = GL.CreateProgram();
            GL.AttachShader(_terrainShaderProgramId, _terrainFragmentShaderId);
            GL.AttachShader(_terrainShaderProgramId, _terrainVertexShaderId);
            GL.LinkProgram(_terrainShaderProgramId);
            Console.Write(string.Format("link program: {0}", GL.GetProgramInfoLog(_terrainShaderProgramId)));

            fogId = GL.GetUniformLocation(_terrainShaderProgramId, "fog_density");
            mvpId = GL.GetUniformLocation(_terrainShaderProgramId, "mvp");
            fogColourId = GL.GetUniformLocation(_terrainShaderProgramId, "fog_colour");
            minLightId = GL.GetUniformLocation(_terrainShaderProgramId, "min_light");
        }
    }
}
