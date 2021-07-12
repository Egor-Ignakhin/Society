using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreensManager : Singleton<LoadScreensManager>, IGameScreen
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
        if (pressKeyText)
            pressKeyText.SetActive(false);
        if (lastLoadLevel != null)
        {
            if (pressKeyText)
                pressKeyText.SetActive(true);
            if (img)
                img.sprite = lastLoadLevel.sprite;
            if (descText)
                descText.SetText(lastLoadLevel.description);
        }
    }

    public void LoadLevel(int level, int currentLevel)
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel == -1 ? level : currentLevel);
        if (background)
            background.SetActive(true);
        int imgIt = Random.Range(1, 8);

        string text = null;
        using (StreamReader sr = new StreamReader($"{Directory.GetCurrentDirectory()}\\Localization\\LoadScreens\\LoadTexts\\{imgIt}.txt", System.Text.Encoding.GetEncoding(1251)))
        {
            text = sr.ReadToEnd();
        }
        if (descText)
            descText.SetText(text);
        lastLoadLevel = new LastLoadLevelContainer(img ? img.sprite = Resources.Load<Sprite>($"LoadScreensImages\\{imgIt}\\LoadImage_{imgIt}") : null, text);
        ScreensManager.SetScreen(this);
    }
    private void Update()
    {
        if (lastLoadLevel != null)
        {
            if (Input.anyKeyDown)
            {
                OnClose();
            }
        }
    }
    private void OnClose()
    {
        if (background)
            background.SetActive(false);
        lastLoadLevel = null;
        Input.ResetInputAxes();
        if (img)
            img.sprite = null;
    }

    public void Hide()
    {

    }

    public KeyCode HideKey() => KeyCode.Escape;

    public class LastLoadLevelContainer
    {
        public readonly string description;
        public readonly Sprite sprite;
        public LastLoadLevelContainer(Sprite s, string t)
        {
            sprite = s;
            description = t;
        }
    }
}
