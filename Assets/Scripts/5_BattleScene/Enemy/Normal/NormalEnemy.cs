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

    // �÷��̾ �׾��� �� �浹 ���θ� �ٽ� �����ϰ� ��
    private void OnDisable()
    {
        rigid.simulated = true;
    }

    // �̺�Ʈ ������ �߰�
    private void Start()
    {
        hitEvent += AttackTimeClear;
    }

    private void Update()
    {
        // ���� ������ �ð��� ������ ������ �����ϰ� �����ϰ�
        // ���� ���ð��� �ʱ�ȭ�Ѵ�.
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

    // ���� ���� ���ð� �ʱ�ȭ
    private void AttackTimeClear()
    {
        attackTime = status.attackSpeed * 2;
    }

    // �ǰݽ� �������� �԰� �Ѵ�.
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