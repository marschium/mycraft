using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace MyCraft.Util
{
    public class ShaderLoader
    {
        /// <summary>
        /// Load the shader at specified path. Returns Id of shader.
        /// </summary>
        /// <param name="filePath">Path to shader.</param>
        /// <param name="shaderType">ShaderType</param>
        /// <returns>
        /// Id of loaded shader.
        /// </returns>
        public int LoadShader(string filePath, ShaderType shaderType)
        {
            var id = GL.CreateShader(shaderType);
            LoadShader(filePath, id);
            return id;
        }

        private void LoadShader(string filePath, int shaderId)
        {
            var shaderCode = File.ReadAllText(filePath);
            GL.ShaderSource(shaderId, shaderCode);
            GL.CompileShader(shaderId);
            var info = GL.GetShaderInfoLog(shaderId);
            Console.Write(info);
            int shaderStatusCode;
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out shaderStatusCode);
            if (shaderStatusCode != 1) throw new ApplicationException(info);
        }
    }
}
