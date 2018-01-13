using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core.Items;
using MyCraft.Engine.Core.Util;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.Particles;
using MyCraft.Engine.Particles.Behaviours;
using MyCraft.Engine.World;
using MyCraft.Engine;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Core;
using MyCraft.Engine.Core.GameObjects;

namespace MyCraft.Engine.GameObjects
{
    public class Player : RaycasterGameObject
    {
        private KeyboardState _lastKeyboardState;
        private SelectedBlock _selectedMarker;

        private float _verticalRotationSpeed = 0.5f;
        private float _horizontalRotationSpeed = 0.5f;

        // TODO move into inventory class
        private Inventory _items;
        private GameObject _menu;

        public Player(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser, Inventory inventory) : base(blockMap, gameObjectInitaliser)
        {
            Speed = 48f;
            AffectedByGravity = false;
            IgnoreCollision = true;
            _selectedMarker = GameObjectInitaliser.Create<SelectedBlock>();
            Height =1.9f;
            Items = inventory;
        }

        public float Speed { get; set; }

        public float JumpVelocity { get; set; }
        public Inventory Items { get => _items; set => _items = value; }

        protected override void OnLoad()
        {
        }

        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
            HorizontalRotation += frameInfo.MouseInfo.HorizontalDelta * frameInfo.TimeDelta * _horizontalRotationSpeed * -1;
            var verticalDelta = frameInfo.MouseInfo.VerticalDelta * frameInfo.TimeDelta* _verticalRotationSpeed;
            if (VerticalRotation + verticalDelta > -1 && VerticalRotation + verticalDelta < 1)
            {
                VerticalRotation += verticalDelta;
            }

            // Handle mouse things
            if (frameInfo.MouseInfo.Clicked)
            {
                HandleMouseClick();
            }

            // TODO buffer to slow down the scrolling
            if (frameInfo.MouseInfo.WheelMovement != 0)
            {
                if (frameInfo.MouseInfo.WheelMovement > 0)
                {
                    Items.Next();
                }
                else
                {
                    Items.Previous();
                }
            }

            // TODO refactor raycasting into a series on components?
            // selected block and menu are in seperate components. raycasting is in a third component, the other two subsribe to an event?
            RunBlockInteractionRaycast(camera);
            MoveSelectedBlockGameObject();
            ShowBlockMenu();

            var keyboardState = Keyboard.GetState();
            if (AffectedByGravity && IsOnGround || (!AffectedByGravity))
            {
                float dx = 0;
                float dz = 0;
                float fluidSlowdownFactor = IsInFluid ? 2 : 1;
                if (keyboardState.IsKeyDown(Key.S))
                {
                    dz = Speed / fluidSlowdownFactor;
                }
                else if (keyboardState.IsKeyDown(Key.W))
                {
                    dz = -Speed / fluidSlowdownFactor;
                }

                if (keyboardState.IsKeyDown(Key.A))
                {
                    dx = -Speed / fluidSlowdownFactor;
                }
                else if (keyboardState.IsKeyDown(Key.D))
                {
                    dx = Speed / fluidSlowdownFactor;
                }
                if (keyboardState.IsKeyDown(Key.F) && _lastKeyboardState.IsKeyUp(Key.F))
                {
                    AffectedByGravity = !AffectedByGravity;
                    IgnoreCollision = !IgnoreCollision;
                    Speed = Speed == 48 ? 12 : 48;
                }

                var forward = camera.LookAt.Column2.Xyz * dz;
                var strafe = camera.LookAt.Column0.Xyz * dx;

                if (!IgnoreCollision)
                {
                    forward.Y = 0; // undo any vertical movement because of camera angle
                    strafe.Y = 0;
                }
                deltaMovement = forward + strafe;


                if (keyboardState.IsKeyDown(Key.Space) && _lastKeyboardState.IsKeyUp(Key.Space))
                {
                    if (IsInFluid)
                    {
                        deltaMovement.Y += 15; // TODO add fluid slowdown factor to blocks?
                    }
                    else
                    {
                        deltaMovement.Y += 30;
                    }
                }
            }

            _lastKeyboardState = keyboardState;
        }

        private void MoveSelectedBlockGameObject()
        {
            if (RaycastHit != null)
            {
                _selectedMarker.Position = WorldUtil.ToBlockPosition(RaycastHit.Location);
                _selectedMarker.GetComponent<ModelComponent>().Invisible = false;
            }
            else
            {
                _selectedMarker.GetComponent<ModelComponent>().Invisible = true;
            }
        }

        private void ShowBlockMenu()
        {
            // TODO should be seperate
            if (RaycastHit != null)
            {
                switch (RaycastHit.Block)
                {
                    case ExplosiveBlock explosiveBlock:
                        Console.WriteLine("Hit explosive block");
                        Vector3 offset = new Vector3(0.5f, 1.5f, 0.5f);
                        if (_menu == null)
                        {
                            _menu = GameObjectInitaliser.Create<Menu>(RaycastHit.Location + offset);
                        }
                        break;
                    default:
                        _menu?.Destroy();
                        _menu = null;
                        break;
                }
            }
        }
        

        //private void DestroyRaycastBlock()
        //{

        //}

        //private void PlaceWater()
        //{
        //    if (RaycastHit != null)
        //    {
        //        var blockSetup = new BlockSetup<WaterBlock>();
        //        blockSetup.With(b => b.Height = 1f);
        //        blockSetup.With(b => b.ScheduleUpdate());
        //        blockSetup.With(b => b.Source = b);
        //        World.World.Current.SetBlock<WaterBlock>(RaycastHit.Location + Vector3.UnitY, false, blockSetup);
        //    }
        //}

        //private void PlaceParticleEffect()
        //{
        //    if (RaycastHit != null)
        //    {
        //        var go = GameObject.Create<GameObject>();
        //        var effect = new SlimeTrailParticleBehaviour();
        //        var pc = new ParticleComponent(go, new Vector3(0.5f, 0.05f, 0.5f), effect) { Scale = 0.5f };
        //        pc.ParticleSystem.ParticleOrientation = ParticleOrientation.Floor;
        //        go.Components.Add(pc);
        //        go.Position = RaycastHit.Block.Position + Vector3.UnitY;
        //    }
        //}

        //private void PlaceSlime()
        //{
        //    if (RaycastHit != null)
        //    {
        //        var pos = RaycastHit.Location + (Vector3.UnitY * 2);
        //        GameObject.Create<GreenSlime>(pos);
        //    }
        //}

        private void HandleMouseClick()
        {
            // TODO if there is no menu override use the item
            Items.CurrentItem?.Use(this); // TODO empty object instead of null?
        }
    }
}
