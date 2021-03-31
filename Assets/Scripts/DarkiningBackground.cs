using System;
using UnityEngine;
/// <summary>
/// класс имеет фото, которое постепенно темнеет
/// </summary>
public sealed class DarkiningBackground : MonoBehaviour, IDelayable
{
    private UnityEngine.UI.Image mImage;
    private Color targetColor = Color.black;
    private float speed;

    public event EventHandlers.EventHandler FinishPart;

    public void Init(UnityEngine.UI.Image img, float neededSpeed = 1)
    {
        mImage = img;
        speed = neededSpeed;
        targetColor.a = 0;
        mImage.color = targetColor;
    }
    private void Update()
    {
        LerpColor();
    }
    private void LerpColor()
    {
        mImage.color += new Color(0, 0, 0, Time.deltaTime * speed);
        if (mImage.color.a >= 1)
        {            
            FinishPart?.Invoke();            
            enabled = false;
        }
    }
}
