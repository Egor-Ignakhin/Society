using System.Collections;
using System.Collections.Generic;

using Society.Missions;

using UnityEngine;
namespace Society.Dialogs
{
    public class IlyaiNPC : NPC
    {
        protected override string PathToClips() => "Dialogs\\Other\\IlyaDialogs\\Ilya_";
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

        protected override IEnumerator DialogsTraker()
        {
            //MissionsManager.Instance.GetTaskDrawer().SetVisible(false);
            while (true)
            {
                //если говорит дима то звук 2д иначе 3д
                if ((currentAsk > 1) && dialogs[currentAsk - 2].IsBreakDialog)
                {
                    FinishDialog();
                    break;
                }

                if ((dialogs[currentAsk - 1].dt == DialogType.Player) && (currentAsk != 1))
                {
                    Dialogs.DialogDrawer drawer = FindObjectOfType<Dialogs.DialogDrawer>();
                    drawer.SetAnswers((dialogs[currentAsk - 1].answerText, TakeAnswerInDialog), (string.Empty, null), (string.Empty, null));
                    answerInDialogHasTaked = false;
                    while (!answerInDialogHasTaked)
                    {
                        yield return null;
                    }
                }
                personSource.spatialBlend = (dialogs[currentAsk - 1].dt == DialogType.Player ? 0 : 1);
                var clip = Resources.Load<AudioClip>($"{PathToClips()}{currentAsk}");
                personSource.PlayOneShot(clip);
                var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
                string pName = (dialogs[currentAsk - 1].dt == DialogType.Player ? "Вы" : personName);
                ddrawer.DrawPersonDialog(pName, dialogs[currentAsk - 1].screenText);
                clipLingth = clip.length;
                currentAsk++;

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
            //MissionsManager.Instance.TaskDrawer.SetVisible(true);
            MissionsManager.Instance.GetActiveMission().Report();
        }

        protected override List<(int mission, int task)> GetInteractableTasksMissions()
        {
            throw new System.NotImplementedException();
        }
    }
}