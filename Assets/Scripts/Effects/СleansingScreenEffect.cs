using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class СleansingScreenEffect : MonoBehaviour
{
    private Image mImage;
    private float braking = 1;
    private Color startColor = Color.white;
    public event Action FinishEvent;
    public void OnInit(float b, Color sc)
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
        mImage.color -= new Color(0, 0, 0, Time.deltaTime / braking);
        if (mImage.color.a <= 0)
        {
            FinishEvent?.Invoke();
            FinishEvent = null;
            Destroy(gameObject);
        }
    }
}
