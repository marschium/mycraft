using MyCraft.Assets;
using MyCraft.Engine.Core.GameObjects;
using MyCraft.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Items
{
    public class FireStarter : RaycastHitItem
    {
        private GameObjectInitaliser _gameObjectInitiliser;

        public FireStarter(GameObjectInitaliser gameObjectInitaliser)
        {
            _gameObjectInitiliser = gameObjectInitaliser;
        }

        public override TextureName InventoryIconTexture => TextureName.Fire1;

        protected override void OnRaycastHit(RayCastHit rayCastHit)
        {
            if (!rayCastHit.Block.IsOnFire)
            {
                rayCastHit.Block.Temperature = 100;
                rayCastHit.Block.IsOnFire = true; // TODO helper so that I don't to remember to set to true every time
                _gameObjectInitiliser.Create<Fire>(rayCastHit.Block.Position);
            }
        }
    }
}
