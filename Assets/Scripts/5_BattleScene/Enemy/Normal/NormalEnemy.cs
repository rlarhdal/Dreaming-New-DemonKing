using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class NormalEnemy : Enemy, IDamagable
{
    public Action attackEvent;
    public Action hitEvent;
    public Action moveEvent;
    public Action idleEvent;
    public Action dieEvent;

    private NormalEnemyMovement enemyMovement;
    Rigidbody2D rigid;
    public AudioClips clips;

    protected float attackTime = 0;
    protected GameObject dmgTxt;
    protected Canvas worldUICanvas;

    protected override void Awake()
    {
        base.Awake();
        enemyMovement = GetComponent<NormalEnemyMovement>();
        rigid = GetComponent<Rigidbody2D>();
        clips = GetComponent<AudioClips>();
        dmgTxt = Managers.Resource.Load<GameObject>("Prefabs/UI/Popup/DMGtxt");
        worldUICanvas = transform.parent.GetComponentInChildren<Canvas>();
        attackTime = status.attackSpeed * 2;
    }

    // 플레이어가 죽었을 때 충돌 여부를 다시 가능하게 함
    private void OnDisable()
    {
        rigid.simulated = true;
    }

    // 이벤트 변수를 추가
    private void Start()
    {
        hitEvent += AttackTimeClear;
    }

    private void Update()
    {
        // 공격 딜레이 시간이 끝나면 공격이 가능하게 변경하고
        // 공격 대기시간을 초기화한다.
        if (attackTime < 0)
        {
            AttackTimeClear();
            enemyMovement.attacking = false;
        }
        else if (attackTime > 0 && enemyMovement?.attacking == true)
        {
            attackTime -= Time.deltaTime;
        }
    }

    // 적의 공격 대기시간 초기화
    private void AttackTimeClear()
    {
        attackTime = status.attackSpeed * 2;
    }

    // 피격시 데미지를 입게 한다.
    public void ApplyDamage(float dmg)
    {
        clips.PlaySFX(EnemyClip.Hit);
        if (status.health <= dmg)
        {
            status.health -= (int)dmg;
            dieEvent?.Invoke();
            rigid.simulated = false;
        }
        else
        {
            if (status.attackType == AttackType.Ranged)
            {
                hitEvent?.Invoke();
            }
            status.health -= (int)dmg;
            Poolable curDMGtxt = Managers.Pool.Pop(dmgTxt, worldUICanvas.gameObject.transform);
            curDMGtxt.gameObject.transform.position = transform.position + 1.7f * Vector3.up;
            curDMGtxt.gameObject.GetComponent<DMGtxt>().SetInfo(dmg);
            
            if (enemyMovement?.GetState() == EnemyState.Idle)
            {
                enemyMovement.StateChange(EnemyState.Chase);
                moveEvent?.Invoke();
                enemyMovement.trackingTime = 10;
            }
        }
    }
}