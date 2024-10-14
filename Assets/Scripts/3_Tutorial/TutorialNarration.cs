using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialNarration : TutorialBase
{
    [SerializeField] private GameObject uiObj;

    private TextMeshProUGUI dialogue;

    public override void Enter()
    {
        gameObject.SetActive(true);
        dialogue = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public override void Execute(TutorialController controller)
    {
        if (gameObject.AddComponent<TypingEffect>().IsTypingEffect(dialogue.text, dialogue))
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
