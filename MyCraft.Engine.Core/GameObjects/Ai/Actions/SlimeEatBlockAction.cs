using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.World;
using OpenTK;

namespace MyCraft.Engine.GameObjects.Ai.Actions
{
    public class SlimeEatBlockAction : GameObjectAction<GreenSlime>
    {
        private Chunk _currentChunk;
        private bool _targetSet;
        private Vector3 _targetPos;
        private SlimeMoveToPositionAction _moveAction;

        public SlimeEatBlockAction(GreenSlime source) : base(source)
        {
            _currentChunk = source.BlockMap.GetChunk(source.Position, false);
        }

        public override bool Act(FrameUpdateInfo frameInfo)
        {
            // TODO : maybe we should only eat if there is something nearby?

            // find a suitable block to eat
            // move to it
            //eat it

            if (!_targetSet)
            {
                // TODO do something if there is no block
                _targetPos = _currentChunk.TopLevelBlocks.Where(b => b?.GetType() == typeof(GrassBlock)).First().Position;
                _moveAction = new SlimeMoveToPositionAction(Source, _targetPos);
                _targetSet = true;
            }
            else
            {
                var moveCompleted = _moveAction.Act(frameInfo);
                if (moveCompleted)
                {
                    Source.BlockMap.SetBlock<DirtBlock>(_targetPos, true);
                    return true;
                }
            }
            return false;
        }
    }
}
