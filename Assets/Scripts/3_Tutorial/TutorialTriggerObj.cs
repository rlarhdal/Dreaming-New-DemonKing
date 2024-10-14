using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerObj : TutorialBase
{
    public override void Enter()
    {
        Managers.Game.player.canMove = true;
    }

    public override void Execute(TutorialController controller)
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
