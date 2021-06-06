using PlayerClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class PlayerValueDrawer : MonoBehaviour
{
    private BasicNeeds playerBn;

    [SerializeField] private bool disableOnZero;
    [SerializeField] private bool isSprite;
    [SerializeField] private bool isLine;

    private enum state { health, thirst, food, radiation };
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
           
            playerBn = FindObjectOfType<BasicNeeds>();        
    }
    private void OnEnable()
    {
        if (mState == state.health)
            playerBn.HealthChangeValue += OnChangePlayerV;
        else if (mState == state.thirst)
            playerBn.ThirstChangeValue += OnChangePlayerV;
        else if (mState == state.food)
            playerBn.FoodChangeValue += OnChangePlayerV;
        else if (mState == state.radiation)
            playerBn.RadiationChangeValue += OnChangePlayerV;
    }    

    private void OnDisable()
    {
        if (mState == state.health)
            playerBn.HealthChangeValue -= OnChangePlayerV;
        else if (mState == state.thirst)
            playerBn.ThirstChangeValue -= OnChangePlayerV;
        else if (mState == state.food)
            playerBn.FoodChangeValue -= OnChangePlayerV;
        else if (mState == state.radiation)
            playerBn.RadiationChangeValue -= OnChangePlayerV;
    }



    private void OnChangePlayerV(float value)
    {
        if (mState == state.health || mState == state.thirst || mState == state.food)
        {
            if (isLine)
            {
                float max = 0;
                switch (mState)
                {
                    case state.health:
                        max = playerBn.MaximumHealth;
                        break;
                    case state.thirst:
                        max = playerBn.MaximumThirst;
                        break;
                    case state.food:
                        max = playerBn.MaximumFood;
                        break;
                }
                max = 100 / max;
                transform.localPosition = new Vector3(max * value - 50, 0);
                mtRtr.sizeDelta = new Vector2(value * max, 100);
            }
            else
                mText.SetText(value.ToString());
        }

        if (mState == state.radiation)
        {
            if (isSprite)
                mImage.enabled = value > 1;
            else
            {
                mText.enabled = value > 1;
                mText.SetText(Mathf.Round(value).ToString());
            }
        }
    }
}