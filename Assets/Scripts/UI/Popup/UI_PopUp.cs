using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : UIBase
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
        Managers.UI.ClosePopupUI(this);
    }
}
