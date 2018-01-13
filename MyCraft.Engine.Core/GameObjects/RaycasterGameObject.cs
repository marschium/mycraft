using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core;
using MyCraft.Engine.GameObjects.Camera;
using OpenTK;
using MyCraft.Engine.World;
using MyCraft.Engine.Core.GameObjects;

namespace MyCraft.Engine.GameObjects
{
    /// <summary>
    /// Represents a GameObject that can raycast to hit blocks
    /// </summary>
    public class RaycasterGameObject : GameObject
    {
        public RaycasterGameObject(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser) 
            : base(blockMap, gameObjectInitaliser)
        {
        }

        public RayCastHit RaycastHit { get; set; }

        protected void RunBlockInteractionRaycast(ICamera camera)
        {
            // cursor raycast
            RaycastHit = RayCasting.RayCast(BlockMap, camera.Position, camera.Direction, 5, b => b != null && b.GetType() != typeof(WaterBlock));
        }
    }
}