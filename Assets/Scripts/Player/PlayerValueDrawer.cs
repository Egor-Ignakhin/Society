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
    [ShowIf(nameof(isLine), true)] [SerializeField] private RectTransform separator;
    [ShowIf(nameof(isLine), true)] [SerializeField] private Image healthpart1, healthpart2;

    private enum state { health, thirst, food, radiation };
    [SerializeField] private state mState;

    private TextMeshProUGUI mText;
    private Image mImage;
    private RectTransform mrt;

    private void Awake()
    {
        if (isSprite || isLine)
        {
            mImage = GetComponent<Image>();
        }
        else
            mText = GetComponent<TextMeshProUGUI>();

        playerBn = FindObjectOfType<BasicNeeds>();
        mrt = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        if (mState == state.health)
            playerBn.HealthChangeValue += OnChangePlayerHealth;
        else if (mState == state.thirst)
            playerBn.ThirstChangeValue += OnChangeThirst;
        else if (mState == state.food)
            playerBn.FoodChangeValue += OnChangePlayerFood;
        else if (mState == state.radiation)
            playerBn.RadiationChangeValue += OnChangePlayerV;
    }

    private void OnDisable()
    {
        if (mState == state.health)
            playerBn.HealthChangeValue -= OnChangePlayerHealth;
        else if (mState == state.thirst)
            playerBn.ThirstChangeValue -= OnChangeThirst;
        else if (mState == state.food)
            playerBn.FoodChangeValue -= OnChangePlayerFood;
        else if (mState == state.radiation)
            playerBn.RadiationChangeValue -= OnChangePlayerV;
    }



    private void OnChangePlayerV(float value)
    {
        if (mState == state.health || mState == state.thirst || mState == state.food)
        {
            if (isLine)
            {


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
    private void OnChangeThirst(float v)
    {        
        var nextPos = separator.anchoredPosition;
        nextPos.x = v * mrt.sizeDelta.x / playerBn.MaximumThirst;
        separator.anchoredPosition = nextPos;
        mImage.fillAmount = v / playerBn.MaximumThirst;
    }
    private void OnChangePlayerFood(float v)
    {
        var nextPos = separator.anchoredPosition;
        nextPos.x = v * mrt.sizeDelta.x / playerBn.MaximumFood;
        separator.anchoredPosition = nextPos;
        mImage.fillAmount = v / playerBn.MaximumFood;

    }
    private void OnChangePlayerHealth(float v)
    {
        var nextPos = separator.anchoredPosition;
        nextPos.x = v * mrt.sizeDelta.x / playerBn.MaximumHealth;
        separator.anchoredPosition = nextPos;
        mImage.fillAmount = v / playerBn.MaximumHealth;

    }
}