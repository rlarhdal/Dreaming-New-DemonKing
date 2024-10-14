using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MiniMap : UI_PopUp
{
    enum Buttons
    {
        ExitBtn,
    }

    void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Time.timeScale = 0f;
        Bind<Button>(typeof(Buttons));
       
        
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(Exit);
    }
    private void Exit(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
        Time.timeScale = 1f;
    }
}
