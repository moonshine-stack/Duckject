using System.Collections.Generic;
using Duckject.Core.Container;
using Duckject.Core.Installer;
using UnityEngine;

namespace Duckject.Core.Context
{
    public abstract class ContextBase : MonoBehaviour
    {
        #region Fields

        [SerializeField, Tooltip("")]
        private List<InstallerBase> _installers = new List<InstallerBase>();

        private static readonly Dictionary<ContextBase, DiContainer> _containers = new Dictionary<ContextBase, DiContainer>();

        #endregion

        #region Properties

        public static IEnumerable<DiContainer> Containers => _containers.Values;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            DiContainer container = new DiContainer();
            _containers.Add(this, container);
            _installers.ForEach(installer => installer.Install(container));
        }

        #endregion

        #region Protected Methods

        protected void DestroyContainer() => _containers.Remove(this);

        #endregion
    }
}
