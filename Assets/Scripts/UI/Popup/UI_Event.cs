using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Event : UI_PopUp
{
    enum Buttons
    {
        BackToStart
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.BackToStart).gameObject.BindEvent(BackToStart);
    }

    private void BackToStart(PointerEventData data)
    {
        Managers.Scene.LoadScene(SceneType.StartScene);
    }
}
