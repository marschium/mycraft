using MyCraft.Engine.Abstract;
using Ninject.Modules;
using OpenTK;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine
{
    public class EngineModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IUpdateScheduler>().To<UpdateScheduler>().InSingletonScope();
            Kernel.Bind<ArrayPool<Vector3>>().ToMethod(ctx => ArrayPool<Vector3>.Shared);
            Kernel.Bind<ArrayPool<Vector2>>().ToMethod(ctx => ArrayPool<Vector2>.Shared);
        }
    }
}
