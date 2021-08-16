using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public sealed class SupportItemCircular : MonoBehaviour
    {
        private Image mCircular;
        private void Start()
        {
            mCircular = GetComponent<Image>();
        }
        public void SetSpriteOpacity(float v)
        {
            var tempColor = mCircular.color;
            tempColor.a = v;
            mCircular.color = tempColor;
        }
    }
}