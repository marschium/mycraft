using MyCraft.Engine.Events;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.World;
using MyCraft.Engine.World.Events;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCraft.Engine
{
    /// <summary>
    /// Keeps track of the chunks that should be draw.
    /// Makes sure that each chunk has a model.
    /// </summary>
    public class ChunkLoader
    {
        private BlockMap _blockMap;
        private int _chunkLoadDistance;
        private Player _player;
        private IList<Chunk> _loadedChunks;
        private bool _initialLoadComplete;
        private ArrayPool<Vector3> _vec3Pool;
        private ArrayPool<Vector2> _vec2Pool;

        private Func<Chunk, ChunkMeshBuilder> _meshBuilderFactory;

        public ChunkLoader(BlockMap blockMap, int chunkLoadDistance, Func<Chunk, ChunkMeshBuilder> meshBuilderFactory, Player player, ArrayPool<Vector3> vec3Pool, ArrayPool<Vector2> vec2Pool)
        {
            _blockMap = blockMap;
            _chunkLoadDistance = chunkLoadDistance;
            _player = player;
            _loadedChunks = new List<Chunk>();
            _meshBuilderFactory = meshBuilderFactory;
            _vec3Pool = vec3Pool;
            _vec2Pool = vec2Pool;
        }

        public event EventHandler<InitialChunkLoadCompleteEventArgs> InitialLoadComplete;
        public IList<Chunk> LoadedChunks { get { return _loadedChunks; } }

        public void StartMonitoringChunks()
        {
            LoadChunksAroundPlayer();
            // UnloadUnusedChunks();
        }

        public void HandleChunkModified(object sender, ChunkModifiedEventArgs args)
        {
            // TODO load these chunks
            //var chunk = args.Chunk;
            //ChunksAwaitingModelLoad.Add(chunk);
        }

        private Task LoadChunksAroundPlayer()
        {
            // TODO this should only load chunks in the frustrum
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    while (true)
                    {
                        var t = DateTime.UtcNow - TimeSpan.FromSeconds(10);
                        var currentTime = DateTime.UtcNow;
                        var loadedChunks = new List<Chunk>();
                        var pos = _player.Position;
                        var currentChunkPos = WorldUtil.FloorToChunkPosition(pos);

                        // anchor all of the chunk near the player
                        for (int x = _chunkLoadDistance; x > _chunkLoadDistance * -1; x--)
                        {
                            for (int z = _chunkLoadDistance; z > _chunkLoadDistance * -1; z--)
                            {
                                var worldCoord = new Vector3(currentChunkPos.X + x * Chunk.Width, 0, currentChunkPos.Z + z * Chunk.Width);
                                // only bother with stuff in the camera frustrum
                                var inCamera = Camera.MainCamera.Frustum.VolumeVsFrustum(worldCoord.X + (Chunk.Width / 2), worldCoord.Y + (Chunk.Width / 2), worldCoord.Z + (Chunk.Width / 2), Chunk.Width / 2, Chunk.Height / 2, Chunk.Width / 2);

                                var chunk = _blockMap.LoadChunkWithTerrain(worldCoord);
                                chunk.LastModifiedTime = currentTime; // mark the chunk as modified to stop it from being unloaded
                                if (inCamera && chunk.HasChanged)
                                {
                                    Console.WriteLine($"Generating model for chunk {chunk.Position}");
                                    GenerateChunkModel(chunk);
                                }

                                loadedChunks.Add(chunk);
                            }
                        }

                        if (!_initialLoadComplete)
                        {
                            InitialLoadComplete(this, new InitialChunkLoadCompleteEventArgs());
                            _initialLoadComplete = true;
                        }

                        // check all chunks to see what should be drawn
                        foreach (var kv in _blockMap.Chunks.ToArray())
                        {
                            if (kv.Value.LastModifiedTime < t)
                            {
                                Chunk c;
                                Console.WriteLine($"Removing chunk {kv.Key}");
                                var x = _blockMap.Chunks.TryRemove(kv.Key, out c);
                                c?.Clear();
                                if (c.SolidModel != null)
                                {
                                    UnloadChunkModel(c.SolidModel);
                                    c.SolidModel = null;
                                }
                            }
                        }

                        Interlocked.Exchange(ref _loadedChunks, loadedChunks);

                        Thread.Sleep(TimeSpan.FromMilliseconds(50)); // TODO could be event based
                    }
                }
                catch (Exception e)
                {
                    var x = e.Message;
                    Debugger.Break();
                }
            });
        }

        public void GenerateChunkModel(Chunk chunk)
        {
            if (chunk.HasChanged)
            {
                // store the current chunk model (if there is one)
                var oldModel = chunk.SolidModel;

                var builder = _meshBuilderFactory(chunk);
                builder.BuildModels();
                chunk.HasChanged = false;

                // if we previously had a model, we should unload it
                if (oldModel != null)
                {
                    UnloadChunkModel(oldModel);
                }
            }
        }

        public void LoadChunkModel(ChunkModel chunkModel)
        {
            // Gen buffer for vertices
            var vHandle = GCHandle.Alloc(chunkModel.Vertices, GCHandleType.Pinned);
            var uvHandle = GCHandle.Alloc(chunkModel.UV, GCHandleType.Pinned);
            var lHandle = GCHandle.Alloc(chunkModel.Lighting, GCHandleType.Pinned);
            int vertexBufferId;
            GL.GenBuffers(1, out vertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, chunkModel.VerticesCount * 3 * sizeof(float), vHandle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            chunkModel.VertexBufferId = vertexBufferId;

            // Gen buffer for block texture array
            int textureBufferId;
            GL.GenBuffers(1, out textureBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, chunkModel.VerticesCount * 2 * sizeof(float), uvHandle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            chunkModel.TextureBufferId = textureBufferId;

            // Gen buffer for lighting data
            int lightBufferId;
            GL.GenBuffers(1, out lightBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, lightBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, chunkModel.VerticesCount * 3 * sizeof(float), lHandle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            chunkModel.LightDataBufferId = lightBufferId;

            chunkModel.LoadedIntoGL = true;

            vHandle.Free();
            uvHandle.Free();
            lHandle.Free();

            // TODO move to model class
            //_vec3Pool.Return(chunkModel.Vertices);
            //_vec2Pool.Return(chunkModel.UV);
            //_vec3Pool.Return(chunkModel.Lighting);
            //chunkModel.Vertices = null;
            //chunkModel.UV = null;
            //chunkModel.Lighting = null;
        }

        public void UnloadChunkModel(ChunkModel chunkModel)
        {
            GL.DeleteBuffers(3, new int[] { chunkModel.VertexBufferId, chunkModel.TextureBufferId, chunkModel.LightDataBufferId });
            chunkModel.VertexBufferId = -1;
            chunkModel.TextureBufferId = -1;
            chunkModel.LightDataBufferId = -1;
            _vec3Pool.Return(chunkModel.Vertices, true);
            _vec2Pool.Return(chunkModel.UV, true);
            _vec3Pool.Return(chunkModel.Lighting, true);
            chunkModel.Vertices = null;
            chunkModel.UV = null;
            chunkModel.Lighting = null;
        }
    }
}
