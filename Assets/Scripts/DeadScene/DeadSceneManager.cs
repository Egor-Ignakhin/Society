using UnityEngine;

namespace Society.DeadScene
{
    sealed class DeadSceneManager : MonoBehaviour// класс отвечающий за сцена смерти
    {
        [SerializeField] private float delayToStartDissolving = 5;// длина ожидания до запуска анимации
        [SerializeField] private float delayToFullTransparency = 2;// длина анимации

        private float currentDSD;// время ост. до запуска анимации
        private float currentDFT;// время ост. до конца анимации
        [SerializeField] private GameObject firstSlide;//первый слайд
        [SerializeField] private TMPro.TextMeshProUGUI firstText;// текст первого слайда

        [SerializeField] private GameObject secondSlide;// второй слайд
        [SerializeField] private TMPro.TextMeshProUGUI secondText;// текст второго слайда

        [SerializeField] private float speedOffset = 0.5f;// скорость анимации
        [SerializeField] [Range(0, 1)] private float minOffset = 0.1f;//минимальная прозрачность
        [SerializeField] [Range(0, 1)] private float maxOffset = 0.9f;// максимальная прозрачность
        private float currentOffset;// текущая прозрачность
        private bool slideWasChanged = true;// слайд уже сменился

        private TMPro.TextMeshProUGUI currentSlide;// текущий слайд

        private void Awake()
        {
            #region присвоенеи дефолтных значений
            currentDSD = delayToStartDissolving;
            currentDFT = delayToFullTransparency;
            currentSlide = firstText;
            #endregion
        }
        private void Update()
        {
            CheckState();
            if (!slideWasChanged && Input.anyKeyDown)
            {
                LoadLastSave();
            }
        }
        /// <summary>
        /// просмотр текущего состояния анимаций
        /// </summary>
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
        /// <summary>
        /// растворение первого слайда
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private Color DissolveFirstSlide(Color c)
        {
            c.a = currentDFT / delayToFullTransparency;
            return c;
        }/// <summary>
         /// растворение второго слайда
         /// </summary>
         /// <param name="c"></param>
         /// <returns></returns>
        private Color DissolveSecondSlide(Color c)
        {
            c.a = currentOffset;
            if (currentOffset >= maxOffset)
            {
                slideWasChanged = false;
            }
            else if (currentOffset <= minOffset)
            {
                slideWasChanged = true;
            }
            currentOffset += Time.deltaTime * (!slideWasChanged ? -1 : 1) * speedOffset;
            return c;
        }
        /// <summary>
        /// загрузка следующего слайда
        /// </summary>
        private void LoadNextSlide()
        {
            currentSlide = secondText;
            secondSlide.SetActive(true);
            firstSlide.SetActive(false);
        }
        /// <summary>
        /// загрузка сохранения
        /// </summary>
        private void LoadLastSave()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
    }
}