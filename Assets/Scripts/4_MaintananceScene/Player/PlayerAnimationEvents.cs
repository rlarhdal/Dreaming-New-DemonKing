using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour // Do not remove monobehaviour due to invoking events during animation 
{
    public UnityAction attackEvents;
    public UnityAction skill1Events;
    public UnityAction dieEvents;
    Animator anim;
    PlayerStatHandler statHandler;

    public void Init()
    {
        Managers.Game.player.AnimationEvents = this;
        anim = Managers.Game.player.SpumPrefabs._anim;
        statHandler = Managers.Game.player.StatHandler;
        Managers.Game.player.GetOrAddComponent<PlayerInteraction>().dieEvent += Die;
    }

    public void UpdateParameter()
    {
//        anim.SetFloat(Animator.StringToHash("MoveSpeedMultiplier"),
//            (statHandler.GetStat<float>(StatSpecies.Speed).value * (1f+statHandler.GetStat<float>(StatSpecies.plusSpeed).value)));
        anim.SetFloat(Animator.StringToHash("AttackSpeedMultiplier"),
            (statHandler.GetStat<float>(StatSpecies.ATKRate).value * (1f+statHandler.GetStat<float>(StatSpecies.plusATKRate).value)));
        anim.SetFloat(Animator.StringToHash("SkillSpeedMultiplier"), 
            (statHandler.GetStat<float>(StatSpecies.ATKRate).value * (1f+statHandler.GetStat<float>(StatSpecies.plusATKRate).value)));
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,5);
    }

    // Attack_Normal 
    public void Attack()
    {
        attackEvents?.Invoke();
    }
    // Skill_Normal
    public void UseSkill()
    {
        skill1Events?.Invoke();
    }
    // Death
    public void Die()
    {
        dieEvents?.Invoke();
    }

    
}