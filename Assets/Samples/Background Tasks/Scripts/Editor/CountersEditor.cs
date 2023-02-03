using Obert.Common.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Samples.Background_Tasks.Scripts.Editor
{
    [CustomEditor(typeof(Counters))]
    public class CountersEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Check unique"))
            {
                var isValid = ((Counters)target).ids.HasUniqueIds(out var validationMessages);
                if (isValid)
                {
                    Debug.Log("Is valid");
                }
                else
                {
                    Debug.LogError(string.Join(",\n", validationMessages));
                }
            }
        }
    }
}