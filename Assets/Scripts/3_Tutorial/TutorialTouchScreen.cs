using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTouchScreen : TutorialBase
{
    public override void Enter()
    {

    }

    public override void Execute(TutorialController controller)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
            controller.SetNextTutorial();
        }

//#if (UNITY_EDITOR)
//        if (Input.GetMouseButtonDown(0))
//        {
//            Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
//            controller.SetNextTutorial();
//        }
//#elif (UNITY_ANDROID)

//                if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//                {
//                        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
//                        controller.SetNextTutorial();
//                }
//#endif()
    }

    public override void Exit()
    {

    }
}
