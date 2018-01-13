using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core.Util;
using MyCraft.Engine.World;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public class WaterBlock : Block, IUpdateBlock
    {
        private Vector3[] _scaledFrontFace;
        private bool _generatedFrontFace;

        private Vector3[] _scaledBackFace;
        private bool _generatedBackFace;

        private Vector3[] _scaledRightFace;
        private bool _generatedRightFace;

        private Vector3[] _scaledLeftFace;
        private bool _generatedLeftFace;

        private Vector3[] _scaledTopFace;
        private bool _generatedTopFace;

        private Vector3[] _scaledBottomFace;
        private bool _generatedBottomFace;

        private float _height = 0.9f;
        private bool _stopUpdates = false;

        public WaterBlock()
        {
            IsTransparent = true;
            IsFluid = true;
            Collision = false;
        }

        public override Vector3[] FrontFace {
            get {
                if (!_generatedFrontFace)
                {
                    _scaledFrontFace = ScaleFace(base.FrontFace);
                    _generatedFrontFace = true;
                }
                return _scaledFrontFace;
            }
            protected set
            {
                _scaledFrontFace = value;
            }
        }

        public override Vector3[] BackFace
        {
            get
            {
                if (!_generatedBackFace)
                {
                    _scaledBackFace = ScaleFace(base.BackFace);
                    _generatedBackFace = true;
                }
                return _scaledBackFace;
            }
            protected set
            {
                _scaledBackFace= value;
            }
        }

        public override Vector3[] RightFace
        {
            get
            {
                if (!_generatedRightFace)
                {
                    _scaledRightFace = ScaleFace(base.RightFace);
                    _generatedRightFace = true;
                }
                return _scaledRightFace;
            }
            protected set
            {
                _scaledRightFace = value;
            }
        }

        public override Vector3[] LeftFace
        {
            get
            {
                if (!_generatedLeftFace)
                {
                    _scaledLeftFace = ScaleFace(base.LeftFace);
                    _generatedLeftFace = true;
                }
                return _scaledLeftFace;
            }
            protected set
            {
                _scaledLeftFace = value;
            }
        }

        public override Vector3[] TopFace
        {
            get
            {
                if (!_generatedTopFace)
                {
                    _scaledTopFace = ScaleFace(base.TopFace);
                    _generatedTopFace = true;
                }
                return _scaledTopFace;
            }
            protected set
            {
                _scaledTopFace = value;
            }
        }

        public override Vector3[] BottomFace
        {
            get
            {
                if (!_generatedBottomFace)
                {
                    _scaledBottomFace = ScaleFace(UnscaledBottomFace);
                    _generatedBottomFace = true;
                }
                return _scaledBottomFace;
            }
            protected set
            {
                _scaledBottomFace = value;
            }
        }

        private static Vector3[] UnscaledBottomFace = new Vector3[]
        {
            // bottom face
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 1f),
        };

        public override TextureName BackFaceTexture => TextureName.WaterSide;

        public override TextureName FrontFaceTexture => TextureName.WaterSide;

        public override TextureName LeftFaceTexture => TextureName.WaterSide;

        public override TextureName RightFaceTexture => TextureName.WaterSide;

        public override TextureName TopFaceTexture => TextureName.Water;

        public override TextureName BottomFaceTexture => TextureName.Water;

        public override float FireFuel => 0;

        public float Height {
            get
            {
                return _height;
            }
            set
            {
                _generatedBackFace = false;
                _generatedFrontFace = false;
                _generatedLeftFace = false;
                _generatedRightFace = false;
                _generatedTopFace = false;
                _height = value;
            }
        }

        public WaterBlock Source { get; set; }

        public ICollection<WaterBlock> Children { get; } = new List<WaterBlock>(5);

        public void ScheduleUpdate()
        {
            Scheduler.Schedule(TimeSpan.FromSeconds(1), () => this.Update());
        }

        public void OnRemove()
        {
            _stopUpdates = true;
            foreach(var child in Children)
            {
                child.Source = null;
                child.ScheduleUpdate();
            }
        }

        public BlockMap BlockMap;

        // TODO move into a specific class?
        public void Update()
        {
            if (_stopUpdates)
            {
                return;
            }

            if (Source == null)
            { 
                // TODO look for another source block?
                Height -= 0.05f;
                if (Height <= 0f)
                {
                    BlockMap.SetBlock(Position, null, true); // TODO check that something else hasn't been put here since the last update. maybe a a check early on?
                    return;
                }
                else
                {
                    ScheduleUpdate();
                }
            }

            // TODO maybe scheudler should be a param so that there are not tons of refs being unused
            var skipNeighbours = FlowDownwards();

            if (!skipNeighbours)
            {
                FlowtoPosition(Position + Vector3.UnitZ);
                FlowtoPosition(Position - Vector3.UnitZ);
                FlowtoPosition(Position + Vector3.UnitX);
                FlowtoPosition(Position - Vector3.UnitX);
            }
        }

        private bool FlowDownwards()
        {
            var pos = Position - Vector3.UnitY;
            var block = BlockMap.GetBlock(pos);
            if (block == null)
            {
                var blockSetup = new BlockSetup<WaterBlock>();
                blockSetup.With(b => b.BlockMap = this.BlockMap);
                blockSetup.With(b => b.Scheduler = this.Scheduler);
                blockSetup.With(b => b.Height = 1f);
                blockSetup.With(b => b.ScheduleUpdate());
                blockSetup.With(b => b.Source = this);
                blockSetup.With(b => Children.Add(b));
                BlockMap.SetBlock<WaterBlock>(pos, false, blockSetup);
                return true;
            }
            else if (block.GetType() == typeof(WaterBlock))
            {
                var waterBlock = block as WaterBlock;
                waterBlock.Height = 1f;
                // TODO override existing source block?
            }

            return false;
        }

        private void FlowtoPosition(Vector3 position)
        {
            var newHeight = Height - 0.2f;
            var block = BlockMap.GetBlock(position);

            // check that our source block isn't over air and we aren't directly beneath if
            if (block == null && newHeight > 0.1f)
            {
                var blockSetup = new BlockSetup<WaterBlock>();
                blockSetup.With(b => b.BlockMap = this.BlockMap);
                blockSetup.With(b => b.Scheduler = this.Scheduler);
                blockSetup.With(b => b.Height = newHeight);
                blockSetup.With(b => b.ScheduleUpdate());
                blockSetup.With(b => b.Source = this);
                blockSetup.With(b => Children.Add(b));
                BlockMap.SetBlock<WaterBlock>(position, false, blockSetup);
            }
            else if (block is WaterBlock waterBlock && waterBlock.Height < newHeight)
            {
                waterBlock.Height = newHeight;
                waterBlock.ScheduleUpdate();
            }
        }

        private Vector3[] ScaleFace(Vector3[] originalFace)
        {
            var scaledFace = new Vector3[originalFace.Length];
            for (int i = 0; i < originalFace.Length; i++)
            {
                var v = originalFace[i];
                scaledFace[i] = new Vector3(v.X, v.Y * Height, v.Z);
            }
            return scaledFace;
        }
    }
}
