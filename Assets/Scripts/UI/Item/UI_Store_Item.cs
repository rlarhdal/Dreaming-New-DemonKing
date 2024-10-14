using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Store_Item : UI_PopUp
{
    enum Texts
    {
        ItemName = 0,
        CostText,
        DescriptionTxt,
    }

    enum Buttons
    {
        UI_Store_Item = 0,
    }

    enum Images
    {
        ItemImg = 0,
        Frame
    }



    // UI
    string itemName, forceBtn;
    int cost;
    Sprite itemImg;

    Item item;

    (float x, float y) gap;
    bool isSoldOut;

    // Frame Sprite
    Sprite frameSprite;

    UI_Store store;

    void Start()
    {
        Init();
        store = Managers.UI.FindPopUp<UI_Store>();
    }

    public override void Init()
    {
        base.Init();

        gap = (10f, 20f);
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetImage((int)Images.ItemImg).sprite = item.itemImage[0];
        GetText((int)Texts.ItemName).text = item.itemName;
        GetText((int)Texts.CostText).text = $"{cost}";

        GetButton((int)Buttons.UI_Store_Item).gameObject.BindEvent(BuyItem);

        UpdateFrame();
        UpdateDesc();
    }

    void UpdateFrame()
    {
        if (item.rarity == ItemRarity.None) return;

        // ?????? ??????
        LoadFrame();
        //GetImage((int)Images.Frame).color = Util.BGcolors[(int)item.rarity];

        // ?????? ?????? ?????? ????
        Vector2 sizeDelta = GetImage((int)Images.ItemImg).gameObject.GetComponent<RectTransform>().sizeDelta;
        float denominator = item.itemImage[0].rect.width > item.itemImage[0].rect.height? item.itemImage[0].rect.width : item.itemImage[0].rect.height;
        sizeDelta.x = sizeDelta.x * item.itemImage[0].rect.width / denominator - 2*gap.x;
        sizeDelta.y = sizeDelta.y * item.itemImage[0].rect.height / denominator - 2* gap.y;
        GetImage((int)Images.ItemImg).GetComponent<RectTransform>().sizeDelta = sizeDelta;

        // ???????? ????
        UpdateItemStatus(isSoldOut);
    }

    private void LoadFrame()
    {
        frameSprite = Managers.Resource.Load<Sprite>($"Images/Frame/{item.rarity}");
        GetImage((int)Images.Frame).sprite = frameSprite;
    }

    private void BuyItem(PointerEventData data)
    {
        UI_StoreBuy.SetInfo(item);
        UI_StoreBuy storeBuy = Managers.UI.ShowPopupUI<UI_StoreBuy>();
//        storeBuy.SetInfo(item);
//    {
//        GameObject go = Managers.UI.MakeItems<UI_StoreBuy>(transform).gameObject;
//        UI_StoreBuy storeBuy = Managers.UI.MakeItems<UI_StoreBuy>(parent: transform).gameObject.GetOrAddComponent<UI_StoreBuy>();
//        storeBuy.SetInfo(item);
//        
//        Managers.UI.ShowPopupUI<UI_StoreBuy>();
//        if (item != null)
//        {
//            if (!Managers.Game.player.Inventory.CheckSoul(cost)) return;
//
//            Managers.Game.player.Inventory?.ReplaceItem(item);
//            isSoldOut = true;
//            UpdateItemStatus(isSoldOut);
//
//            store.UpdateSoulTxt();
//        }
//        else
//        {
//            Debug.LogWarning("?????????? ???????? ????????.");
//        }
    }

    void UpdateItemStatus(bool isSoldOut)
    {
        if (this.isSoldOut == true)
        {
            Color color = GetImage((int)Images.ItemImg).color;
            color.a = 0;
            GetImage((int)Images.ItemImg).color = color;
            Get<TextMeshProUGUI>((int)Texts.ItemName).text = "Sold";
        }
    }

    void UpdateDesc()
    {
        float value1, value2;

        Get<TextMeshProUGUI>((int)Texts.DescriptionTxt).text = item.description;
//        switch (item.itemType)
//        {
//            case ItemType.Weapon:
//                value1 = item.statValue1;// - Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.plusATKPower).value;
//                value2 = item.statValue2;// - Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.plusATKRate).value;
//                GetText((int)Texts.Stat1Text).text = $"공격력 + {value1}";
//                GetText((int)Texts.Stat2Text).text = $"공속 + {value2}";
//                break;
//            case ItemType.Armor:
//                value1 = item.statValue1;// - Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.plusHP).value;
//                value2 = item.statValue2;// - Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.HealAmount).value;
//                GetText((int)Texts.Stat1Text).text = $"체력 + {value1}";
//                GetText((int)Texts.Stat2Text).text = $"회복량 + {value2}";
//                break;
//            case ItemType.Accessory:
//                value1 = item.statValue1;// - Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.plusATKRate).value;
//                value2 = item.statValue2;// - Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.plusSpeed).value;
//                if(value1 != 0)
//                    GetText((int)Texts.Stat1Text).text = $"공속 + {value1}";
//                else if(value2 != 0)
//                    GetText((int)Texts.Stat1Text).text = $"이속 + {value2}";
//                
//                GetText((int)Texts.Stat2Text).text = "";
//                
//                break;
//        }
    }


    public void SetInfo(bool isSoldOut, Item _item)
    {
        item = _item;
        //this.isSoldOut = isSoldOut;


        itemImg = item.itemImage[0];
        itemName = item.itemName;
        cost = item.soulCost;
        
    }
}
