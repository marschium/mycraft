using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Ui;
using MyCraft.Engine.World;
using MyCraft.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.World.Events;
using MyCraft.Engine.GameObjects.Ai;
using MyCraft.Engine.Particles;
using MyCraft.Engine.Particles.Behaviours;
using MyCraft.Engine.GameObjects.Components;
using System.Linq;
using System.Diagnostics;
using MyCraft.Engine.Core.GameObjects;
using Ninject;
using System.Buffers;

namespace MyCraft.Engine
{
    public class GameEngine : GameWindow
    {
        public static int MvpId { get; private set; }

        private static Vector3 UnderwaterFogColour = new Vector3(0.3f, 0.6f, 1f);

        private int _vertexArrayId;
        private int _textureId;

        private ChunkRenderer _chunkRenderer;
        private ChunkLoader _chunkLoader;
        private GameObjectModelRenderer _gameObjectRenderer;
        private ParticleSystemRenderer _particleRenderer;
        private IUpdateScheduler _updateScheduler;

        private GameObjectInitaliser _gameObjectInitaliser;
        private Player _player;
        private World.BlockMap _world;
        private GameInterface _ui;
        
        private double _mouseXDelta;
        private double _mouseYDelta;
        private bool _mouseClicked;
        private Vector2 _lastMousePos;
        private float _mouseWheelDelta;

        private double _timeSinceLastWindowUpdate;
        private bool _intialChunkLoadComplete;
        private Stopwatch _runtimeTimer;

        public GameEngine(IKernel kernel) : base(1600, 900)
        {
            // TODO sort this mess out

            _world = kernel.Get<BlockMap>();
            _gameObjectInitaliser = kernel.Get<GameObjectInitaliser>();
            _ui = kernel.Get<GameInterface>();


            var shaderLoader = new ShaderLoader();
            CursorVisible = false;
            _chunkRenderer = new ChunkRenderer(shaderLoader, kernel.Get<TextureLoader>());
            _lastMousePos = Vector2.Zero;

            // create before any game objects
            _updateScheduler = new UpdateScheduler();
            GameObject.UpdateScheduler = _updateScheduler;

            _player = _gameObjectInitaliser.Create<Player>(new Vector3(0, 56, 0));

            // _player = GameObject.Create<Player>();
            var cloudGenerator = _gameObjectInitaliser.Create<CloudGenerator>(new Vector3(0, 0, 0));
            cloudGenerator.Player = _player;

            Camera.MainCamera = new FirstPersonCamera(_player, new Vector3(0, 1.8f, -0.2f));

            // TODO sort out. replace with inject factory func
            
            _chunkLoader = new ChunkLoader(
                _world, 
                6, 
                (c) => new ChunkMeshBuilder(_world, kernel.Get<TextureLoader>(), c, new ChunkLighting(), kernel.Get<ArrayPool<Vector3>>(), kernel.Get<ArrayPool<Vector2>>()),
                _player,
                kernel.Get<ArrayPool<Vector3>>(),
                kernel.Get<ArrayPool<Vector2>>());

            _gameObjectRenderer = new GameObjectModelRenderer(shaderLoader);
            _particleRenderer = new ParticleSystemRenderer(shaderLoader);

            _runtimeTimer = Stopwatch.StartNew();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LogSetup.ApplyConfig();

            // Setup depth testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            // setup backface culling
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);

            // Set background color
            GL.ClearColor(0.3f, 0.6f, 1f, 0f);

            // VOA
            GL.GenVertexArrays(1, out _vertexArrayId);
            GL.BindVertexArray(_vertexArrayId);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);


            // setup input
            Keyboard.KeyDown += ExitOnEsc;

            // start chunk loading task
            _chunkLoader.StartMonitoringChunks();
            _chunkLoader.InitialLoadComplete += (o, q) => _intialChunkLoadComplete = true;
            _chunkLoader.InitialLoadComplete += (o, q) => Console.Write($"Completed inital load. {_runtimeTimer.Elapsed}\n");
            _ui.Load();

            _updateScheduler.Start(); // TODO maybe capture the task ref
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            // Setup projection matrix
            Camera.MainCamera.UpdateProjection(Width, Height);
        }

        private void RenderChunkModel(ChunkModel model, Vector3 position, FrameRenderInfo renderInfo)
        {
            if (model != null)
            {
                if (!model.LoadedIntoGL)
                {
                    _chunkLoader.LoadChunkModel(model);
                }

                if (Camera.MainCamera.Frustum.VolumeVsFrustum(position.X + (Chunk.Width / 2), position.Y + (Chunk.Width / 2), position.Z + (Chunk.Width / 2), Chunk.Width / 2, Chunk.Height / 2, Chunk.Width / 2))
                {
                    _chunkRenderer.DrawModel(model, position, renderInfo);
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //render
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var fogDensity = 0.008f;
            if (_player.IsUnderwater)
            {
                GL.ClearColor(UnderwaterFogColour.X, UnderwaterFogColour.Y, UnderwaterFogColour.Z, 0f);
                fogDensity = (Chunk.WaterLevel - _player.Position.Y) * 0.05f;
            }
                

            var renderInfo = new FrameRenderInfo
            {
                MvpId = MvpId,
                Fog = fogDensity,
                FogColour = UnderwaterFogColour,
            };

            // bind texture for chunks TODO move into chunk code
            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            // draw solids
            foreach (var mp in _chunkLoader.LoadedChunks.Select(c => new { Model = c.SolidModel, c.Position } ))
            {
                RenderChunkModel(mp.Model, mp.Position, renderInfo);
            }

            // draw transparents
            GL.Disable(EnableCap.CullFace);
            foreach (var mp in _chunkLoader.LoadedChunks.Select(c => new { Model = c.FloraModel, c.Position }))
            {
                RenderChunkModel(mp.Model, mp.Position, renderInfo);
            }

            foreach (var mp in _chunkLoader.LoadedChunks.Select(c => new { Model = c.TransparentModel, c.Position }))
            {
                RenderChunkModel(mp.Model, mp.Position, renderInfo);
            }
            GL.Enable(EnableCap.CullFace);

            // draw gameobjects
            //if (_intialChunkLoadComplete)
            {
                foreach (var kv in _gameObjectInitaliser.ActiveObjects)
                {
                    var obj = kv.Key;
                    foreach (var component in obj.Components)
                    {
                        // TODO: factory and in a different project
                        if (component.GetType() == typeof(ModelComponent))
                        {
                            _gameObjectRenderer.Render((ModelComponent)component, Camera.MainCamera, renderInfo);
                        } 
                        else if (component.GetType() == typeof(ParticleComponent))
                        {
                            _particleRenderer.DrawParticleSystem((ParticleComponent)component, renderInfo, Camera.MainCamera);
                        }
                    }
                }
            }

            // framecounter
            _timeSinceLastWindowUpdate += e.Time;
            if (_timeSinceLastWindowUpdate > 1)
            {
                _timeSinceLastWindowUpdate = 0;
                var framerate = 1 / e.Time;
                Title = $"{framerate.ToString()} ({_player.Position})({_player.HorizontalRotation}, {_player.VerticalRotation})";
            }

            // UI
            _ui.Render();

            SwapBuffers();
        }

        protected override void OnDisposed(EventArgs e)
        {
            base.OnDisposed(e);

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // input
            base.OnUpdateFrame(e);

            Camera.MainCamera.Update(e.Time);

            // focused
            if (Focused)
            {
                var newMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                Vector2 delta = newMousePos - _lastMousePos;
                _mouseXDelta += delta.X;
                _mouseYDelta -= delta.Y;
                OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
                _lastMousePos = newMousePos;
            }

            var mouseInfo = new MouseMovementInfo {
                HorizontalDelta = _mouseXDelta,
                VerticalDelta = _mouseYDelta,
                Clicked = _mouseClicked,
                WheelMovement = _mouseWheelDelta
            };
            var frameInfo = new FrameUpdateInfo(e.Time, mouseInfo);

            //if (_intialChunkLoadComplete)
            {
                while(!_gameObjectInitaliser.UnloadedObjects.IsEmpty)
                {
                    _gameObjectInitaliser.UnloadedObjects.TryTake(out var o);
                    o.Load();
                }
                foreach (var kv in _gameObjectInitaliser.ActiveObjects)
                {
                    kv.Key.Update(Camera.MainCamera, frameInfo);
                }
            }

            _mouseXDelta = 0;
            _mouseYDelta = 0;
            _mouseClicked = false;
            _mouseWheelDelta = 0;

            _ui.Update(frameInfo);
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2); // reset mouse position
        }

        private void ExitOnEsc(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Environment.Exit(0);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            _mouseClicked = true;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _mouseWheelDelta = e.DeltaPrecise;
        }
    }
}
