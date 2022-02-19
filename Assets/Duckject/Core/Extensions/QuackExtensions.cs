using Duckject.Core.Utils;
using UnityEngine;

namespace Duckject.Core.Extensions
{
    public static class QuackExtensions
    {
        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public static void Quack(this MonoBehaviour component)
        {
            QuackUtils.Inject(component);
        }

        #endregion
    }
}
