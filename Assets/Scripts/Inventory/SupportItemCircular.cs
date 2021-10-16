using Society.Patterns;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Inventory
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
        public void SetColorByTypeOfInteractiveObject(InteractiveObject[] ios)
        {
            foreach (var io in ios)
            {
                if (io is Missions.MissionInteractiveObject)
                {
                    mCircular.color = Color.green;
                    print(1);
                    return;
                }
                else
                    mCircular.color = Color.white;
            }
        }
    }
}