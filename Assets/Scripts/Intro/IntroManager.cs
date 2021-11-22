using System.Collections.Generic;

using Society.GameScreens;

using UnityEngine;

namespace Society.Intro
{
    internal sealed class IntroManager : MonoBehaviour// класс отвечающий за интро-сцену
    {
        private const int listsCount = 3;// количество слайдов
        [SerializeField] private List<GameObject> lists = new List<GameObject>(listsCount);// слайды
        [SerializeField] private List<float> listsLength = new List<float>(listsCount);// уникальная длина воспроизведения для каждого слайда
        private int currentList = 0;// текущий слайд
        private float timeToChange = 0;// время до смены слайда

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)Scenes.Bunker);
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
            //dev only
            if (Input.anyKeyDown)
            {
                StartGame();
            }
            //dev only

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
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)Scenes.Bunker);
        }
    }
}