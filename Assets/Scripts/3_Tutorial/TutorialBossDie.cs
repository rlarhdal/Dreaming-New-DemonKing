
using UnityEngine;

public class TutorialBossDie : TutorialBase
{
    [SerializeField] private EventBossAnim eventBossAnim;

    public override void Enter()
    {

    }

    public override void Execute(TutorialController controller)
    {
        eventBossAnim.BossDie();

        if (eventBossAnim.isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
