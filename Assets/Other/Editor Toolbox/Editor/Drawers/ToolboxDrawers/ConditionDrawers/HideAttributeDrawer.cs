﻿using UnityEditor;

using UnityEngine;

namespace Toolbox.Editor.Drawers
{
    public class HideAttributeDrawer : ToolboxConditionDrawer<HideAttribute>
    {
        protected override PropertyCondition OnGuiValidateSafe(SerializedProperty property, HideAttribute attribute)
        {
            return PropertyCondition.NonValid;
        }
    }
}