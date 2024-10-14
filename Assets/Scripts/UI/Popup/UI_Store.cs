using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Store : UI_PopUp
{
    enum GameObjects
    {
        StoreSlots = 0,
    }

    enum Buttons
    {
        RerollBtn = 0,
        ExitBtn
    }

    enum Texts
    {
        SoulCnt
    }

    // �����丵 : setactive(false)�� �ٲ㼭 �� ��ũ��Ʈ���� �� ������ �ؾ���
    private List<Item> currentItems;
    private List<Item> saveItems;
    (bool isSold, Item item)[] StoreItem;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        // UI
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        UpdateSoulCnt();

        GetButton((int)Buttons.RerollBtn).gameObject.BindEvent(RerollBtn);
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn);

        // Feature
        saveItems = Managers.UI.storeSaveItems;
        StoreItem = Managers.Data.Item.itemsOnStore;

        if (saveItems != null)
        {
            currentItems = saveItems;
            //Array.ForEach(StoreItem, itemStatus => currentItems.Add(itemStatus.item));
            MakeSlots();
        }
        else
        {
            Reroll();
        }
    }

    private void ExitBtn(PointerEventData data)
    {
        PopUpExit();
    }

    private void UpdateSoulCnt()
    {
        GetText((int)Texts.SoulCnt).text = Managers.Game.player.Inventory.soulCount.ToString();
    }

    public void UpdateSoulTxt()
    {
        Get<TextMeshProUGUI>((int)Texts.SoulCnt).text = Managers.Game.player.Inventory.soulCount.ToString();
    }

    public void PopUpExit()
    {
        Managers.UI.storeSaveItems = currentItems; // ���� ������ ����Ʈ ����

        base.ClosePopupUI();
    }

    private void RerollBtn(PointerEventData data)
    {
        if (Managers.Game.player.Inventory.CheckSoul(50)) 
        {
            // todo : ��ȭ ���� �˾�
            Reroll();
            UpdateSoulCnt();
        }
    }

    /// <summary>
    /// ���� ��ư
    /// </summary>
    private void Reroll()
    {
        RefreshItems();
        MakeSlots();
    }

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    private void RefreshItems()
    {
        currentItems = Managers.Data.Item.GetRandomItems(3);
        if (Managers.UI.storeSaveItems == null)
            Managers.UI.storeSaveItems = new List<Item>();
        Managers.UI.storeSaveItems.Clear();
        currentItems.ForEach(item => Managers.UI.storeSaveItems.Add(item));
    }

    /// <summary>
    /// item slot ����
    /// </summary>
    private void MakeSlots()
    {
        GameObject slots = Get<GameObject>((int)GameObjects.StoreSlots);

        // �г� �ʱ�ȭ
        foreach (Transform child in slots.transform)
            Managers.Resource.Destroy(child.gameObject);

        for(int i = 0; i < currentItems.Count; i++)
        {
            GameObject stat = Managers.UI.MakeItems<UI_Store_Item>(parent: slots.transform).gameObject;

            UI_Store_Item storeItem = stat.GetOrAddComponent<UI_Store_Item>();
            storeItem.SetInfo(false, currentItems[i]);
        }

        //foreach (Item item in currentItems)
        //{
        //    GameObject stat = Managers.UI.MakeItems<UI_Store_Item>(parent: slots.transform).gameObject;

        //    UI_Store_Item storeItem = stat.GetOrAddComponent<UI_Store_Item>();
        //    storeItem.SetInfo(false, item);
        //}
//        foreach (var itemStatus in StoreItem)
//        {
//            GameObject stat = Managers.UI.MakeItems<UI_Store_Item>(parent: slots.transform).gameObject;
//            UI_Store_Item itemOnSale = stat.GetOrAddComponent<UI_Store_Item>();
//            itemOnSale.SetInfo(itemStatus.isSold, itemStatus.item);
//        }
    }
}
