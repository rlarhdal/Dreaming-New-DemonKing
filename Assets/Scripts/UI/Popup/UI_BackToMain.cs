using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BackToMain : UI_PopUp
{
    enum Buttons
    {
        ExitBtn = 0,
        StayBtn
    }

    enum Images
    {
        UI_BackToMain = 0,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn);
        GetButton((int)Buttons.StayBtn).gameObject.BindEvent(StayBtn);

        GetButton((int)Images.UI_BackToMain).gameObject.BindEvent(StayBtn);
    }

    private void StayBtn(PointerEventData data)
    {
        base.ClosePopupUI();
    }

    private void ExitBtn(PointerEventData data)
    {
        // todo : 필요 데이터 저장 로직 작성

        Managers.Data.GiveSouls(MapGenerator.instance.GetSoul());
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
        Managers.Scene.LoadScene(SceneType.MaintenanceScene);
    }
}
