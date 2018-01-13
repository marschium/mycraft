using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using MyCraft.Engine.Abstract;

namespace MyCraft.Engine.GameObjects.Ai.Actions
{
    public class SlimeMoveToPositionAction : GameObjectAction<GreenSlime>
    {
        private double _d;

        public SlimeMoveToPositionAction(GreenSlime source, Vector3 position) : base(source)
        {
            TargetPosition = position;
        }
        
        public Vector3 TargetPosition { get; }

        public override bool Act(FrameUpdateInfo frameInfo)
        {
            // if not facing the correct way, rotate
            // if facing the correct direction, jump

            if (!IsFacingCorrectDirection())
            {
                // TODO always rotates the same way atm
                if (_d == 0)
                {
                    _d = GetRotationDirection();
                }
                // TODO if rotation is small enough, do it on the spot
                Source.RotateInAir((float)(_d *  frameInfo.TimeDelta));
            }
            else
            {
                // TODO : controlled jumps or slide if we are close enough
                var v = TargetPosition.Xz - Source.Position.Xz;
                if (v.Length > 2)
                {
                    Source.Jump();
                }
                else
                {
                    Source.Slide();
                }
            }

            if (IsInPosition())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private float GetRotationDirection()
        {
            var a = Source.Position;
            var b = Source.Position + Source.Direction;
            var c = Source.Position + (TargetPosition - Source.Position);
            // we have three points on a triangle. find the angle bac
            var ba = (b - a).Xz.Length;
            var bc = (b - c).Xz.Length;
            var ca = (c - a).Xz.Length;
            var t = ((ca * ca) + (ba * ba) - (bc * bc)) / (2f * ca * ba);
            var r = Math.Acos(t);

            var d = 1;
            if (r <= 0 || r >= MathHelper.PiOver2)
            {
                d = d * -1;
            }
            return d;
        }

        private bool IsFacingCorrectDirection()
        {
            var targetDirection = ((TargetPosition - Source.Position).Xz.Normalized());
            var diff = Source.Direction.Xz - targetDirection;
            return (diff.X > -0.1 && diff.X < 0.1) && (diff.Y > -0.1 && diff.Y < 0.1);
        }

        private bool IsInPosition()
        {
            var diff = (TargetPosition.Xz - Source.Position.Xz).Length;
            return diff < 1;
        }
    }
}
