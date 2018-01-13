using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.Particles;
using MyCraft.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyCraft.Engine.GameObjects
{
    public class ParticleSystemRenderer
    {

        private bool _loadedShaders;
        private int _shaderProgramId;
        private int _vertexShaderProgramId;
        private int _fragmentShaderProgramId;

        private int _vertexParamId = 8;
        private int _posParamId = 9;
        private int _colourParamId = 10;

        private ShaderLoader _shaderLoader;

        public ParticleSystemRenderer(ShaderLoader shaderLoader)
        {
            _shaderLoader = shaderLoader;
        }

        public void DrawParticleSystem(ParticleComponent particleComponent, FrameRenderInfo renderInfo, ICamera camera)
        {
            if (!_loadedShaders)
            {
                LoadShaders();
            }

            var particleSystem = particleComponent.ParticleSystem;
            if (!particleSystem.Loaded)
            {
                particleSystem.BuildBuffers();
            }

            GL.UseProgram(_shaderProgramId);

            GL.EnableVertexAttribArray(_vertexParamId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, particleSystem.VertexBufferId);
            GL.VertexAttribPointer(
               _vertexParamId,                  //  must match the layout in the shader.
               3,                  // size
               VertexAttribPointerType.Float,           // type
               false,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );

            GL.EnableVertexAttribArray(_posParamId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, particleSystem.PositionBufferId);
            var positions = particleSystem.ParticlePositions;
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, positions.Length * 3 * sizeof(float), positions);
            GL.VertexAttribPointer(
               _posParamId,                  // must match the layout in the shader.
               3,                  // size
               VertexAttribPointerType.Float,           // type
               false,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );

            GL.EnableVertexAttribArray(_colourParamId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, particleSystem.ColourBufferId);
            var colours = particleSystem.ParticleColours;
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, colours.Length * 4 * sizeof(byte), colours);
            GL.VertexAttribPointer(
               _colourParamId,                  //  must match the layout in the shader.
               4,                  // size
               VertexAttribPointerType.UnsignedByte,           // type
               true,           // normalized?
               0,                  // stride
               0            // array buffer offset
            );

            var modelMatrix = Matrix4.Identity;
            Matrix4.CreateTranslation(particleComponent.Position.X, particleComponent.Position.Y, particleComponent.Position.Z, out modelMatrix);

            // set the rotation section of the modelMatrix to the transpose of the rotation section of the camera look at matrix so that always facing camera
            if (particleComponent.ParticleSystem.ParticleOrientation == ParticleOrientation.Billboard)
            {
                modelMatrix.M11 = camera.LookAt.M11;
                modelMatrix.M12 = camera.LookAt.M21;
                modelMatrix.M13 = camera.LookAt.M31;
                modelMatrix.M21 = camera.LookAt.M12;
                modelMatrix.M22 = camera.LookAt.M22;
                modelMatrix.M23 = camera.LookAt.M32;
                modelMatrix.M31 = camera.LookAt.M13;
                modelMatrix.M32 = camera.LookAt.M23;
                modelMatrix.M33 = camera.LookAt.M33;
            }


            var scale = Matrix4.CreateScale(particleComponent.Scale); // TODO different sized particles in the same system
            modelMatrix = scale * modelMatrix;

            var modelViewProjectionMatrix = modelMatrix * camera.LookAt * camera.Projection; // Opentk has row-orientated matrices so its m*v*p and not p*v*m
            GL.UniformMatrix4(renderInfo.MvpId, false, ref modelViewProjectionMatrix);

            GL.VertexAttribDivisor(_vertexParamId, 0); // reuse the same array every time
            GL.VertexAttribDivisor(_posParamId, 1); // one pos entry per qaud
            GL.VertexAttribDivisor(_colourParamId, 1); // one colour entry per quad
            GL.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, ParticleSystem.BillboardVertices.Length, particleSystem.Particles.Count);

            GL.DisableVertexAttribArray(_vertexParamId);
            GL.DisableVertexAttribArray(_posParamId);
            GL.DisableVertexAttribArray(_colourParamId);
        }

        private void LoadShaders()
        {
            _vertexShaderProgramId = _shaderLoader.LoadShader(@"shaders\particle.vert", ShaderType.VertexShader);
            _fragmentShaderProgramId = _shaderLoader.LoadShader(@"shaders\particle.frag", ShaderType.FragmentShader);
            _shaderProgramId = GL.CreateProgram();
            GL.AttachShader(_shaderProgramId, _fragmentShaderProgramId);
            GL.AttachShader(_shaderProgramId, _vertexShaderProgramId);
            GL.LinkProgram(_shaderProgramId);
            Console.Write(string.Format("link program: {0}", GL.GetProgramInfoLog(_shaderProgramId)));
            _loadedShaders = true;
        }
    }
}
