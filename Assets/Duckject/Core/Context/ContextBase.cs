using System.Collections.Generic;
using System.Linq;
using Duckject.Core.Container;
using Duckject.Core.Installer;
using UnityEngine;

namespace Duckject.Core.Context
{
    public abstract class ContextBase : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<InstallerBase> _installers = new List<InstallerBase>();

        private static readonly Dictionary<ContextBase, DiContainer> DiContainers =
            new Dictionary<ContextBase, DiContainer>();

        #endregion

        #region Properties

        public static IEnumerable<DiContainer> Containers => DiContainers.Values;

        #endregion

        #region Public Methods

        public static Transform GetTransformFor(Binding binding) =>
            DiContainers.First(pair => pair.Value.Bindings.Contains(binding)).Key.transform;

        public void Initialize()
        {
            DiContainer container = new DiContainer();
            DiContainers.Add(this, container);
            _installers.ForEach(installer => installer.Install(container));
        }

        #endregion

        #region Protected Methods

        protected void DestroyContainer() => DiContainers.Remove(this);

        #endregion
    }
}