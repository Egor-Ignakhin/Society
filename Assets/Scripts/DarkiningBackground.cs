using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DarkiningBackground : MonoBehaviour
{
    private UnityEngine.UI.Image mImage;
    private Color targetColor =Color.black;
    private float speed;
    public void Init(UnityEngine.UI.Image img, float neededSpeed = 1)
    {
        mImage = img;
        speed = neededSpeed;
        mImage.color = new Color(0, 0, 0, 0);
    }
    private void Update()
    {
        mImage.color = Color.Lerp(mImage.color, targetColor, Time.deltaTime * speed);
        if (System.Math.Round(mImage.color.a, 4) == 0)
        {
            enabled = false;
        }
    }
}
