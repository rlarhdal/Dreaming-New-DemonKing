using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_HUD : UI_Scene
{
    enum Buttons
    {
        Option = 0,
        MiniMap,
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

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Option).gameObject.BindEvent(Option);
        if(SceneManager.GetActiveScene().name=="MaintenanceScene")
        {
            GetButton((int)Buttons.MiniMap).gameObject.BindEvent(MiniMap);
        }
    }

    void Damage(PointerEventData obj)
    {
//        Debug.Log("Damage Btn pressed");
        Managers.Game.player.Interaction.ApplyDamage(100);
        int curHP = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.HP).value;
        int maxHP = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.MaxHP).value +
                    Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.plusHP).value;
    }

    private void Option(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
        Managers.UI.ShowPopupUI<UI_Options>();
    }

    private void MiniMap(PointerEventData data)
    {
        Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Map]);
        Managers.UI.ShowPopupUI<UI_MiniMap>();
        
    }


}
