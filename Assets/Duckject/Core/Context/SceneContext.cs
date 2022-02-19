using Duckject.Core.Utils;

namespace Duckject.Core.Context
{
    public sealed class SceneContext : ContextBase
    {
        #region Event Functions

        private void Awake()
        {
            Initialize();
            
            QuackUtils.CreateNonLazyInstances();
        }

        private void OnDestroy() => DestroyContainer();

        #endregion
    }
}