using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] GameObject bossObject;
    GameObject enemy;

    public void SpawnStart()
    {
        enemy = Managers.Pool.Pop(bossObject, transform).gameObject;
        enemy.transform.position = transform.position;
    }
}
