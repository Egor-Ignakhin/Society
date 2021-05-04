using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class SlotForAddingItems : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image backgroundImage;
    private Color startColorBackground;
    private Color startColorTextType;
    private Color startColorTextCount;
    private Vector3 defaultPosition;

    public string textOfType { get; private set; }
    public int textOfCount { get; private set; }
    public void Awake()
    {
        defaultPosition = transform.localPosition;
        startColorTextType = typeText.color;
        startColorTextCount = countText.color;
        startColorBackground = backgroundImage.color;


        gameObject.SetActive(false);
    }
    public void SetText(string type, int count)
    {
        typeText.SetText(textOfType = type);
        countText.SetText("+" + (textOfCount = count));
        gameObject.SetActive(true);
    }

    internal void Translate(int i, float shift)
    {
        transform.localPosition = defaultPosition + new Vector3(0, shift * i, 0);
    }
    public void SetDefaultPosition()
    {
        transform.localPosition = defaultPosition;
    }
    public float GetAlphaColor()
    {
        return backgroundImage.color.a;
    }

    public void ReturnDefaultColor()
    {
        typeText.color = startColorTextType;
        countText.color = startColorTextCount;
        backgroundImage.color = startColorBackground;
    }
    public void DifferenceColor(Color c)
    {
        backgroundImage.color -= c;
        typeText.color -= c;
        countText.color -= c;
    }
    public bool DisableSlot()
    {
        bool retV = GetAlphaColor() <= 0;

        gameObject.SetActive(!retV);

        return retV;
    }
}
