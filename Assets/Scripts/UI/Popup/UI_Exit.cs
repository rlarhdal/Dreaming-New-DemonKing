using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Exit : UI_PopUp
{
    enum Buttons
    {
        Exit,
        Stay
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.Exit).gameObject.BindEvent(Exit);
        GetButton((int)Buttons.Stay).gameObject.BindEvent(Stay);
    }

    private void Exit(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
        // 게임 종료 로직
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Stay(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
        // 팝업 닫기
        Managers.UI.ClosePopupUI(this);
    }
}
