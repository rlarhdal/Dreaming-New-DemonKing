using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // ������Ʈ ĳ��(new)
    GameObject clone;

    // Ǯ�� ������ ������Ʈ�� �߰��Ѵ�.
    void Start()
    {
        PoolAdd($"Projectile/Arrow");
        PoolAdd($"Projectile/Explosion");
        PoolAdd($"Projectile/SwordAura");
        PoolAdd($"Projectile/SwordExplosion");
        PoolAdd($"Projectile/SpecialAura");
        PoolAdd($"Entity/Soul");
    }

    void PoolAdd(string adress)
    {
        clone = Resources.Load<GameObject>(adress);
        Managers.Pool.CreatePool(clone, 20);
    }
}
