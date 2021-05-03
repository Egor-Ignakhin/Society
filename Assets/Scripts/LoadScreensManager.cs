using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreensManager : Singleton<LoadScreensManager>
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject pressKeyText;
    [SerializeField] private Image img;
    [SerializeField] private TMPro.TextMeshProUGUI descText;

    private static LastLoadLevelContainer lastLoadLevel;

    private void Awake()
    {
        Instance = this;

        if (lastLoadLevel == null)
        {
            OnClose();
            return;
        }
        pressKeyText.SetActive(false);
        if (lastLoadLevel != null)
        {
            pressKeyText.SetActive(true);
            img.sprite = lastLoadLevel.sprite;
            descText.SetText(lastLoadLevel.description);
        }
    }
    public void LoadLevel(int level, int currentLevel)
    {
        var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(currentLevel == -1 ? level : currentLevel);
        background.SetActive(true);
        int imgIt = Random.Range(1, 8);
        
        using (StreamReader sr = new StreamReader($"{Directory.GetCurrentDirectory()}\\Localization\\LoadScreens\\LoadTexts\\{imgIt}.txt",System.Text.Encoding.GetEncoding(1251)))
        {
            string text = sr.ReadToEnd();
            descText.SetText(text);
            lastLoadLevel = new LastLoadLevelContainer(operation, img.sprite = Resources.Load<Sprite>($"LoadScreensImages\\{imgIt}\\LoadImage_{imgIt}"), text);
        }

    }
    private void Update()
    {
        if (lastLoadLevel != null)
        {
            text.SetText($"{lastLoadLevel.GetProggres()}/100%");
            if (Input.anyKeyDown)
            {
                OnClose();
            }
        }
    }
    private void OnClose()
    {
        background.SetActive(false);
        lastLoadLevel = null;
        Input.ResetInputAxes();
        img.sprite = null;
    }

    public class LastLoadLevelContainer
    {
        private readonly AsyncOperation operation;
        public readonly string description;
        public float GetProggres() => operation == null ? operation.progress : 100;
        public readonly Sprite sprite;
        public LastLoadLevelContainer(AsyncOperation op, Sprite s, string t)
        {
            operation = op;
            sprite = s;
            description = t;
        }
    }
}
