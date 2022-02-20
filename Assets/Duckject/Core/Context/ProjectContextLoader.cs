using Duckject.Core.Utils;
using UnityEngine;

namespace Duckject.Core.Context
{
    public class ProjectContextLoader : MonoBehaviour
    {
        private const string PREFAB_PATH = "Project Context";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            ProjectContext projectContext = Resources.Load<ProjectContext>(PREFAB_PATH);
            var instance = Instantiate(projectContext);
            DontDestroyOnLoad(instance);
            instance.Initialize();

            QuackUtils.CreateNonLazyInstances();
        }
    }
}