using Society.Enemies;
using Society.Inventory;
using Society.Player.Controllers;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Society.Player
{
    /// <summary>
    /// класс который отвечает за привлечение тварей к шумным местам
    /// </summary>
    public sealed class PlayerSoundsCalculator : MonoBehaviour
    {
        [SerializeField] private RectTransform drawingImage;
        private float additionalNoise = 0;//добавляемый шум от оружия
        private const float NoiseMax = 100;
        private Transform player;

        private float playerSpeed;// шум от ходьбы персонажа        
        private static BasicNeeds basicNeeds;
        public void AddNoise(float v)
        {
            additionalNoise += v;
            if (additionalNoise > NoiseMax)
                additionalNoise = NoiseMax;
        }
        public void SetPlayerSpeed(float s) => playerSpeed = s;

        private IEnumerator Start()
        {
            player = FindObjectOfType<FirstPersonController>().transform;
            basicNeeds = BasicNeeds.Instance;
            StartCoroutine(nameof(CallMonsters));
            while (true)
            {
                Calculate();
                Draw();
                yield return new WaitForFixedUpdate();
            }
        }

        internal static void AddItemSound(InventoryItem item)
        {
            decimal wight = ItemStates.GetWeightItem(item.Id);
            Vector3 itemPos = item.transform.position;
            var startedCollection = EnemiesData.GetCollection();//все монстры на локации
            startedCollection.RemoveAll(e => e == null);

            for (int i = 0; i < startedCollection.Count; i++)// проходимся по всем позициям монстров
            {
                if (startedCollection[i].HasEnemy())
                    continue;

                if (Vector3.Distance(itemPos, startedCollection[i].transform.position) < startedCollection[i].GetDistanceToTarget())// если дистанция между позицией игрока и позицией монстра > шум от ходьбы игрока + добавляемый шум
                {
                    startedCollection[i].SetEnemy(basicNeeds, true, item.transform);
                }
            }
        }

        private void Calculate()
        {
            if (additionalNoise > 0)
                additionalNoise -= 0.5f;
        }
        private void Draw() => drawingImage.sizeDelta = new Vector2(drawingImage.sizeDelta.x, Mathf.Clamp(playerSpeed + additionalNoise, 0, 100));
        private IEnumerator CallMonsters()
        {
            while (true)
            {
                FindMonsters();
                yield return new WaitForSeconds(1);
            }
        }
        private void OnDestroy() => StopAllCoroutines();
        private void FindMonsters()
        {
            var startedCollection = EnemiesData.GetCollection();//все монстры на локации
            startedCollection.RemoveAll(e => e == null);
            var montsterPoses = new List<Vector3>();// лист позиций монстров
            foreach (var e in startedCollection)//добавление в лист позиций все позиции монстров
                montsterPoses.Add(e.transform.position);

            Vector3 playerPos = player.position;// позиция игрока

            for (int i = 0; i < montsterPoses.Count; i++)// проходимся по всем позициям монстров
            {
                if (Vector3.Distance(playerPos, montsterPoses[i]) > (playerSpeed + additionalNoise))// если дистанция между позицией игрока и позицией монстра > шум от ходьбы игрока + добавляемый шум
                    continue;//пропускаем

                startedCollection[i].SetEnemy(basicNeeds, true);// указываем их цель как игрока
            }
        }
    }
}