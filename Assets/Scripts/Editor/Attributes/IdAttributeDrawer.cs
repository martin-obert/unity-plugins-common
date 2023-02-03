using System;
using Obert.Common.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Obert.Common.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(IdAttribute))]
    public class IdAttributeDrawer : PropertyDrawer
    {
        private readonly Texture copy;
        private readonly Texture refresh;
        private readonly GUIStyle style;

        public IdAttributeDrawer()
        {
            style = new GUIStyle(EditorStyles.miniButton)
            {
                padding = new RectOffset(2, 2, 2, 2),
                margin = new RectOffset(0, 0, 0, 0)
            };
            copy = Resources.Load<Texture>("content_copy_FILL0_wght400_GRAD0_opsz48");
            refresh = Resources.Load<Texture>("autorenew_FILL0_wght400_GRAD0_opsz48");
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var oneSpace = position.height;
            var initialValue = property.stringValue;

            var oneSize = new Vector2(oneSpace, oneSpace);
            var rectPosition = position.position;
            var positionWidth = position.width - oneSpace * 2;

            var value = EditorGUI.TextField(new Rect(rectPosition, new Vector2(positionWidth, position.height)), label,
                initialValue);


            if (GUI.Button(new Rect(rectPosition + Vector2.right * positionWidth, oneSize), refresh, style))
            {
                value = Guid.NewGuid().ToString("D");
            }

            if (GUI.Button(new Rect(rectPosition + Vector2.right * (positionWidth + oneSpace), oneSize), copy, style))
            {
                GUIUtility.systemCopyBuffer = value;
                Debug.Log($"Id copied to clipboard: {value}");
                return;
            }

            if (string.Equals(value, initialValue)) return;
            Undo.RecordObject(property.serializedObject.targetObject, $"Set {property.serializedObject.targetObject}");
            property.stringValue = value;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}