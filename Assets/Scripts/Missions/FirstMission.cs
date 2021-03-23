using UnityEngine;
using UnityEngine.UI;

public class FirstMission : Mission
{
   [SerializeField] private MissionsManager missionsManager;

    [SerializeField] private ChairMesh startChair;
    private Image backgroundWhiteImage;
    private bool messageWasListened = false;
    [SerializeField] private GameObject farewallNote;
    [SerializeField] private Transform fNTransform;
    [SerializeField] private Transform startStayPlace;    
    public override void StartOrContinueMission(int skipLength)
    {
        if (skipLength == 0)
        {
            CreateBackground();
            PutOnAChair();
            ListenMessage();
            WriteNote();
            Comment(0);
            SetTask(currentTask);
        }
        for(int i = 0; i < skipLength; i++)
        {
            currentTask++;
            SetTask(currentTask);
        }
    }
    public override void FinishMission()
    {
        throw new System.NotImplementedException();
    }
    public override int GetMissionNumber()
    {
        return 0;
    }
    /// <summary>
    /// событие чекпоинта
    /// </summary>
    public override void Report()
    {
        currentTask++;
        missionsManager.ReportTask();
        SetTask(currentTask);
    }
    /// <summary>
    /// игрока садят на табурет
    /// </summary>
    private void PutOnAChair()
    {
        var player = FindObjectOfType<PlayerClasses.PlayerStatements>();
        player.transform.position = startStayPlace.position;
        startChair.Interact(player);      
    }
    /// <summary>
    /// на специальном холсте для
    /// спец-эффектов вознкикает
    /// фоновый рисунок и постепенно тает
    /// </summary>
    private void CreateBackground()
    {        
        GameObject image = new GameObject("WhiteBackground");
        Canvas effectsCanvas = missionsManager.GetEffectsCanvas();
        image.transform.SetParent(effectsCanvas.transform);
        backgroundWhiteImage = image.AddComponent<Image>();
        backgroundWhiteImage.gameObject.AddComponent<LighteningBackground>().Init(backgroundWhiteImage, 0.25f);
        backgroundWhiteImage.transform.localScale = effectsCanvas.GetComponent<CanvasScaler>().referenceResolution / 100;
        backgroundWhiteImage.transform.localPosition = Vector2.zero;
    }
    /// <summary>
    /// переключается радио и слушается сигнал
    /// </summary>
    private void ListenMessage()
    {
        //TODO: сделать переключение и калибровку радио для прослушивания сообщения       
    }
    /// <summary>
    /// на столе пишется записка
    /// </summary>
    private void WriteNote()
    {
        //TODO: сделать анимацию записывания записки

        //В будущем заменить
        Instantiate(farewallNote, fNTransform.position, fNTransform.rotation);
        //В будущем заменить
    }
    /// <summary>
    /// Главный герой говорит комментарий
    /// </summary>
    /// <param name="i"></param>
    private void Comment(int i)
    {
        string[] allContent = System.IO.File.ReadAllLines(Localization.PathToCurrentLanguageContent(Localization.Type.Dialogs, GetMissionNumber()));
        string neededContent = allContent[i];
        Dialogs.Dialog dialog = new Dialogs.Dialog(neededContent);
        missionsManager.GetDialogDrawer().DrawNewDialog(dialog, 4);
    }
    /// <summary>
    /// назначение задачи 
    /// </summary>
    /// <param name="i"></param>
    private void SetTask(int i)
    {
        string[] allContent = System.IO.File.ReadAllLines(Localization.PathToCurrentLanguageContent(Localization.Type.Tasks, GetMissionNumber()));
        string neededContent;
        try
        {
           neededContent = DecodeTask(allContent[i]);
        }

        catch(System.IndexOutOfRangeException)
        {
           neededContent = "Ошибка при прочтении задания : недостаточно заданий";
        }

        missionsManager.GetTaskDrawer().DrawNewTask(neededContent);
    }    
    /// <summary>
    /// преобразование текста в нормальный текст, убираются спецсимволы
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string DecodeTask(string str)
    {
        string retStr = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].ToString() == @"\" && str[i + 1] == 'n')
            {
                retStr += "\n";
                i++;
            }
            else            
               retStr += str[i];            
        }
        return retStr;
    }
}
