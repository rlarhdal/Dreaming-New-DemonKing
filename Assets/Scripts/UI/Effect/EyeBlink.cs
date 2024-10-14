using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EyeBlink : MonoBehaviour
{
    [System.Serializable]
    private class EyeBlinkEvent : UnityEvent { }
    private EyeBlinkEvent onEyeBlinkEvent = new EyeBlinkEvent();

    private Animator eyeBlink;

    private void Awake()
    {
        eyeBlink = GetComponent<Animator>();
    }

    public void Eyeblinking(UnityAction action)
    {
        onEyeBlinkEvent.AddListener(action);

        //eyeBlink.SetTrigger("Blinking");
        eyeBlink.SetBool("Blink", true);
    }

    public void EyeOpen(UnityAction action)
    {
        onEyeBlinkEvent.AddListener(action);

        eyeBlink.SetBool("Open", true);
        //eyeBlink.SetTrigger("Opening");
    }

    public void OnEndTutorial()
    {
        onEyeBlinkEvent.Invoke();
    }

    public void OnEndAnimation(string animationBool)
    {
        eyeBlink.SetBool(animationBool, false);
        onEyeBlinkEvent.Invoke();
    }
}
