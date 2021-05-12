using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// класс который отвечает за привлечение тварей к игроку
/// </summary>
public class PlayerSoundsCalculator : MonoBehaviour
{
    [SerializeField] private RectTransform drawingImage;
    private float Noise = 0;
    private const float NoiseMax = 100;
    private Transform player;
    private bool mustCall = false;//нужно ли вызывать поиск 
    public void AddNoise(float v)
    {
        Noise += v;
        if (Noise > NoiseMax)
            Noise = NoiseMax;
        mustCall = true;
    }
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
        if (Noise > 0)
            Noise -= 1;
    }
    private void Draw() => drawingImage.sizeDelta = new Vector2(drawingImage.sizeDelta.x, Noise);
    private IEnumerator CallMonsters()
    {
        while (true)
        {
            if (mustCall)
            {
                FindMonsters();
            }
            yield return new WaitForSeconds(1);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private async void FindMonsters()
    {
        var startedCollection = MonstersData.GetCollection();
        var poses = new List<Vector3>();        
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
                if (Vector3.Distance(playerPos, poses[i]) < Noise)
                {
                    print(1 / Vector3.Distance(playerPos, poses[i]));
                    if ((Noise / Vector3.Distance(playerPos, poses[i])) > 0.02f)
                    {
                        calledMonsters.Add(startedCollection[i]);
                    }
                }
            }
        });

        foreach (var e in calledMonsters)
        {
            e.SetEnemy(PlayerClasses.BasicNeeds.Instance, true);
        }
        mustCall = false;
    }
}

