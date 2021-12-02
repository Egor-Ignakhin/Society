using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Society.Patterns;

using TMPro;

using UnityEngine;

namespace Society.Missions.TaskSystem
{
    public class CommentDrawer : Singleton<CommentDrawer>
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