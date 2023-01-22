using System;
using System.Linq;
using Obert.Common.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Obert.Common.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(SceneRefAttribute))]
    public class SceneRefAttributeDrawer : PropertyDrawer
    {
        //UnityEditorInternal.InternalEditorUtility.tags


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                Debug.LogError($"{nameof(SceneRefAttribute)} is only supported on fields of type {nameof(String)}!");
                return;
            }
            
            var scenePath = property.stringValue;
            SceneAsset currentValue = null;
            if (!string.IsNullOrWhiteSpace(scenePath))
            {
                currentValue = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                if (!currentValue)
                {
                    Debug.LogWarning($"Unable to find {nameof(SceneAsset)} at path: \"{scenePath}\"");
                }
            }

            var scene = EditorGUI.ObjectField(position, label, currentValue, typeof(SceneAsset), false) as SceneAsset;
            if (scene != currentValue)
            {
                if (scene)
                {
                    var sceneNameWithExtension = $"{scene.name}.unity";
                    var sceneBuild = EditorBuildSettings.scenes.FirstOrDefault(x =>
                        x.path.EndsWith(sceneNameWithExtension, StringComparison.InvariantCultureIgnoreCase));
                    if (sceneBuild == null)
                    {
                        Debug.LogError($"Unable to find scene {sceneNameWithExtension} in EditorBuildSettings.scenes");
                        return;
                    }
                    
                    property.stringValue = sceneBuild.path;
                }
                else
                {
                    property.stringValue = string.Empty;
                }
                
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}