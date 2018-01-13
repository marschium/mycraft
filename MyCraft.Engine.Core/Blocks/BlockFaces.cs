using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public class BlockFaces
    {
        public static Vector3[] BackFace = new Vector3[]
{
            // back face
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 0f, 0f),
};

        public static Vector3[] RightFace = new Vector3[]
        {
            // right face
            
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 0f, 1f),
        };

        public static Vector3[] LeftFace = new Vector3[]
        {
            // left face
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 1f, 0f),
        };

        public static Vector3[] FrontFace = new Vector3[]
        { 
            // front face
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 1f),
        };

        public static Vector3[] TopFace = new Vector3[]
        {
            // top face
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 1f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 1f, 0f),
        };

        public static Vector3[] BottomFace = new Vector3[]
        {
            // bottom face
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, 1f),
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 1f),
            new Vector3(0f, 0f, 1f),
        };

        public static Vector2[] FaceTextureCoords = new Vector2[]
        {
            // top tri
            new Vector2(1f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),

            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),
        };

        public static Vector2[] FrontFaceTextureCoords = new Vector2[]
        {
            // top tri
            new Vector2(0f,1f),
            new Vector2(1f,1f),
            new Vector2(1f,0f),

            // bottom tri
            new Vector2(0f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),
        };
    }
}
