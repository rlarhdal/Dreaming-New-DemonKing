using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckUI : TutorialBase
{
    [SerializeField] private GameObject gameObject;

    public override void Enter()
    {

    }

    public override void Execute(TutorialController controller)
    {
        if (Managers.UI.FindPopUp<UI_Store>() != null)
        {
            gameObject.SetActive(false);
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
