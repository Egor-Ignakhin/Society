using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMission : Mission
{
    [SerializeField] private MissionsManager missionsManager;

    [SerializeField] private ChairMesh startChair;
    private bool messageWasListened = false;
    [ShowIf(nameof(startChair), true)] [SerializeField] private GameObject farewallNote;
    [ShowIf(nameof(startChair), true)] [SerializeField] private Transform fNTransform;
    [ShowIf(nameof(startChair), true)] [SerializeField] private Transform startStayPlace;

    [Space(15)]
    [SerializeField] private GameObject dogsForInstance;
    [ShowIf(nameof(dogsForInstance), true)] [SerializeField] private GameObject deadMan;
    [ShowIf(nameof(dogsForInstance), true)] [SerializeField] private List<DogEnemy> dogsFI = new List<DogEnemy>();
    [ShowIf(nameof(dogsForInstance), true)] [SerializeField] private DoorManager doorOnSurface;

    [SerializeField] private List<TaskChecker> checkers = new List<TaskChecker>();
    private const int part2Task = 13;
    [SerializeField] private GameObject part2Object;
    [ShowIf(nameof(startChair), true)] [SerializeField] private Transform startPointBeforePart2;
    [ShowIf(nameof(startChair), true)] [SerializeField] private MusicPlayer musicPlayer;
    [ShowIf(nameof(startChair), true)] [SerializeField] private TaskChecker taskChecker2Part;

    GameObject finishBackground;
    public override void ContinueMission(int skipLength)
    {
        if (messageWasListened)
        {

        }
        currentTask = skipLength;
        SetTask(currentTask);
        SetActiveCheckers();
    }
    public override void FinishMission()
    {
        missionsManager.FinishMission(0);
        Destroy(finishBackground);
        gameObject.SetActive(false);
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
    private IDelayable CreateBackground(bool whited)
    {
        var effectsCanvas = missionsManager.GetEffectsCanvas();
        IDelayable delayable;
        GameObject image;
        if (whited)
        {
            image = new GameObject("WhiteBackground");
            Image backgroundWhiteImage = image.AddComponent<Image>();

            LighteningBackground background = backgroundWhiteImage.gameObject.AddComponent<LighteningBackground>();
            delayable = background;

            image.transform.SetParent(effectsCanvas.transform);
            background.Init(backgroundWhiteImage, 0.25f);
            backgroundWhiteImage.transform.localScale = effectsCanvas.GetComponent<CanvasScaler>().referenceResolution / 100;
            backgroundWhiteImage.transform.localPosition = Vector2.zero;
        }
        else// blacked
        {
            finishBackground = image = new GameObject("BlackBackground");
            Image backgroundDarkImage = image.AddComponent<Image>();

            DarkiningBackground background = backgroundDarkImage.gameObject.AddComponent<DarkiningBackground>();
            delayable = background;

            image.transform.SetParent(effectsCanvas.transform);
            background.Init(backgroundDarkImage, 0.5f);
            backgroundDarkImage.transform.localScale = effectsCanvas.GetComponent<CanvasScaler>().referenceResolution / 100;
            backgroundDarkImage.transform.localPosition = Vector2.zero;
        }
        return delayable;
    }
    /// <summary>
    /// переключается радио и слушается сигнал
    /// </summary>
    private void ListenMessage()
    {
        //TODO: сделать переключение и калибровку радио для прослушивания сообщения       
        messageWasListened = true;
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
        Dialogs.DialogDrawer.Instance.DrawNewDialog(dialog, 4);
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

        catch (System.IndexOutOfRangeException)
        {
            neededContent = "Ошибка при прочтении задания : недостаточно заданий";
        }

        TaskDrawer.Instance.DrawNewTask(neededContent);
        DoingsInMission.CheckTaskForPossibleDoing(i, this);
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

    static class DoingsInMission
    {
        private static FirstMission mission;

        private const int codeForSM = 0;//code for start mission
        private const int codeForOFB = 4;// code for outside from bunker
        private const int codeForAT = 6;// code for add tracker
        private const int codeForRT = 10;// code for remove tracker
        private const int codeForIFD = 11;// code for instance flock dogs
        private const int codeForECD = 12;// code for extrim close door
        private const int codeForTPMD = 13;// code for translate player to main door
        private const int codeForDB = 15;// code for darkining background
        public static void CheckTaskForPossibleDoing(int i, FirstMission m)
        {
            mission = m;
            switch (i)
            {
                case codeForSM:
                    StartMission();
                    break;
                case codeForOFB:
                    OutsideFromBunker();
                    break;
                case codeForAT:
                    AddTracker();
                    break;
                case codeForRT:
                    RemoveTracker();
                    break;
                case codeForIFD:
                    InstanceFlockDogs();
                    break;
                case codeForECD:
                    ExtrimCloseDoor();
                    break;
                case codeForTPMD:
                    TranslatePlayerToMainDoor();
                    break;
                case codeForDB:
                    DarkiningBackground();
                    break;
            }
        }
        private static void StartMission()
        {
            mission.CreateBackground(true);
            mission.PutOnAChair();
            mission.ListenMessage();
            mission.WriteNote();
            mission.Comment(0);
        }
        private static void OutsideFromBunker()
        {
            mission.Comment(1);
        }
        private static void InstanceFlockDogs()
        {
            mission.dogsForInstance.SetActive(true);
            foreach (var d in mission.dogsFI)
            {
                d.SetEnemy(mission.missionsManager.GetPlayerBasicNeeds());
                d.SetCurrentEnemyForewer(true);
            }
        }
        private static void ExtrimCloseDoor()
        {
            mission.doorOnSurface.SetExtrimSituation(true);
            mission.doorOnSurface.SetStateAfterNextInteract(State.locked);
        }
        private static void AddTracker()
        {
            mission.deadMan.AddComponent<Maps.PointOnMap>();
        }
        private static void RemoveTracker()
        {
            Destroy(Maps.MapManager.GetTracker(mission.deadMan));
        }
        private static void TranslatePlayerToMainDoor()
        {
            if (mission.startChair == null)
                return;
            var playerT = FindObjectOfType<FirstPersonController>();
            playerT.SetPosAndRot(mission.startPointBeforePart2);
        }
        private static void DarkiningBackground()
        {
            mission.CreateBackground(false).FinishPart += mission.FinishMission;
        }
    }
    private const int dogsToKill = 3;
    private int killedDogs = 0;
    public void KillTheDogs()
    {
        if (++killedDogs == dogsToKill)
        {
            Report();
        }
    }
    public bool PossibleMoveToBunker()
    {
        return doorOnSurface.GetState() == State.locked;
    }
    private void SetActiveCheckers()
    {
        if (currentTask == part2Task)
        {
            part2Object.SetActive(true);
            musicPlayer.DisablePlayer();
            taskChecker2Part.SetActive(true);
        }
        for (int i = 0; i < checkers.Count; i++)
        {
            checkers[i].SetInteracted(i < currentTask);
        }
    }
}
