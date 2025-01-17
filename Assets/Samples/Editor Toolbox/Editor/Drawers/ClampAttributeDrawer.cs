﻿using UnityEditor;

using UnityEngine;

namespace Toolbox.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ClampAttribute))]
    public class ClampAttributeDrawer : ToolboxNativePropertyDrawer
    {
        protected override float GetPropertyHeightSafe(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeightSafe(property, label);
        }

        protected override void OnGUISafe(Rect position, SerializedProperty property, GUIContent label)
        {
            var minValue = Attribute.MinValue;
            var maxValue = Attribute.MaxValue;

            if (property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Clamp(property.floatValue, minValue, maxValue);
            }
            else
            {
                property.intValue = Mathf.Clamp(property.intValue, (int)minValue, (int)maxValue);
            }

            EditorGUI.PropertyField(position, property, label, property.isExpanded);
        }


        public override bool IsPropertyValid(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Float ||
                   property.propertyType == SerializedPropertyType.Integer;
        }


        private ClampAttribute Attribute => attribute as ClampAttribute;
    }
}