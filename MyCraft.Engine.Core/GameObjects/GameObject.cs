using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core.GameObjects;
using MyCraft.Engine.GameObjects.Ai;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.World;
using MyCraft.Util;
using NLog;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects
{
    public class GameObject
    {

        protected static ILogger Logger = LogManager.GetCurrentClassLogger();

        private bool _rotationChanged;
        private Vector3 _direction;

        private double _horizontalRotation;
        private double _verticalRotation;


        protected Vector3 deltaMovement;

        public GameObject(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser)
        {
            GameObjectInitaliser = gameObjectInitaliser;
            Components = new List<AbstractComponent>();
            BlockMap = blockMap;
            _rotationChanged = true;
        }

        public BlockMap BlockMap { get; }

        protected GameObjectInitaliser GameObjectInitaliser { get; }

        public static IUpdateScheduler UpdateScheduler { get; set; }

        public event EventHandler<GameObjectDestroyedArgs> OnDestroy;

        public Vector3 Position { get; set; }

        public float Height { get; set; }

        public bool AffectedByGravity { get; set; }

        public bool IsOnGround { get; set; }

        public bool IsInFluid { get; set; }

        public bool IgnoreCollision { get; set; }

        public bool IsLoaded { get; private set; }

        public bool Destroyed { get; private set; }

        public bool IsUnderwater { get; private set; }

        public ICollection<AbstractComponent> Components { get; }

        public Vector3 FrameMovementDelta { get; protected set; }

        public Vector3 Direction
        {
            get
            {
                if (_rotationChanged)
                {
                    _direction = new Vector3(
                        (float)(Math.Cos(VerticalRotation) * Math.Sin(HorizontalRotation)),
                        (float)(Math.Sin(VerticalRotation)),
                        (float)(Math.Cos(VerticalRotation) * Math.Cos(HorizontalRotation))
                    );
                }
                _rotationChanged = false;
                return _direction;
            }
            set
            {
                var theta = Math.Atan2(value.X, value.Z);
                HorizontalRotation = theta;
                VerticalRotation = Math.Atan2(value.Y, value.X);
                _direction = value;
            }
        }

        public double HorizontalRotation
        {
            get
            {
                return _horizontalRotation;
            }
            set
            {
                _rotationChanged = true;
                _horizontalRotation = value;
            }
        }

        public double VerticalRotation
        {
            get
            {
                return _verticalRotation;
            }
            set
            {
                _rotationChanged = true;
                _verticalRotation = value;
            }
        }

        public T GetComponent<T>()
            where T : AbstractComponent
        {
            return (T)Components.Where(t => t.GetType() == typeof(T)).FirstOrDefault();
        }

        public void Update(ICamera camera, FrameUpdateInfo frameInfo)
        {
            foreach(var c in Components)
            {
                if (!c.Destroyed)
                {
                    c.Update(frameInfo);
                }
            }

            // TODO move a collision component
            if (AffectedByGravity && !IsOnGround)
            {
                if (IsInFluid)
                {
                    deltaMovement.Y -= Physics.Gravity / 4f;
                }
                else
                {
                    deltaMovement.Y -= Physics.Gravity;
                }
            }

            OnUpdate(camera, frameInfo);
            FrameMovementDelta = deltaMovement * (float)frameInfo.TimeDelta;
            CheckCollision();
            Position += FrameMovementDelta;

            // TODO move to component?
            IsUnderwater = false;
            if (BlockMap.BlockMatches(Position + (Vector3.UnitY * Height), b => b != null && b.GetType() == typeof(WaterBlock)))
            {
                IsUnderwater = true;
            }

            // if components are all destroyed -> delete this game object
            // TODO better way of destroying and removing components
            if (Components.Count > 0 && Components.All(c => c.Destroyed))
            {
                Destroy();
            }
        }

        public void Load()
        {
            OnLoad();
            foreach (var component in Components)
            {
                component.Load();
            }
            IsLoaded = true;
        }

        public void Destroy()
        {
            Destroyed = true;
            byte o;
            if (OnDestroy != null) OnDestroy(this, new GameObjectDestroyedArgs(this));
            Console.WriteLine($"GameObject destroyed: {this}");
        }

        protected virtual void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {

        }

        protected virtual void OnLoad()
        {

        }

        private void CheckCollision()
        {
            // TODO support bounding box/ multiple collision points
            var deltaX = FrameMovementDelta.X;
            var deltaY = FrameMovementDelta.Y;
            var deltaZ = FrameMovementDelta.Z;
            if (!IgnoreCollision)
            {
                if (!BlockMap.BlockMatches(Position + new Vector3(FrameMovementDelta.X, 0, 0), b => b == null || !b.Collision))
                {
                    deltaX = 0;
                }
                if (!BlockMap.BlockMatches(Position + new Vector3(0, 0, FrameMovementDelta.Z), b => b == null || !b.Collision))
                {
                    deltaZ = 0;
                }

                // if FrameMovement is > 0. then check using that (we must be falling)
                // otherwise check the block directly underneath
                var y = FrameMovementDelta.Y;
                if (y == 0)
                {
                    y = -0.5f;
                }

                // TODO check if we are currently in water and flag accordingly <- affected movement speed
                var blockBelow = BlockMap.GetBlock(Position + new Vector3(0, y, 0));
                if (blockBelow == null) // replace with bounding box dimensions
                {
                    IsOnGround = false;
                    IsInFluid = false;
                }
                else if (blockBelow.IsFluid)
                {
                    IsOnGround = false;
                    IsInFluid = true;
                }
                else
                {
                    IsOnGround = true;
                    deltaY = -(Position.Y - (int)Position.Y);
                }
            }

            FrameMovementDelta = new Vector3(deltaX, deltaY, deltaZ);
        }
    }
}
