using Duckject.Core.Container;
using Duckject.Core.Installer;

namespace Duckject.Examples._10_Test.Scripts
{
    public class SceneInstaller : InstallerBase
    {
        public override void Install(IContainer container)
        {
            container.Bind(10)
                .Bind<ClassWithoutConstructor>().AsCached().SetNonLazy()
                .Bind<StructWithQuackConstructor>().AsCached().SetNonLazy();
        }
    }
}