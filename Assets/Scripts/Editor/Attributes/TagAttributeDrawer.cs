using System;
using System.Linq;
using Obert.Common.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Obert.Common.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                Debug.LogError($"{nameof(SceneRefAttribute)} is only supported on fields of type {nameof(String)}!");
                return;
            }

            var value = property.stringValue;
            var selected = EditorGUI.TagField(position, label, value);
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
                {
                    property.stringValue = selected;

                    if (changeCheckScope.changed)
                        property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}