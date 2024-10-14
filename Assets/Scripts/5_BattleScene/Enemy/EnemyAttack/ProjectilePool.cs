using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // 오브젝트 캐싱(new)
    GameObject clone;

    // 풀에 저장할 오브젝트를 추가한다.
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
