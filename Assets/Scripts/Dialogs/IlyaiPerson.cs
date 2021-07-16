using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlyaiPerson : TalkingPerson
{
    protected override void Awake()
    {
        base.Awake();

        dialogs = new List<(DialogType, string, string, bool IsBreakDialog)>
    {
        ( DialogType.Player,"Привет, Илюх. Где Гриша?",null,false ),
        (DialogType.Opponent,"Отошёл набить брюхо. А ты по какому поводу? Ужин ещё только через полчаса.", null,false),
        (DialogType.Player,"Полчаса до ужина, Гриня уже хомячит… В его стиле. То-то он сразу на пост завпровизией сразу претендовал." +
        " Ладно, не в этом дело, у нас фильтры полетели", "Мда..",false ),
        (DialogType.Opponent,"Знаю, Саныч уже заходил, нет больше фильтров", null,false),
        (DialogType.Player,"В этом и загвоздка. Фильтров нет, вода тоже кончается, так что долго без них мы не протянем", "В этом и загвоздка",false),
        (DialogType.Opponent,"Умеешь ты обнадёживать..", null,false),
        (DialogType.Player,"Ну так вот, Саныч дал ориентировку, что в соседнем здании есть очистная станция…", "Мне тут Саныч подсказал...",false),
        (DialogType.Opponent,"Ты ж не хочешь..", null,false),
        (DialogType.Player, "Не хочу конечно, помирать никому не охота, да надо. Без фильтров мы в любом случае помрём, а так только я, если не повезёт.",
        "Не хочу конечно, помирать никому не охота, да надо. ",false),
        (DialogType.Opponent, "Ну в таком случае… Вот тебе противогаз, химза и антирад. Береги себя, Дим.", null,true)
    };
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

    public override void Hide()
    {
        if (canLeaveFromDialog)
            FinishDialog();
    }

    public override KeyCode HideKey() => KeyCode.Escape;

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

    protected override IEnumerator DialogsTraker()
    {
        while (true)
        {
            //если говорит дима то звук 2д иначе 3д
            if ((currentDialog > 1) && dialogs[currentDialog - 2].IsBreakDialog)
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
            string pName = (dialogs[currentDialog - 1].dt == DialogType.Player ? "Вы" : personName);
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

    protected override string PathToClips() => "Dialogs\\Other\\IlyaDialogs\\Ilya_";
}
