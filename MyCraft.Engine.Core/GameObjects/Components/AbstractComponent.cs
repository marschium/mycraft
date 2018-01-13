using MyCraft.Engine.Abstract;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects.Components
{
    public abstract class AbstractComponent
    {
        private Vector3 _offset;

        public AbstractComponent(GameObject parent) : this(parent, Vector3.Zero)
        {
        }

        public AbstractComponent(GameObject parent, Vector3 offset)
        {
            Parent = parent;
            _offset = offset;
        }

        public GameObject Parent { get; }

        public float Scale { get; set; } = 1;

        public bool Invisible { get; set; } = false;

        public Vector3 Position => Parent.Position + _offset;

        public virtual void Load() { }

        public virtual void Update(FrameUpdateInfo frameInfo) { }

        public bool Destroyed { get; protected set; } // TODO components should be in a thread safe collection so that they can remove themselves
    }
}
