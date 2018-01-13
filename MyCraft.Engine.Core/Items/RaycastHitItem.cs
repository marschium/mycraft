using MyCraft.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Items
{
    public abstract class RaycastHitItem : Item
    {

        public override void Use(GameObject owner)
        {
            var ownerAsRaycaster = (RaycasterGameObject)owner;
            if (ownerAsRaycaster.RaycastHit != null && ownerAsRaycaster.RaycastHit.Block != null)
            {
                OnRaycastHit(ownerAsRaycaster.RaycastHit); // TODO pass raycast as a parameter instead?
            }
        }

        protected abstract void OnRaycastHit(RayCastHit rayCastHit);
    }
}
