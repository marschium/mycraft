using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World
{
    public enum ModelType
    {
        Solid,
        Water,
        Flora
    }

    public class ChunkModel
    {
        public ChunkModel(Vector3[] vertices, Vector2[] uv, Vector3[] lightMap, int verticesCount)
        {
            VerticesCount = verticesCount;
            Vertices = vertices;
            UV = uv;
            Lighting = lightMap;
        }

        public Vector3[] Vertices { get; set; }

        public Vector2[] UV { get; set; }

        public Vector3[] Lighting { get; set; }

        public int VertexBufferId { get; set; }

        public int TextureBufferId { get; set; }

        public int LightDataBufferId { get; set; }

        public bool LoadedIntoGL { get; set; }

        public ModelType ModelType { get; set; }

        public int VerticesCount { get; set; }
    }
}
