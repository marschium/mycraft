using MyCraft.Assets;
using MyCraft.Engine;
using MyCraft.Engine.Core;
using Ninject;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft
{
    class Program
    {
        [HandleProcessCorruptedStateExceptions]
        static void Main(string[] args)
        {
            try
            {
                var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = true });
                kernel.Load(new EngineModule());
                kernel.Load(new EngineCoreModule());
                kernel.Load(new AssetsModule());

                using (var g = new GameEngine(kernel)) // TODO kernel load instead
                {
                    g.VSync = VSyncMode.Off;
                    g.Run(60,0);
                }
            }
            catch (AccessViolationException e)
            {
                  Debugger.Break();
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}