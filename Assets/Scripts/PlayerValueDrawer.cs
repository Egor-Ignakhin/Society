using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerValueDrawer : MonoBehaviour
{
    [SerializeField] private BasicNeeds valueClass;

    [SerializeField] private bool disableOnZero;
    [SerializeField] private bool isSprite;
    [SerializeField] private bool isLine;

    private enum state {health, thirst, food, radiation };
    [SerializeField] private state mState;

    private TextMeshProUGUI mText;
    private Image mImage;
    private RectTransform mtRtr;

    private void Awake()
    {
        if (isSprite || isLine)
        {
            mImage = GetComponent<Image>();
            mtRtr = GetComponent<RectTransform>();
        }
        else
            mText = GetComponent<TextMeshProUGUI>();

        if(valueClass == null)
        {
            valueClass = FindObjectOfType<BasicNeeds>();
        }
    }
    private void OnEnable()
    {
        if (mState == state.health)
            valueClass.HealthChangeValue += OnValueChange;
        else if (mState == state.thirst)
            valueClass.ThirstChangeValue += OnValueChange;
        else if (mState == state.food)
            valueClass.FoodChangeValue += OnValueChange;
        else if (mState == state.radiation)
            valueClass.RadiationChangeValue += OnValueChange;
    }

    private void OnValueChange(float value)
    {
        SetActivity(value);
    }
    private void OnDisable()
    {
        if (mState == state.health)
            valueClass.HealthChangeValue -= OnValueChange;
       else if (mState == state.thirst)
            valueClass.ThirstChangeValue -= OnValueChange;
        else if (mState == state.food)
            valueClass.FoodChangeValue -= OnValueChange;
        else if (mState == state.radiation)
            valueClass.RadiationChangeValue -= OnValueChange;
    }



    private void SetActivity(float value)
    {
        if (mState == state.health || mState == state.thirst || mState == state.food)
        {
            if(isLine)
            {
                float max = 0;
                switch (mState)
                {
                    case state.health:
                        max = valueClass.MaximumHealth;                      
                        break;
                    case state.thirst:
                        max = valueClass.MaximumThirst;
                        break;
                    case state.food:
                        max = valueClass.MaximumFood;
                        break;
                }
                max = 100 / max;
                transform.localPosition = new Vector3(max * value * 0.5f - 50, 0);
                mtRtr.sizeDelta = new Vector2(value * max, 100);
            }   
            else
                mText.SetText(value.ToString());
        }

        if (mState == state.radiation)
        {
            if (isSprite)
                mImage.enabled = value > valueClass.MinRadiation && value >= 1;
            else
            {
                mText.enabled = value > valueClass.MinRadiation && value >= 1;
                mText.SetText(Mathf.Round(value).ToString());
            }
        }
    }  
}