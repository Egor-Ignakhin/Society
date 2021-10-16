using Society.Player;

using UnityEngine;

namespace Society.Enemies
{
    public interface IPlayerSoundReceiver
    {
        Transform GetTransform();
        float GetDistanceToTarget();
        void SetPlayer(BasicNeeds bn);
    }
}