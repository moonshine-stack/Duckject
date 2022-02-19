using Duckject.Core.Container;
using UnityEngine;

namespace Duckject.Core.Installer
{
    public abstract class InstallerBase : MonoBehaviour
    {
        public abstract void Install(IContainer container);
    }
}