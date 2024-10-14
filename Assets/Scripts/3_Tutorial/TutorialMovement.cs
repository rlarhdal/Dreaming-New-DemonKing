using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovement : TutorialBase
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector3 endPosition;
    private bool isCompleted = false;

    public override void Enter()
    {
        StartCoroutine(nameof(Movement));
    }

    public override void Execute(TutorialController controller)
    {
        if(isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }

    private IEnumerator Movement()
    {
        float current = 0;
        float percent = 0;
        float moveTime = 1f;
        Vector3 start = rectTransform.anchoredPosition;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            rectTransform.anchoredPosition = Vector3.Lerp(start, endPosition, percent);

            yield return null;
        }

        isCompleted = true;
    }
}
