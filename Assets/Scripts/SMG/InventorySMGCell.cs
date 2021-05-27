using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SMG.ModifierCharacteristics;

namespace SMG {
    public class InventorySMGCell : MonoBehaviour
    {
        private Image mImage;
        public void OnInit()
        {
            mImage = GetComponent<Image>();
        }
        internal void Clear()
        {
            mImage.sprite = null;
        }
        public void RewriteSprite(SMGTitleTypeIndex modState)
        {
            mImage.sprite = GetSprite(modState);
        }
    }
}