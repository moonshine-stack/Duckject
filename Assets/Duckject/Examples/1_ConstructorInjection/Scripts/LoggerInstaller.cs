using Duckject.Core.Attributes;
using Duckject.Core.Container;
using Duckject.Core.Installer;
using UnityEngine;

namespace Duckject.Examples._1_ConstructorInjection.Scripts
{
    public class LoggerInstaller : InstallerBase
    {
        public override void Install(IContainer container)
        {
            container.Bind("Test Message")
                .Bind<Logger>().AsCached().SetNonLazy();
        }

        private class Logger
        {
            [Quack]
            public Logger(string message) => Debug.Log(message);
        }
    }
}
