using System.Collections.Generic;
using System.Linq;
using Duckject.Core.Context;
using Duckject.Core.Injection;
using UnityEditor;

namespace Duckject.Editor
{
    [InitializeOnLoad]
    public class ExecutionOrderValidator
    {
        private static readonly Dictionary<string, int> ExecutionOrder = new Dictionary<string, int>()
        {
            {nameof(SceneContext), -10000},
            {nameof(AutoInjection), -9999}
        };

        static ExecutionOrderValidator()
        {
            MonoImporter.GetAllRuntimeMonoScripts()
                .Where(script => ExecutionOrder.ContainsKey(script.name))
                .ToList()
                .ForEach(SetOrder);
        
            void SetOrder(MonoScript script)
            {
                if (MonoImporter.GetExecutionOrder(script) == 0)
                    MonoImporter.SetExecutionOrder(script, ExecutionOrder[script.name]);
            }
        }
    }
}