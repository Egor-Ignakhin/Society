using UnityEngine;

/// <summary>
/// класс имеет фото, которое постепенно светлеет
/// </summary>
public sealed class LighteningBackground : MonoBehaviour
{
    private UnityEngine.UI.Image mImage;
    private Color targetColor = new Color(0, 0, 0, 0);
    private float speed;
    public void Init(UnityEngine.UI.Image img, float neededSpeed = 1)
    {
        mImage = img;
        speed = neededSpeed;
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
