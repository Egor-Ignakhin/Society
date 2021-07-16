using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class SanSanychPerson : TalkingPerson
{
    protected override void Awake()
    {
        base.Awake();

        dialogs = new List<(DialogType, string, string)>
    {
        ( DialogType.Player,"Даров, Сан Саныч, чего случилось?",null ),
        (DialogType.Opponent,"Хана случилась, Димон. Главный фильтр накрылся медным тазом, запасов хватит дай бог если на пару недель," +
        " ну а дальше нам останется только в бутылочку ссать. Если сейчас с этой ебаторией не порешать – то крышка нам.", null),
        (DialogType.Player," М-да, проблема серьёзная. Починить фильтры никак нельзя?","Починить фильтры никак нельзя?" ),
        (DialogType.Opponent," Ишь чего удумал, починить. Да хуй там плавал, можно было бы " +
        "– давно бы уже всё изолентой синенькой перемотал, долбанул пару раз и готово. Тут только менять, а запаски у нас нема.", null),
        (DialogType.Player,"И что предлагаешь делать? Не померать же нам тут всем от обезвоживания", "И как быть?"),
        (DialogType.Opponent,"Вообще идейка есть, но рисковая." +
        " На поверхность мы ещё не шастали, но походу придётся. В соседнем доме есть очистная" +
        " станция – там фильтры должны быть. Понимаю, шаг непростой, но без этих фильтров" +
        " мы так и так помрём, так что деваться некуда. Иди возьми у Ильи с Гришей" +
        " снарягу, и отчаливай. И да, постарайся вернуться с тем же" +
        " количеством конечностей, что и сейчас. И лучше без зеленоватого свечения.", null),
        (DialogType.Player,"Напутствия это конечно не твоё, Сан Саныч. Ладно, деваться всё равно некуда. Постараюсь что-нибудь раздобыть", "Ладно, тогда пошёл собираться..")
    };
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
    public override void FinishDialog()
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
    protected override IEnumerator DialogsTraker()
    {
        while (true)
        {
            //если говорит дима то звук 2д иначе 3д
            if ((currentDialog - 1) == dialogs.Count)
            {
                FinishDialog();
                break;
            }

            if ((dialogs[currentDialog - 1].dt == DialogType.Player) && (currentDialog != 1))
            {
                Dialogs.DialogDrawer drawer = FindObjectOfType<Dialogs.DialogDrawer>();
                drawer.SetAnswers((dialogs[currentDialog - 1].answerText, TakeAnswerInDialog), (string.Empty, null), (string.Empty, null));
                answerInDialogHasTaked = false;
                while (!answerInDialogHasTaked)
                {
                    yield return null;
                }
            }
            personSource.spatialBlend = (dialogs[currentDialog - 1].dt == DialogType.Player ? 0 : 1);
            var clip = Resources.Load<AudioClip>($"{PathToClips()}{currentDialog}");
            personSource.PlayOneShot(clip);
            var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
            string pName = (dialogs[currentDialog - 1].dt == DialogType.Player ? "Вы" : "Сан Саныч");
            ddrawer.DrawPersonDialog(pName, dialogs[currentDialog - 1].screenText);
            clipLingth = clip.length;
            currentDialog++;

            while (clipLingth > 0)
            {
                clipLingth -= Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (clipLingth > 0)
                    {
                        //SKIP
                        personSource.Stop();
                        clipLingth = 0;
                    }
                }
                yield return null;
            }
        }
    }

    public override void Hide()
    {
        if (canLeaveFromDialog)
            FinishDialog();
    }

    public override KeyCode HideKey() => KeyCode.Escape;

    protected override string PathToClips() => "Dialogs\\Other\\SanSanych_";
}
