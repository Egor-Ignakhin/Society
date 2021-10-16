using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Society.Dialogs
{
    internal sealed class SanSanychPerson : TalkingPerson
    {
        protected override string PathToClips() => "Dialogs\\Other\\SanSanych\\";
        protected override void Awake()
        {
            base.Awake();
            dialogs = new List<(DialogType type, string mainData, string shortData, bool IsBreakDialog)>
    {
        ( DialogType.Player,"Даров, Сан Саныч, чего случилось?",null,false ),
        (DialogType.Opponent,"Хана случилась, Димон. Главный фильтр накрылся медным тазом, запасов хватит дай бог если на пару недель," +
        " ну а дальше нам останется только в бутылочку ссать. Если сейчас с этой ебаторией не порешать – то крышка нам.", null,false),
        (DialogType.Player," М-да, проблема серьёзная. Починить фильтры никак нельзя?","Починить фильтры никак нельзя?",false ),
        (DialogType.Opponent," Ишь чего удумал, починить. Да хуй там плавал, можно было бы " +
        "– давно бы уже всё изолентой синенькой перемотал, долбанул пару раз и готово. Тут только менять, а запаски у нас нема.", null,false),
        (DialogType.Player,"И что предлагаешь делать? Не померать же нам тут всем от обезвоживания", "И как быть?",false),
        (DialogType.Opponent,"Вообще идейка есть, но рисковая." +
        " На поверхность мы ещё не шастали, но походу придётся. В соседнем доме есть очистная" +
        " станция – там фильтры должны быть. Понимаю, шаг непростой, но без этих фильтров" +
        " мы так и так помрём, так что деваться некуда. Иди возьми у Ильи с Гришей" +
        " снарягу, и отчаливай. И да, постарайся вернуться с тем же" +
        " количеством конечностей, что и сейчас. И лучше без зеленоватого свечения.", null,false),
        (DialogType.Player,"Напутствия это конечно не твоё, Сан Саныч. Ладно, деваться всё равно некуда. Постараюсь что-нибудь раздобыть", "Ладно, тогда пошёл собираться..",true),

        (DialogType.Opponent,"Слушай, Дим, я понимаю, дело рискованной, но ты уж постарайся вернуться. На вот, чтобы уж точно не пропал *Всучивает аптечку*", null,false),
        (DialogType.Player,"Спасибо, Сан Саныч, постараюсь.", "Спасибо, Сан Саныч, постараюсь.",true)
        };
        }
        protected override IEnumerator DialogsTraker()
        {
            while (true)
            {
                //если говорит дима то звук 2д иначе 3д

                if (currentDialog > 2 && dialogs[currentDialog - 2].IsBreakDialog)
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
    }
}