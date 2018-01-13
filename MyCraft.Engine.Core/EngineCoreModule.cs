using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core.GameObjects;
using MyCraft.Engine.Core.Items;
using MyCraft.Engine.Core.Items.Debug;
using MyCraft.Engine.Core.World.Biomes;
using MyCraft.Engine.Core.World.TerrainDecorators;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.World.TerrainDecorator;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core
{
    public class EngineCoreModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<Engine.World.BlockMap>().ToSelf().InSingletonScope();
            Kernel.Bind<GameObjectInitaliser>().ToSelf().InSingletonScope();

            Kernel.Bind<Player>().ToSelf().InSingletonScope();
            Kernel.Bind<Func<Type, GameObject>>().ToMethod(ctx => t => (GameObject)ctx.Kernel.Get(t));
            Kernel.Bind<Func<Type, Block>>().ToMethod(ctx => t => (Block)ctx.Kernel.Get(t));

            Kernel.Bind<Inventory>().ToMethod(ctx => 
            new Inventory(
                ctx.Kernel.Get<FireStarter>(),
                ctx.Kernel.Get<ExplosionMaker>()
                /*ctx.Kernel.Get<DestoryBlock>(),
                ctx.Kernel.Get<ExplosionMaker>(),
                ctx.Kernel.Get<PlaceBlock<ExplosiveBlock>>()*/));

            Kernel.Bind<AbstractTerrainDecorator>().To<TreeDecorator>().WhenInjectedInto<WoodlandBiome>();
            Kernel.Bind<AbstractTerrainDecorator>().To<FlowerDecorator>().WhenInjectedInto<WoodlandBiome>();
            Kernel.Bind<AbstractTerrainDecorator>().To<WatchtowerDecorator>().WhenInjectedExactlyInto<WoodlandBiome>();

            Kernel.Bind<AbstractTerrainDecorator>().To<MushroomDecorator>().WhenInjectedInto<MushroomBiome>();
            Kernel.Bind<AbstractTerrainDecorator>().To<SmallMushroomDecorator>().WhenInjectedInto<MushroomBiome>();

            Kernel.Bind<AbstractTerrainDecorator>().To<DeadPlantDecorator>().WhenInjectedInto<SandBiome>();
            Kernel.Bind<AbstractTerrainDecorator>().To<CactusDecorator>().WhenInjectedInto<SandBiome>();
        }
    }
}
