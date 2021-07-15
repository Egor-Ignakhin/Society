using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class TalkingPerson : InteractiveObject, IGameScreen
{
    [SerializeField] private Transform cameraPlace;
    private Transform lastCameraParent;
    private Vector3 neededPosition;
    private Quaternion neededRotation;
    private int personLevel = 1;
    [SerializeField] private string personName = "N/A";
    [SerializeField] private string fraction = "N/A";
    [SerializeField] private string personRelationAtPlayer = "N/A";

    private AudioSource personSource;
    private TaskChecker mtaskChecker;
    private List<(DialogType dt, string text)> dialogs = new List<(DialogType, string)>//1 is dima. 2 is sansanych
    {
        ( DialogType.Player,"Даров, Сан Саныч, чего случилось?" ),
        (DialogType.SanSanych,"Хана случилась, Димон. Главный фильтр накрылся медным тазом, запасов хватит дай бог если на пару недель," +
        " ну а дальше нам останется только в бутылочку ссать. Если сейчас с этой ебаторией не порешать – то крышка нам."),
        (DialogType.Player," М-да, проблема серьёзная. Починить фильтры никак нельзя?"),
        (DialogType.SanSanych," Ишь чего удумал, починить. Да хуй там плавал, можно было бы " +
        "– давно бы уже всё изолентой синенькой перемотал, долбанул пару раз и готово. Тут только менять, а запаски у нас нема."),
        (DialogType.Player,"И что предлагаешь делать? Не померать же нам тут всем от обезвоживания"),
        (DialogType.SanSanych,"Вообще идейка есть, но рисковая." +
        " На поверхность мы ещё не шастали, но походу придётся. В соседнем доме есть очистная" +
        " станция – там фильтры должны быть. Понимаю, шаг непростой, но без этих фильтров" +
        " мы так и так помрём, так что деваться некуда. Иди возьми у Ильи с Гришей" +
        " снарягу, и отчаливай. И да, постарайся вернуться с тем же" +
        " количеством конечностей, что и сейчас. И лучше без зеленоватого свечения."),
        (DialogType.Player,"Напутствия это конечно не твоё, Сан Саныч. Ладно, деваться всё равно некуда. Постараюсь что-нибудь раздобыть")
    };
    private int currentDialog = 1;
    private enum DialogType { SanSanych, Player }
    [SerializeField] private bool canLeaveFromDialog;
    private void Start()
    {
        mtaskChecker = GetComponent<TaskChecker>();
        personSource = GetComponent<AudioSource>();
        if (cameraPlace == null)
            Debug.LogError("Camera place is null!");
    }
    public override void Interact()
    {
        var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
        ddrawer.SetEnableAll(true);
        ddrawer.SetNameAndLevel(personName, personLevel);
        ddrawer.SetRelationAtPlayer(personRelationAtPlayer);
        ddrawer.SetFraction(fraction);

        var cameraPlayer = Camera.main.transform;
        lastCameraParent = cameraPlayer.parent;
        cameraPlayer.SetParent(null);
        neededPosition = cameraPlace.position;
        neededRotation = cameraPlace.rotation;
        ScreensManager.SetScreen(this);
    }
    public void FinishDialog()
    {
        MissionsManager.GetCurrentMission().Report();
        var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
        ddrawer.SetEnableAll(false);


        var cameraPlayer = Camera.main.transform;
        cameraPlayer.SetParent(lastCameraParent);
        cameraPlayer.localPosition = Vector3.zero;
        cameraPlayer.localRotation = Quaternion.identity;
        lastCameraParent = null;
        ScreensManager.SetScreen(null);
    }

    internal void PlayDialogsTraker()
    {
        StartCoroutine(nameof(DialogsTraker));
    }

    private IEnumerator DialogsTraker()
    {
        while (true)
        {
            //если говорит дима то звук 2д иначе 3д
            if ((currentDialog - 1) == dialogs.Count)
            {
                FinishDialog();
                break;
            }

            print(dialogs[currentDialog - 1].dt == DialogType.Player);
            print(currentDialog != 1);
            if ((dialogs[currentDialog - 1].dt == DialogType.Player) && (currentDialog != 1))
            {
                while (true)
                    yield return null;
            }
            personSource.spatialBlend = (dialogs[currentDialog - 1].dt == DialogType.Player ? 0 : 1);
            var clip = Resources.Load<AudioClip>($"Dialogs\\Other\\SanSanych_{currentDialog}");
            personSource.PlayOneShot(clip);
            var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
            string pName = (dialogs[currentDialog - 1].dt == DialogType.Player ? "Вы" : "Сан Саныч");
            ddrawer.DrawPersonDialog(pName, dialogs[currentDialog - 1].text);
            currentDialog++;

            yield return new WaitForSeconds(clip.length);
        }
    }

    internal void Say(AudioClip audioClip)
    {
        personSource.PlayOneShot(audioClip);
    }

    public void LateUpdate()
    {
        if (lastCameraParent)
        {
            var cameraPlayer = Camera.main.transform;
            cameraPlayer.position = neededPosition;
            cameraPlayer.rotation = neededRotation;
        }
    }

    public void Hide()
    {
        if (canLeaveFromDialog)
            FinishDialog();
    }

    public KeyCode HideKey()
    {
        return KeyCode.Escape;
    }
}
