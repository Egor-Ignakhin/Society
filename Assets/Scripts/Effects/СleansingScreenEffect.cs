using System;

using UnityEngine;
using UnityEngine.UI;
namespace Society.Effects
{
    internal sealed class СleansingScreenEffect : MonoBehaviour
    {
        private Image mImage;
        private double braking = 1;
        private Color startColor = Color.white;
        public event Action FinishEvent;
        public void OnInit(double b, Color sc)
        {
            braking = b;
            startColor = sc;
        }

        public void SubsctibeOnFinish(Action method) => FinishEvent += method;
        private void Start()
        {
            mImage = gameObject.AddComponent<Image>();
            var ec = FindObjectOfType<EffectsCanvas>();
            transform.SetParent(ec.transform);
            transform.localScale = ec.GetComponent<CanvasScaler>().referenceResolution / 100;
            transform.localPosition = Vector2.zero;
            mImage.color = startColor;
        }
        private void Update() => LerpColor();

        private void LerpColor()
        {
            var color = mImage.color;
            color.a = Mathf.MoveTowards(color.a, 0, (float)(Time.deltaTime / braking));
            mImage.color = color;
            if (mImage.color.a == 0)
            {
                FinishEvent?.Invoke();
                FinishEvent = null;
                Destroy(gameObject);
            }
        }
    }
}