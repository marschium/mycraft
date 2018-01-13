using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.World;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.GameObjects
{
    /// <summary>
    /// Generates cloud near the player.
    /// </summary>
    public class CloudGenerator : GameObject
    {
        private static int minY = 50;
        private static int maxY = 56;
        private static float maxDistance = 128;
        private static int maxClouds = 25;

        private List<Cloud> _clouds = new List<Cloud>();
        private Random _random;

        public CloudGenerator(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser) : base(blockMap, gameObjectInitaliser)
        {
        }

        public Player Player { get; set; }

        protected override void OnLoad()
        {
            base.OnLoad();

            _random = new Random();
            for(int i = 0; i < maxClouds; i++)
            {
                CreateCloud();
            }
        }

        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
            base.OnUpdate(camera, frameInfo);
            List<Cloud> _cloudsToRemove = new List<Cloud>();
            foreach(var cloud in _clouds)
            {
                if ((cloud.Position - Player.Position).LengthFast > maxDistance)
                {
                    _cloudsToRemove.Add(cloud);
                }
            }
            foreach(var cloud in _cloudsToRemove)
            {
                _clouds.Remove(cloud);
                cloud.Destroy();
                if (_clouds.Count < maxClouds)
                {
                    CreateCloud();
                }
            }
        }

        private void CreateCloud()
        {
            var x = _random.Next(-64, 64);
            var z = _random.Next(-64, 64);
            var y = _random.Next(minY, maxY);
            _clouds.Add(GameObjectInitaliser.Create<Cloud>(new Vector3(Player.Position.X + x, y, Player.Position.Z + z)));
        }
    }
}
