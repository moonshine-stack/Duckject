using Duckject.Core.Container;
using Duckject.Core.Installer;

namespace Duckject.Examples._10_Test.Scripts
{
    public class SceneInstaller : InstallerBase
    {
        public override void Install(IContainer container)
        {
            container
                .Bind<string>("adawd")
                .Bind<int>(9876543).To<int>()
                .Bind<int>(1).To<StructWithQuackConstructor>()
                .Bind<StructWithQuackConstructor>().AsCached().SetNonLazy()
                .Bind<ClassWithoutConstructor>().AsCached().SetNonLazy()
                .Bind<MonoBehaviourClass>();
        }
    }
}