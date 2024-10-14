using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Maintenance : UI_Scene
{
    enum Texts
    {
        LevelCnt,
    }

    enum Images
    {
        LevelBar = 0,
        CharacterIcon
    }

    enum Buttons
    {
        PlayerPortrait,
    }

    private void Start()
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

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        
        GetButton((int)Buttons.PlayerPortrait).gameObject.BindEvent( data => Managers.UI.ShowPopupUI<UI_CharacterInfo>());

        UpdatePlayerStatUI();
    }

    void EquipStick(PointerEventData obj)
    {
        Item item = Managers.Resource.Load<Item>("ItemData/defaultWeapon");
        Managers.Game.player.Inventory.Equip(item);
    }


    public void UpdatePlayerStatUI()
    {
        PlayerStatHandler statHandler = Managers.Game.player.StatHandler;
        int lv =     statHandler.GetStat<int>(StatSpecies.LV).value;
        int curExp = statHandler.GetStat<int>(StatSpecies.Exp).value;
        int maxExp = statHandler.GetStat<int>(StatSpecies.MaxExp).value;

        Get<TextMeshProUGUI>((int)Texts.LevelCnt).text = $"{lv}";
        GetImage((int)Images.LevelBar).fillAmount = (float)curExp / maxExp;
    }
}