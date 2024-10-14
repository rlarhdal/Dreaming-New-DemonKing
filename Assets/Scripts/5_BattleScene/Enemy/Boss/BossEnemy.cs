using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy, IDamagable
{
    // ���� �̺�Ʈ ����
    public Action attack1Event;
    public Action attack2Event;
    public Action attack3Event;
    public Action attack4Event;
    public Action special1Event;
    public Action special2Event;
    public Action dieEvent;
    public Action stunEvent;

    // ���� ������Ʈ
    public BossMovement movement;
    BossSkill skill;

    // Ÿ���� �Ÿ� ĳ��
    private Vector3 direction;

    void Start()
    {
        Init(Managers.Game.player.transform);
    }

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<BossMovement>();
        skill = GetComponent<BossSkill>();
    }

    // Ÿ�ٰ� �ڽ��� �Ÿ��� ��ȯ
    public Vector2 Direction()
    {
        if (target == null)
            target = Managers.Game.player.transform;
        direction = target.transform.position - transform.position;
        return direction;
    }

    // �ǰ� �� ������ �԰� �Ѵ�.
    public void ApplyDamage(float dmg)
    {
        // �Դ� �������� ü�º��� ũ�ų� ���ٸ� �װ���
        if (status.health <= dmg)
        {
            status.health = 0;

            // ����
            dieEvent?.Invoke();
            skill.StopAllCoroutines();
            movement.rigid.velocity = Vector3.zero;

            // �������� Ŭ����
            MapGenerator.instance.StageClear();
        }
        // �ƴ϶�� �������� �԰� ����
        else
        {
            if (skill.specialHit)
            {
                skill.specialCount++;
                status.health -= 1;
            }
            else
            {
                status.health -= (int)dmg;
            }
            // ���� UI���� ������ �������� ���� ������ UI�� ������Ʈ �ϰ� ��
            MapGenerator.instance.Hit(maxHealth, status.health);
        }
    }
}