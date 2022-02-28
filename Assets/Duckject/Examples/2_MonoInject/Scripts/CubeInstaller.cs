using Duckject.Core.Container;
using Duckject.Core.Installer;
using UnityEngine;

namespace Duckject.Examples._2_MonoInject.Scripts
{
    public class CubeInstaller : InstallerBase
    {
        public override void Install(IContainer container)
        {
            container.Bind(GameObject.CreatePrimitive(PrimitiveType.Cube).transform)
                .Bind(15f).To<Rotator>(); 
        }
    }
}
