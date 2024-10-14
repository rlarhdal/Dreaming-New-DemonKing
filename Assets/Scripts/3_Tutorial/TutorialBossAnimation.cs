using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossAnimation : TutorialBase
{
    [SerializeField] private EventBossAnim eventBossAnim;

    public override void Enter()
    {

    }

    public override void Execute(TutorialController controller)
    {
        eventBossAnim.BossTalk();

        if (eventBossAnim.isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
