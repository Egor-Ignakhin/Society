using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeadSceneScripts
{
    sealed class DeadSceneManager : MonoBehaviour
    {
        [SerializeField] private float delayToStartDissolving = 5;
        [SerializeField] private float delayToFullTransparency = 5;

        private float currentDSD;
        private float currentDFT;
        [SerializeField] private GameObject firstSlide;
        [SerializeField] private TMPro.TextMeshProUGUI firstText;

        [SerializeField] private GameObject secondSlide;
        [SerializeField] private TMPro.TextMeshProUGUI secondText;

        [SerializeField] private float speedOffset;
        [SerializeField] [Range(0, 1)] private float minOffset = 0.1f;
        [SerializeField] [Range(0, 1)] private float maxOffset = 0.9f;
        private float currentOffset;
        private bool nextOffset = true;

        private TMPro.TextMeshProUGUI currentSlide;

        private void Awake()
        {
            currentDSD = delayToStartDissolving;
            currentDFT = delayToFullTransparency;
            currentSlide = firstText;
        }
        private void Update()
        {
            CheckState();
        }
        private void CheckState()
        {
            if (currentSlide == secondText)
            {
                secondText.color = DissolveSecondSlide(secondText.color);
                return;
            }
            if (currentDSD > 0)
            {
                currentDSD -= Time.deltaTime;
                return;
            }
            else if (currentDFT > 0)
            {
                currentDFT -= Time.deltaTime;
                currentSlide.color = DissolveFirstSlide(currentSlide.color);
                return;
            }
            else
            {
                LoadNextSlide();
            }
        }
        private Color DissolveFirstSlide(Color c)
        {
            c.a = currentDFT / delayToFullTransparency;
            return c;
        }
        private Color DissolveSecondSlide(Color c)
        {
            c.a = currentOffset;
            if (currentOffset >= maxOffset)
            {
                nextOffset = false;
            }
            else if (currentOffset <= minOffset)
            {
                nextOffset = true;
            }
            currentOffset += Time.deltaTime * (nextOffset == false ? -1 : 1) * speedOffset;
            return c;
        }
        private void LoadNextSlide()
        {
            currentSlide = secondText;
            secondSlide.SetActive(true);
            firstSlide.SetActive(false);
        }
    }
}