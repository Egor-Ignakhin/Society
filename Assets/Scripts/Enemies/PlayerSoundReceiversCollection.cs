using System.Collections.Generic;
namespace Society.Enemies
{
    /// <summary>
    /// Коллекция улавливателей звуков игрока
    /// </summary>
    public static class PlayerSoundReceiversCollection
    {
        private static readonly List<IPlayerSoundReceiver> playerSoundReceivers = new List<IPlayerSoundReceiver>();
        public static void AddListner(IPlayerSoundReceiver e) => playerSoundReceivers.Add(e);
        public static void RemoveListner(IPlayerSoundReceiver e) => playerSoundReceivers.Remove(e);
        public static List<IPlayerSoundReceiver> GetCollection() => playerSoundReceivers;
    }
}