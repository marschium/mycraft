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
    public class FirstPersonCamera : ICamera
    {
        private GameObject _target;
        private Vector3 _offset;
        private Matrix4 _projection;

        public FirstPersonCamera(GameObject target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
            Frustum = new Frustum();
        }

        public Matrix4 LookAt { get; private set; }

        public Matrix4 Projection { get { return _projection; } private set { _projection = value; } }

        public Vector3 Position { get; private set; }

        public Vector3 Direction { get { return _target.Direction; } }

        public Frustum Frustum { get; }
        
        public void Update(double timeDelta)
        {
            Position = _target.Position + _offset;
            LookAt = Matrix4.LookAt(Position, Position + _target.Direction, Vector3.UnitY);
            Frustum.CalculateFrustum(Projection, LookAt);
        }

        public void UpdateProjection(int width, int height)
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(1f, width / (float)height, 0.5f, 1000f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref _projection);
        }
    }
}
