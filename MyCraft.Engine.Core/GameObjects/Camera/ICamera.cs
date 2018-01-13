using MyCraft.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects.Camera
{
    public interface ICamera
    {
        Matrix4 LookAt { get; }
        Matrix4 Projection { get; }
        Vector3 Position { get; }
        Vector3 Direction { get; }
        Frustum Frustum { get; } 

        void Update(double timeDelta);
        void UpdateProjection(int width, int height);
    }
}
