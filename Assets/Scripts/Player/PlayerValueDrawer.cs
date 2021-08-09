using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses
{
    sealed class PlayerValueDrawer : MonoBehaviour
    {
        private BasicNeeds playerBn;
        [SerializeField] private State mState;
        [SerializeField] private bool disableOnZero;
        [SerializeField] private bool isSprite;
        [SerializeField] private bool isLine;
        [ShowIf(nameof(isLine), true)] [SerializeField] private RectTransform separator;
        [ShowIf(nameof(isLine), true)] [SerializeField] private bool isAdditionalLine;
        [ShowIf(nameof(isAdditionalLine), true)] [SerializeField] private State additionalState = State.health;

        private enum State { health, thirst, food, radiation };

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
            if (mState == State.health)
            {
                playerBn.HealthChangeValue += OnChangePlayerHealth;
            }
            else if (mState == State.thirst)
                playerBn.ThirstChangeValue += OnChangePlayerThirst;
            else if (mState == State.food)
                playerBn.FoodChangeValue += OnChangePlayerFood;
            else if (mState == State.radiation)
                playerBn.RadiationChangeValue += OnChangePlayerRadiation;
        }

        private void OnDisable()
        {
            if (mState == State.health)
            {
                playerBn.HealthChangeValue -= OnChangePlayerHealth;
            }
            else if (mState == State.thirst)
                playerBn.ThirstChangeValue -= OnChangePlayerThirst;
            else if (mState == State.food)
                playerBn.FoodChangeValue -= OnChangePlayerFood;
            else if (mState == State.radiation)
                playerBn.RadiationChangeValue -= OnChangePlayerRadiation;
        }

        private void OnChangePlayerRadiation(float value)
        {
            if (isSprite)
                mImage.enabled = value > 1;
            else
            {
                mText.enabled = value > 1;
                mText.SetText(Mathf.Round(value).ToString());
            }
        }
        private void OnChangePlayerThirst(float v)
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
        private void OnChangePlayerHealth(float _)
        {
            void SetSeparatorState()
            {
                var nextPos = separator.anchoredPosition;
                nextPos.x = mrt.sizeDelta.x * mImage.fillAmount;
                separator.anchoredPosition = nextPos;
            }
            switch (additionalState)
            {
                case State.health:
                    mImage.fillAmount = playerBn.Health / playerBn.MaximumHealth;
                    SetSeparatorState();
                    break;
                case State.food:
                    mImage.fillAmount = playerBn.HealthForFood / BasicNeeds.MaxHealthForFood;
                    
                    playerBn.HealthForFood++;
                    SetSeparatorState();
                    break;
                default:
                    break;
            }

        }
    }
}