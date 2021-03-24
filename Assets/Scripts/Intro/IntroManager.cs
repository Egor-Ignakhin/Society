using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intro
{
    public sealed class IntroManager : MonoBehaviour
    {
        private const int listsCount = 3;
        [SerializeField] private List<GameObject> lists = new List<GameObject>(listsCount);
        [SerializeField] private List<float> listsLength = new List<float>(listsCount);
        private int currentList = 0;
        private float timeToChange = 0;

        private void OnEnable()
        {
            EnableNextList();
        }
        private void DisableOldList()
        {
            lists[currentList].SetActive(false);
        }
        private void EnableNextList()
        {            
            lists[currentList].SetActive(true);
            timeToChange = listsLength[currentList];
        }
        private void ChangeCurrentList()
        {
            if (currentList + 1 >= listsCount)
            {
                StartGame();
            }
            else
            {
                currentList++;
            }
        }
        private void Update()
        {
            if (timeToChange <= 0)
            {
                DisableOldList();
                ChangeCurrentList();
                EnableNextList();
            }
            else
                timeToChange -= Time.deltaTime;
        }
        private void StartGame()
        {
            FindObjectOfType<ScenesManager>().LoadNextScene();
        }
    }
}