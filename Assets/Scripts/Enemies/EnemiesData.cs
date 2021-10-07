using System.Collections.Generic;
namespace Society.Enemies
{
    public static class EnemiesData
    {
        private static readonly List<Enemy> enemies = new List<Enemy>();
        public static void AddEnemy(Enemy e) => enemies.Add(e);
        public static void RemoveEnemy(Enemy e) => enemies.Remove(e);
        public static List<Enemy> GetCollection() => enemies;
    }
}