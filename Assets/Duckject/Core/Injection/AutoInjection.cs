using System.Linq;
using Duckject.Core.Extensions;
using Duckject.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Duckject.Core.Injection
{
    public class AutoInjection : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private InjectionWay _injectionWay = InjectionWay.Scene;

        [SerializeField]
        private MonoBehaviour[] _injectionBehaviours;

        #endregion

        private void Awake()
        {
            foreach (var monoBehaviour in _injectionBehaviours)
                monoBehaviour.Quack();
        }

        #region Neted Types

        private enum InjectionWay
        {
            Scene = 0,
            GameObject = 1
        }

        #endregion

        #region Editor

#if UNITY_EDITOR

        //private void OnValidate() => UpdateInjectionBehaviours();

        [ContextMenu("Update Injection Behaviours")]
        private void UpdateInjectionBehaviours()
        {
            switch (_injectionWay)
            {
                case InjectionWay.Scene:
                    _injectionBehaviours = GetInjectionBehaviours(Resources.FindObjectsOfTypeAll<MonoBehaviour>()
                        .Where(behaviour => !EditorUtility.IsPersistent(behaviour.transform.root.gameObject) &&
                                            !(behaviour.hideFlags == HideFlags.NotEditable ||
                                              behaviour.hideFlags == HideFlags.HideAndDontSave))
                        .ToArray());
                    break;
                case InjectionWay.GameObject:
                    _injectionBehaviours = GetInjectionBehaviours(GetComponentsInChildren<MonoBehaviour>(true));
                    break;
            }
            
            EditorUtility.SetDirty(this);
        }

        private MonoBehaviour[] GetInjectionBehaviours(MonoBehaviour[] monoBehaviours)
        {
            return monoBehaviours.Select(behaviour => (behaviour, QuackUtils.IsContainInjectionMethods(behaviour)))
                .Where(item => item.Item2)
                .Select(item => item.behaviour)
                .ToArray();
        }

        private void Reset() => UpdateInjectionBehaviours();

#endif

        #endregion
    }
}