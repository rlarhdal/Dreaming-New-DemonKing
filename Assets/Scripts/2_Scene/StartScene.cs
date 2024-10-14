using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    AudioClips clips;

    protected override void Init()
    {
        base.Init();

        SceneType = SceneType.StartScene;

        Managers.UI.ShowSceneUI<UI_Start>();
    }

    private void Start()
    {
        Managers.Sound.VolumeInit();
        Managers.Sound.PlaySound(Managers.Sound.clipBGM[(int)BGMClip.Intro]);
        if (GameObject.Find("@Managers") == null)
        {
            new GameObject(name: "@Managers").AddComponent<Managers>();
            Application.targetFrameRate = 60;
        }
    }


    public override void Clear()
    {

    }
}
