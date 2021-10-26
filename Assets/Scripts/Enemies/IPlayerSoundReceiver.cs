using Society.Player;

using UnityEngine;

namespace Society.Enemies
{
    public interface IPlayerSoundReceiver
    {
        Transform Transform { get; }

        float GetDistanceToTarget();
        void SetPlayer(BasicNeeds bn);
    }
}