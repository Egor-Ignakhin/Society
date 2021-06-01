using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// класс который отвечает за привлечение тварей к игроку
/// </summary>
public class PlayerSoundsCalculator : MonoBehaviour
{
    [SerializeField] private RectTransform drawingImage;
    private float additionalNoise = 0;//добавляемый шум от оружия
    private const float NoiseMax = 100;
    private Transform player;

    private float playerSpeed;// шум от ходьбы персонажа
    public void AddNoise(float v)
    {
        additionalNoise += v;
        if (additionalNoise > NoiseMax)
            additionalNoise = NoiseMax;
    }
    public void SetPlayerSpeed(float s) => playerSpeed = s * 100;

    private IEnumerator Start()
    {
        player = FindObjectOfType<FirstPersonController>().transform;
        StartCoroutine(nameof(CallMonsters));
        while (true)
        {
            Calculate();
            Draw();
            yield return new WaitForFixedUpdate();
        }
    }

    private void Calculate()
    {
        if (additionalNoise > 0)
            additionalNoise -= 0.5f;
    }
    private void Draw() => drawingImage.sizeDelta = new Vector2(drawingImage.sizeDelta.x, playerSpeed + additionalNoise);
    private IEnumerator CallMonsters()
    {
        while (true)
        {
            FindMonsters();
            yield return new WaitForSeconds(1);
        }
    }
    private void OnDestroy() => StopAllCoroutines();
    private async void FindMonsters()
    {
        var startedCollection = MonstersData.GetCollection();
        var poses = new List<Vector3>();
        startedCollection.RemoveAll(e => e == null);
        foreach (var e in startedCollection)
        {
            poses.Add(e.transform.position);
        }

        var calledMonsters = new List<Enemy>();
        Vector3 playerPos = player.position;
        //асинхронный подсчёт дистанции

        await Task.Run(() =>
        {
            for (int i = 0; i < poses.Count; i++)
            {
                if (Vector3.Distance(playerPos, poses[i]) > (playerSpeed + additionalNoise))
                    continue;

                calledMonsters.Add(startedCollection[i]);
            }
        });

        foreach (var e in calledMonsters)
        {
            e.SetEnemy(PlayerClasses.BasicNeeds.Instance, true);
        }
    }
}

