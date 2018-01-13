using MyCraft.Engine.GameObjects;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.GameObjects
{
    public class GameObjectInitaliser
    {
        private readonly Func<Type, GameObject> _gameObjectFactory;

        public ConcurrentDictionary<GameObject, byte> ActiveObjects = new ConcurrentDictionary<GameObject, byte>();

        public ConcurrentBag<GameObject> UnloadedObjects = new ConcurrentBag<GameObject>();

        public GameObjectInitaliser(Func<Type, GameObject> gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public T Create<T>()
            where T : GameObject
        {
            return Create<T>(Vector3.Zero);
        }

        public T Create<T>(Vector3 position)
            where T : GameObject
        {
            var t =_gameObjectFactory(typeof(T));
            t.Position = position;
            t.OnDestroy += (s, a) => ActiveObjects.TryRemove(a.GameObject, out byte b);
            ActiveObjects[t] = 1;
            UnloadedObjects.Add(t);
            return (T)t;
        }
    }
}
