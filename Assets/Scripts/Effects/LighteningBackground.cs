using UnityEngine;

/// <summary>
/// класс имеет фото, которое постепенно светлеет
/// </summary>
public sealed class LighteningBackground : MonoBehaviour, IDelayable
{
    private UnityEngine.UI.Image mImage;
    private Color targetColor = new Color(0, 0, 0, 0);
    private float speed;

    public event EventHandlers.EventHandler FinishPart;

    public void Init(UnityEngine.UI.Image img, float neededSpeed = 1)
    {
        mImage = img;
        speed = neededSpeed;
    }
    private void Update()
    {
        LerpColor();
    }
    private void LerpColor()
    {
        mImage.color = Color.Lerp(mImage.color, targetColor, Time.deltaTime * speed);
        if (mImage.color.a <= 0)
        {
            FinishPart?.Invoke();
            enabled = false;
        }
    }
}
