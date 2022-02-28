using Duckject.Core.Container;
using Duckject.Core.Installer;
using UnityEngine;

namespace Duckject.Examples._0_HelloWorld.Scripts
{
    public class HelloWorldInstaller : InstallerBase
    {
        public override void Install(IContainer container)
        {
            container.Bind<HelloWorld>().AsCached().SetNonLazy();
        }

        private class HelloWorld
        {
            public HelloWorld() => Debug.Log("Hello World!");
        }
    }
}
