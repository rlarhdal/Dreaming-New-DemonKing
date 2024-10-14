using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BehaviourPotion : BehaviourInput
{
    PlayerStatHandler statHandler;
    Stat<float> statHeal;
    float healAmount; // 0 ~ 1(100%) percentage

    public int PotionCount { get; set; }
    void Start()
    {
//        Init();
    }

    public override void Init()
    {
        PotionCount = 3;
        statHandler = Managers.Game.player.StatHandler;
        statHeal = statHandler.GetStat<float>(StatSpecies.HealAmount);
        input = new InputPotion();

        PlayerInputHandler handler = Managers.Game.player.InputHandler;
        handler.data[(int)ActionType.Potion].Subscribers[InputStatus.Started] += Started;
        handler.SubscribeToggle();
    }

    public void DrinkPotion()
    {
        PotionCount--;
        // Issue that HP doesn't recover but coolTime is begun should be fixed
//        if (isCoolDown)
//            return;
        healAmount = statHeal.value; // 10 %

        int cHP, mHP, pHP;
        cHP = statHandler.GetStat<int>(StatSpecies.HP).value;
        mHP = statHandler.GetStat<int>(StatSpecies.MaxHP).value;
        pHP = statHandler.GetStat<int>(StatSpecies.plusHP).value;
        statHandler.GetStat<int>(StatSpecies.HP).value = Mathf.Min(
            cHP + (int)(healAmount * (mHP + pHP)),
            mHP+pHP
        );
        
        Managers.Game.player.Interaction.UpdateHPBar();
        
    }

    protected override void Started(InputAction.CallbackContext context)
    {
    }

    protected override void Performed(InputAction.CallbackContext context)
    {
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
    }
}