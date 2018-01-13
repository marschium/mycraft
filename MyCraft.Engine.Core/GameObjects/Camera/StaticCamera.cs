using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyCraft.Engine.GameObjects.Camera
{
    public class StaticCamera : ICamera
    {

        public StaticCamera(Vector3 postition, Vector3 target)
        {
            LookAt = Matrix4.LookAt(postition, target, new Vector3(0, 1, 0));
            Position = postition;
            Direction = target;
            Frustum = new Frustum();
        }

        public Matrix4 LookAt { get; }

        public Matrix4 Projection { get; } 

        public Vector3 Position { get; }

        public Vector3 Direction { get; }

        public Frustum Frustum { get; }

        public void Update(double timeDelta)
        {
        }

        public void UpdateProjection(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
