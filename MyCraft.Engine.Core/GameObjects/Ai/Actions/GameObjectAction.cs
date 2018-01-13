using MyCraft.Engine.Abstract;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects.Ai.Actions
{
    public abstract class GameObjectAction<T>
        where T : GameObject
    {
        public GameObjectAction(T source)
        {
            Source = source;
        }

        protected T Source { get; }

        public abstract bool Act(FrameUpdateInfo frameInfo);
    }
}
