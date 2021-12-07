using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Society.Dialogs
{
    public class YanNPC : NPC
    {
        protected override IEnumerator DialogsTraker()
        {
            throw new NotImplementedException();
        }

        protected override List<(int mission, int task)> GetInteractableTasksMissions()
        {
            throw new NotImplementedException();
        }

        protected override string PathToClips()
        {
            throw new NotImplementedException();
        }
    }
}