using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Assets
{
    public class AssetsModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<TextureLoader>().ToSelf().InSingletonScope();
        }
    }
}
