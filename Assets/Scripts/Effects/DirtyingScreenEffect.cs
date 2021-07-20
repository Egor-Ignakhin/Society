using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class DirtyingScreenEffect : MonoBehaviour
{
    private Image mImage;
    private float braking = 1;
    private Color startColor = Color.black;
    public event Action FinishEvent;
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
        mImage.color += new Color(0, 0, 0, Time.deltaTime / braking);
        if (mImage.color.a > 1)
        {
            FinishEvent?.Invoke();
            FinishEvent = null;            
        }
    }

    public void SubsctibeOnFinish(Action method) => FinishEvent += method;

    internal void OnInit(int b, Color sc)
    {
        braking = b;
        startColor = sc;
        startColor.a = 0;
    }
}
