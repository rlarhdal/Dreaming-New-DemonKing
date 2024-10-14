using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class EnemyStatus
{
    public string name;
    public MonsterType monsterType;
    public int health;
    public int attack;
    public float attackSpeed;
    public float attackRange;
    public AttackType attackType;
    public float moveSpeed;
    public int soul;
    public int exp;
}

public enum AttackType
{
    Melee,
    Ranged,
    Dual,
    None,
}

public enum MonsterType
{
    Normal,
    Rare,
    Named,
    Boss
}
