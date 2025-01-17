﻿using UnityEditor;

using UnityEngine;

namespace Toolbox.Editor.Drawers
{
    public abstract class ToolboxPropertyDrawer<T> : ToolboxPropertyDrawerBase where T : ToolboxPropertyAttribute
    {
        protected virtual void OnGuiSafe(SerializedProperty property, GUIContent label, T attribute)
        {
            ToolboxEditorGui.DrawDefaultProperty(property);
        }


        public override bool IsPropertyValid(SerializedProperty property)
        {
            return true;
        }

        public sealed override void OnGui(SerializedProperty property, GUIContent label)
        {
            OnGui(property, label, PropertyUtility.GetAttribute<T>(property));
        }

        public sealed override void OnGui(SerializedProperty property, GUIContent label, ToolboxAttribute attribute)
        {
            OnGui(property, label, attribute as T);
        }


        public void OnGui(SerializedProperty property, GUIContent label, T attribute)
        {
            if (attribute == null)
            {
                return;
            }

            if (IsPropertyValid(property))
            {
                OnGuiSafe(property, label, attribute);
            }
            else
            {
                var warningContent = new GUIContent(property.displayName + " has invalid property drawer");
                //create additional warning log to the console window
                ToolboxEditorLog.WrongAttributeUsageWarning(attribute, property);
                //create additional warning label based on the property name
                ToolboxEditorGui.DrawEmptyProperty(property, warningContent);
            }
        }
    }
}