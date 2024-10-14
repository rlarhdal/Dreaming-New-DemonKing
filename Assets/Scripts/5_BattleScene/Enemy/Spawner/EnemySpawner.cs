using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum EnemySub
{
    Farmer,
    Hunter,
    Count
}

public enum EnemyCenter
{
    AdventurerM,
    AdventurerR,
    Count
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoint;
    NormalMapManager currentMapManager;
    GameObject enemy;

    private void Awake()
    {
        currentMapManager = transform.root.GetComponent<NormalMapManager>();
    }

    private void Start()
    {
        Invoke("SpawnStart", 1.0f);
    }


    private void SpawnStart()
    {
        enemy = Managers.Pool.Pop(Resources.Load<GameObject>($"Enemy/{(EnemyCenter)(Random.Range(0, (int)EnemyCenter.Count))}"), transform).gameObject;
        enemy.GetComponentInChildren<Enemy>().Init(Managers.Game.player.transform);
        SpawnSetting(ref enemy, transform);

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            if (i == spawnPoint.Length - 1)
            {
                enemy = Managers.Pool.Pop(Resources.Load<GameObject>($"Enemy/Citizen"), spawnPoint[i]).gameObject;
                enemy.transform.position = spawnPoint[i].transform.position;
                enemy.GetComponentInChildren<CitizenMovement>().runPos =
                    currentMapManager.GetPos(mapObject.escapePortal);
                currentMapManager.AddCount();
            }
            else
            {
                enemy = Managers.Pool.Pop(Resources.Load<GameObject>($"Enemy/{(EnemySub)(Random.Range(0, (int)EnemySub.Count))}"), spawnPoint[i]).gameObject;
                SpawnSetting(ref enemy, spawnPoint[i]);
            }
            enemy.GetComponentInChildren<Enemy>().Init(Managers.Game.player.transform);
        }
    }

    private void SpawnSetting(ref GameObject enemy, Transform transform)
    {
        enemy.transform.position = transform.position;
        enemy.GetComponentInChildren<NormalEnemyMovement>().turnPos = transform.position;
        currentMapManager.AddCount();
    }
}
