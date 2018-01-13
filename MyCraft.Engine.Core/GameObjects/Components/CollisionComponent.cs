using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.GameObjects.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.GameObjects.Components
{
    public class CollisionComponent : AbstractComponent
    {
        public CollisionComponent(GameObject parent) : base(parent)
        {
        }

        public override void Update(FrameUpdateInfo frameInfo)
        {
            // TODO support bounding box/ multiple collision points

            if (Parent.AffectedByGravity && !Parent.IsOnGround)
            {
                if (Parent.IsInFluid)
                {
                     Parent.DeltaMovement.Y -= Physics.Gravity / 4f * (float)frameInfo.TimeDelta;
                }
                else
                {
                    Parent.DeltaMovement.Y -= Physics.Gravity * (float)frameInfo.TimeDelta;
                }
            }

            if (!Parent.IgnoreCollision)
            {
                if (!World.World.Current.BlockMatches(Position + new Vector3(Parent.DeltaMovement.X, 0, 0), b => b == null || !b.Collision))
                {
                    Parent.DeltaMovement.X = 0;
                }
                if (!World.World.Current.BlockMatches(Position + new Vector3(0, 0, Parent.DeltaMovement.Z), b => b == null || !b.Collision))
                {
                    Parent.DeltaMovement.Z = 0;
                }

                // if FrameMovement is > 0. then check using that (we must be falling)
                // otherwise check the block directly underneath
                var y = Parent.DeltaMovement.Y;
                if (y == 0)
                {
                    y = -0.5f;
                }

                // TODO check if we are currently in water and flag accordingly <- affected movement speed
                var blockBelow = World.World.Current.GetBlock(Position + new Vector3(0, y, 0));
                if (blockBelow == null) // replace with bounding box dimensions
                {
                    Parent.IsOnGround = false;
                    Parent.IsInFluid = false;
                }
                else if (blockBelow.IsFluid)
                {
                    Parent.IsOnGround = false;
                    Parent.IsInFluid = true;
                }
                else
                {
                    Parent.IsOnGround = true;
                    Parent.DeltaMovement.Y = -(Position.Y - (int)Position.Y);
                }
            }
        }
    }
}
