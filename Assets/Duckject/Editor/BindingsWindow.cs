using System.Collections.Generic;
using System.Linq;
using Duckject.Core.Container;
using Duckject.Core.Context;
using UnityEditor;
using UnityEngine;

namespace Duckject.Editor
{
    public class BindingsWindow : EditorWindow
    {
        private Vector2 _scrollPosition = Vector2.zero;
        
        [MenuItem("Duckject/Bindings Window")]
        private static void Init()
        {
            BindingsWindow window = (BindingsWindow) GetWindow(typeof(BindingsWindow), false, "Bindings Window");
            window.Show();
        }

        private void OnGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                GUILayout.Label("Show binding in runtime");
                return;
            }

            float sectionWidth = position.width / (3 * 1.05f);
            GUILayoutOption[] options = {GUILayout.Width(sectionWidth)};

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width),
                GUILayout.Height(position.height));

            GUILayout.BeginHorizontal();

            GUILayout.Label("Service", EditorStyles.wordWrappedLabel, options);
            GUILayout.Label("Identifier", EditorStyles.wordWrappedLabel, options);
            GUILayout.Label("Targets", EditorStyles.wordWrappedLabel, options);

            GUILayout.EndHorizontal();

            List<Binding> bindings = ContextBase.Containers.SelectMany(container => container.Bindings).ToList();

            for (int i = 0; i < bindings.Count; i++)
            {
                GUILayout.BeginHorizontal("box");

                GUILayout.Label(bindings[i].Service.ToString(), EditorStyles.wordWrappedLabel, options);

                GUILayout.Label(bindings[i]?.ToString() ?? "-", EditorStyles.wordWrappedLabel, options);

                if (bindings[i].Targets.Any())
                {
                    GUILayout.Label(bindings[i].Targets.Select(target => target.ToString())
                            .Aggregate((current, next) => current + "\n\n" + next).ToString(),
                        EditorStyles.wordWrappedLabel,
                        options);
                }
                else
                {
                    GUILayout.Label("-", EditorStyles.wordWrappedLabel, options);
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}