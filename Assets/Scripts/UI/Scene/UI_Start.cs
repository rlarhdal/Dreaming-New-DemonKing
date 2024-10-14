using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Start : UI_Scene
{
    enum Texts
    {
        StartMessage = 0,
    }
    enum Images
    {
        BackBoard = 0,
        Option
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void OnDestroy()
    {
        Managers.UI.UIlist.Remove(this);
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetImage((int)Images.BackBoard).gameObject.BindEvent(BackBoard);
        GetImage((int)Images.Option).gameObject.BindEvent(Option);
    }

    private void Option(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
        Managers.UI.ShowPopupUI<UI_Options>();
    }

    private void BackBoard(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.StartEffect]);

        Managers.Scene.LoadScene(SceneType.MaintenanceScene);
    }

}
