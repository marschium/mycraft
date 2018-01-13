using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects
{
    public class GameObjectDestroyedArgs : EventArgs
    {
        public GameObjectDestroyedArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public GameObject GameObject { get; }
    }
}
