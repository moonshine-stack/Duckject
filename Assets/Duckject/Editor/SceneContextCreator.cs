using Duckject.Core.Context;
using Duckject.Core.Injection;
using UnityEditor;
using UnityEngine;

namespace Duckject.Editor
{
    public class SceneContextCreator
    {
        [MenuItem("Duckject/Create Scene Context")]
        private static void Init()
        {
            GameObject gameObject = new GameObject("[SCENE CONTEXT]");
            gameObject.AddComponent<SceneContext>();
            gameObject.AddComponent<AutoInjection>();
        }
    }
}