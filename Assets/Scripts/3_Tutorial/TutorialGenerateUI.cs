using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TutorialGenerateUI : TutorialBase
{
    [SerializeField] private GameObject uiObj;
    private TutorialController tutorialController;

    public override void Enter()
    {

    }

    public override void Execute(TutorialController controller)
    {
        tutorialController = controller;

        bool isCompleted = GenerateUI();

        if (isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }

    public bool GenerateUI()
    {
        if (tutorialController.uis.Count != 0)
        {
            GameObject obj = tutorialController.uis.Pop();

            // obj�� ���� ������Ʈ�� stack�� ������ ���ְ� ����
            if (obj.name == uiObj.name)
            {
                obj.SetActive(false);
                tutorialController.uis.Clear();

                return true;
            }

            obj.SetActive(false);

            tutorialController.uis.Clear();
        }

        uiObj.SetActive(true);
        tutorialController.uis.Push(uiObj);

        return true;
    }
}
