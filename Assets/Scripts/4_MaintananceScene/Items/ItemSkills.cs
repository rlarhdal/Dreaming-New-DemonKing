using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSkills
{
    // common
    PlayerStatHandler statHandler;
    PlayerAnimationEvents animationEvents;
    Animator animator;
    Player player;
    
    // assassinate
    Coroutine assassinateCoroutine;
    WaitUntil hideUntilAttackOrWait;
    float counter;
    Material materialForHideEffect;
    StateBase stateHide;
    BehaviourAttack behaviourAttack;

    // sword aura
    Transform parentOfProjectiles;
    GameObject skillObjectOrigin;
    GameObject skillObject;
    WaitForSeconds shootDelay;
    WaitForSeconds animDelay;
    public ItemSkills()
    {
        // Common
        player = Managers.Game.player;
        animationEvents = player.AnimationEvents;
//        animator = player.GetComponentInChildren<Animator>();
        statHandler = player.StatHandler;
        
        // Assassinate
        materialForHideEffect = Managers.Game.player.mat;
        hideUntilAttackOrWait = new WaitUntil(() => animator.GetBool("AttackCoolDown") || counter > 3f);
        stateHide = Managers.Game.player.StateHandler.GetState(ActionType.Hide);
        behaviourAttack = player.GetOrAddComponent<BehaviourAttack>();
        
        // SwordAura
        skillObjectOrigin = Managers.Resource.Load<GameObject>("Projectile/SwordAura");
        Managers.Pool.CreatePool(skillObjectOrigin, 20);
        parentOfProjectiles = new GameObject(name: "Projectiles").transform;
        shootDelay = new WaitForSeconds(.2f);
        animDelay = new WaitForSeconds(.2f);
        
        // Smite
    }
    public void Init()
    {
        player = Managers.Game.player;
        animationEvents = player.AnimationEvents;
        animator = player.GetComponentInChildren<Animator>();
        statHandler = Managers.Game.player.StatHandler;
    }

    #region  Assassinate
    public void Assassinate(ItemRarity rarity)
    {
        Init();
        behaviourAttack.multiplier = 1.5f + 0.5f*(int)rarity;
        if(assassinateCoroutine == null)
            assassinateCoroutine =  player.StartCoroutine(HideAndAttack());
    }

    IEnumerator HideAndAttack()
    {
        counter = 0f;
        if(materialForHideEffect == null)
            materialForHideEffect = Managers.Game.player.mat;
        player.StateHandler.stateMachine.ChangeState(stateHide);
        player.StartCoroutine(HideTimeCounter());
        if (animationEvents == null)
            animationEvents = player.AnimationEvents;
        yield return hideUntilAttackOrWait;
        if (counter > 3f)
            behaviourAttack.multiplier = 1f;
        player.StateHandler.stateMachine.ChangeState(stateHide,true);

        assassinateCoroutine = null;
    }


    IEnumerator HideTimeCounter()
    {
        while (counter < 5f)
        {
            counter += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region SwordAura
    public void SwordAura(ItemRarity rarity)
    {
        Init();
        player.StartCoroutine(ShootProjectiles(1+(int)rarity));
    }

    IEnumerator ShootProjectiles(int num)
    {
        var targets = Physics2D.CircleCastAll(player.transform.position, 10, Vector2.zero, 0, LayerMask.GetMask("Enemy"));
        for (int cnt = 0; cnt < num; cnt++)
        {
            foreach (var target in targets)
            {
                if (target.collider.TryGetComponent(out Transform transform))
                {
                    Managers.Game.player.SpumPrefabs._anim.SetBool("AttackCoolDown",true);
                    yield return animDelay;
                    Managers.Game.player.SpumPrefabs._anim.SetBool("AttackCoolDown",false);
                    skillObject = Managers.Pool.Pop(skillObjectOrigin, parentOfProjectiles).gameObject;
                    skillObject.transform.position = player.transform.position;
                    Vector2 direction = (target.collider.transform.position - player.transform.position).normalized;
                    
                    ProjectileController projController = skillObject.GetComponent<ProjectileController>();
                    projController.direction = direction;
                    projController.targetTag = Tags.Enemy;
                    projController.Rotate();
                    yield return shootDelay; 
                    break;
                }
            }
        }
        // attackEvent invoke
        
        // delay
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region Smite

    public void Smite(ItemRarity rarity)
    {
        Init();
        player.SpumPrefabs._anim.SetTrigger("Skill");
        var hits = Physics2D.CircleCastAll(player.transform.position, 2f, Vector2.zero, 0, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IDamagable target))
            {
                float atk = statHandler.GetStat<int>(StatSpecies.ATKPower).value +
                            statHandler.GetStat<int>(StatSpecies.plusATKPower).value;
                float skillMultiplier = 2 + 0.5f*(int)rarity;
                target.ApplyDamage(skillMultiplier*atk);
                Debug.Log(skillMultiplier*atk);
            }
        }
    }
    #endregion
}