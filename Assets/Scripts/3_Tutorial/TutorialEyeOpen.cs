using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEyeOpen : TutorialBase
{
    [SerializeField] private EyeBlink eyeBlink;
    private bool isCompleted = false;

    public override void Enter()
    {
        eyeBlink.EyeOpen(OnAfterFadeEffect);
    }

    private void OnAfterFadeEffect()
    {
        isCompleted = true;
    }

    public override void Execute(TutorialController controller)
    {
        if (isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        eyeBlink.gameObject.SetActive(false);
    }
}
