using System.Threading.Tasks;

using TMPro;

using UnityEngine;

namespace Society.Missions.TaskSystem
{
    internal sealed class CommentDrawer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            text.text = string.Empty;
        }
        internal async void Push(string v)
        {
            text.text = v;

            await Task.Delay(4000);

            text.text = string.Empty;
        }
    }
}