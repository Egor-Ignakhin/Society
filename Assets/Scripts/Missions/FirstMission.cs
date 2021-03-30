using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMission : Mission
{
    [SerializeField] private MissionsManager missionsManager;

    [SerializeField] private ChairMesh startChair;
    private Image backgroundWhiteImage;
    private Image backgroundDarkImage;
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

    private bool part2IsStarted;
    public override void ContinueMission(int skipLength)
    {
        currentTask = skipLength;
        if (skipLength == 0)
        {
            CreateBackground(true);
            PutOnAChair();
            ListenMessage();
            WriteNote();
            Comment(0);
        }
        SetTask(currentTask);
        SetActiveCheckers();
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
    private void CreateBackground(bool whited)
    {
        Canvas effectsCanvas = missionsManager.GetEffectsCanvas();
        if (whited)
        {
            GameObject image = new GameObject("WhiteBackground");
            image.transform.SetParent(effectsCanvas.transform);
            backgroundWhiteImage = image.AddComponent<Image>();
            backgroundWhiteImage.gameObject.AddComponent<LighteningBackground>().Init(backgroundWhiteImage, 0.25f);
            backgroundWhiteImage.transform.localScale = effectsCanvas.GetComponent<CanvasScaler>().referenceResolution / 100;
            backgroundWhiteImage.transform.localPosition = Vector2.zero;
        }
        else// blacked
        {
            GameObject image = new GameObject("BlackBackground");
            image.transform.SetParent(effectsCanvas.transform);
            backgroundDarkImage = image.AddComponent<Image>();
            backgroundDarkImage.gameObject.AddComponent<DarkiningBackground>().Init(backgroundDarkImage, 0.5f);
            backgroundDarkImage.transform.localScale = effectsCanvas.GetComponent<CanvasScaler>().referenceResolution / 100;
            backgroundDarkImage.transform.localPosition = Vector2.zero;
        }
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

        catch (System.IndexOutOfRangeException)
        {
            neededContent = "Ошибка при прочтении задания : недостаточно заданий";
        }

        missionsManager.GetTaskDrawer().DrawNewTask(neededContent);
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
        private const int codeForAT = 6;// code for add tracker
        private const int codeForRT = 10;// code for remove tracker
        private const int codeForIFD = 11;// code for instance flock dogs
        private const int codeForECD = 12;// code for extrim close door
        private const int codeForTPMD = 13;// code for translate player to main door
        private const int codeForDB = 15;// code for darkining background
        public static void CheckTaskForPossibleDoing(int i, FirstMission mission)
        {
            switch (i)
            {
                case codeForAT:
                    AddTracker(mission);
                    break;
                case codeForRT:
                    RemoveTracker(mission);
                    break;
                case codeForIFD:
                    InstanceFlockDogs(mission);
                    break;
                case codeForECD:
                    ExtrimCloseDoor(mission);
                    break;
                case codeForTPMD:
                    TranslatePlayerToMainDoor(mission);
                    break;
                case codeForDB:
                    DarkiningBackground(mission);                    
                    break;
            }
        }
        private static void InstanceFlockDogs(FirstMission mission)
        {
            mission.dogsForInstance.SetActive(true);
            foreach (var d in mission.dogsFI)
            {
                d.SetEnemy(mission.missionsManager.GetPlayerBasicNeeds());
                d.SetCurrentEnemyForewer(true);
            }
        }
        private static void ExtrimCloseDoor(FirstMission mission)
        {
            mission.doorOnSurface.SetExtrimSituation(true);
            mission.doorOnSurface.SetStateAfterNextInteract(State.locked);
        }
        private static void AddTracker(FirstMission mission)
        {
            mission.deadMan.AddComponent<Maps.PointOnMap>();
        }
        private static void RemoveTracker(FirstMission mission)
        {
            Destroy(Maps.MapManager.GetTracker(mission.deadMan));
        }
        private static void TranslatePlayerToMainDoor(FirstMission mission)
        {
            var playerT =FindObjectOfType<FirstPersonController>();
            playerT.SetPosAndRot(mission.startPointBeforePart2);
        }
        private static void DarkiningBackground(FirstMission mission)
        {
            mission.CreateBackground(false);
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
            part2IsStarted = true;
            taskChecker2Part.SetActive(true);
        }
        for (int i = 0; i < checkers.Count; i++)
        {
            checkers[i].SetInteracted(i < currentTask);
        }
    }
}
