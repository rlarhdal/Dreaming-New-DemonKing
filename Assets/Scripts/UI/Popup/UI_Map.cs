using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Map : UI_PopUp
{
    enum Buttons
    {
        ExitBtn
    }

    enum Images
    {
        Home,
        Village,
        Town,
        City
    }

    private void Start()
    {
        Init();

        //Managers.Camera.uiCam.gameObject.SetActive(true);

        //canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //canvas.worldCamera = Managers.Camera.uiCam;
        
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn);
        GetImage((int)Images.Home).gameObject.BindEvent(Home);
        GetImage((int)Images.Village).gameObject.BindEvent(Village);
//        GetImage((int)Images.Town).gameObject.BindEvent(Town);
//        GetImage((int)Images.City).gameObject.BindEvent(City);
    }

    private void City(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.StageSelected]);
        Managers.Scene.LoadScene(SceneType.BattleScene);
    }

    private void Town(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.StageSelected]);
        Managers.Scene.LoadScene(SceneType.BattleScene);
    }

    private void Village(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.StageSelected]);
        Managers.Scene.LoadScene(SceneType.BattleScene);
    }

    private void Home(PointerEventData data)
    {
        base.ClosePopupUI();
    }

    private void ExitBtn(PointerEventData data)
    {
        base.ClosePopupUI();
    }
}
