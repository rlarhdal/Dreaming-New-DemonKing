using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentForce : UI_PopUp
{
    enum Texts
    {
        SoulTxt,
    }
    enum GameObjects
    {
        EquipmentSlots = 0,
    }

    enum Buttons
    {
        ExitBtn
    }

    enum RawImages
    {
        CharacterImage,
    }

    private List<Item> equipmentItems = new List<Item>();

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<RawImage>(typeof(RawImages));
        Bind<TextMeshProUGUI>(typeof(Texts));

        LoadItems();

        GetText((int)Texts.SoulTxt).text = Managers.Game.player.Inventory.soulCount.ToString();
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn);

        // choice of you
        // 1. drag and drop the texture on the prefab
        // 2. use below code to load dynamically
//        Get<RawImage>((int)RawImages.CharacterImage).texture = Managers.Resource.Load<RenderTexture>("UI/PlayerTexture");

        MakeSlots();
    }

    public void UpdateSoulTxt(PointerEventData data)
    {
        Get<TextMeshProUGUI>((int)Texts.SoulTxt).text = Managers.Game.player.Inventory.soulCount.ToString();
    }

    private void LoadItems()
    {
        // todo :: 인벤토리에 있는 무기, 외피, 반지 가져오기
        for(int i = 0; i < 3; i++)
        {
            ItemType type = (ItemType)i;
            Item item = Managers.Game.player.Inventory.GetEquippedItem(type);
            equipmentItems.Add(item);
        }
    }

    private void ExitBtn(PointerEventData data)
    {
        base.ClosePopupUI();
    }

    private void CharacterForceBtn(PointerEventData data)
    {
//        base.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_CharacterForce>();
    }

    private void MakeSlots()
    {
        GameObject slots = Get<GameObject>((int)GameObjects.EquipmentSlots);

        // 패널 초기화
        foreach (Transform child in slots.transform)
            Managers.Resource.Destroy(child.gameObject);

        for(int i = 0; i < 3; i++)
        {
            if (equipmentItems[i] == null)
                equipmentItems[i] = Managers.Data.Item.none;

            GameObject stat = Managers.UI.MakeItems<UI_EquipmentForce_item>(parent: slots.transform).gameObject;

            UI_EquipmentForce_item equipmentStatForce = stat.GetOrAddComponent<UI_EquipmentForce_item>();
            equipmentStatForce.SetInfo(equipmentItems[i]);
        }
    }
}
