using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentForce_item : UI_PopUp
{
    enum Texts
    {
        ItemName = 0,
//        ItemStatChange1,
//        ItemStatChange2,
        EnchantPercentage,
        DestroyPercentage,
        Cost
    }

    enum Buttons
    {
        ForceBtn = 0,
    }

    enum Images
    {
        ItemImg = 0,
        Frame,
    }

    private void Start()
    {
        Init();
    }

    string itemText, forceBtn;
    Sprite itemImg;

    (float x, float y) gap;
    public Item item;
    PlayerInventory inventory;

    // Frame Sprite
    Sprite frameSprite;


    public override void Init()
    {
        base.Init();

        gap = (10f, 20);
        inventory = Managers.Game.player.Inventory;
        
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        //todo::?? ?????????? ?? ????
        if ( item == null)
            item =  Managers.Data.Item.none;
        GetText((int)Texts.ItemName).GetComponent<TextMeshProUGUI>().text = item.itemName;
        GetImage((int)Images.ItemImg).sprite = item.itemImage[0];
        if(item.itemType != ItemType.None)
            GetButton((int)Buttons.ForceBtn).gameObject.BindEvent(Enchant);
        GetButton((int)Buttons.ForceBtn).gameObject.BindEvent(Managers.UI.FindPopUp<UI_EquipmentForce>().UpdateSoulTxt);
        
        UpdateFrame();
        UpdateDescription();
    }

    void Enchant(PointerEventData obj)
    {
        if (item == null || inventory == null) return;
        if (item.rarity == ItemRarity.None) return;
        if (!inventory.CheckSoul(100 * (item.enchantLvl + 1))) return;
        float successRate = Mathf.Max(0f, 100f - (item.enchantLvl * 5f));
        float failRate = 90f - successRate;
        float revertRate = item.enchantLvl >= 10 ? Mathf.Min(100f, (item.enchantLvl - 9) * 5f) : 0f;
        float randomValue = Random.Range(0f, 100f);
        if (randomValue < successRate)
        {
            item.enchantLvl++;
//            item.statValue1 += item.enchantFactor1;
//            item.statValue2 += item.enchantFactor2;
        }
        else if (randomValue < successRate + failRate)
        {
        }
        else if (randomValue < successRate + failRate + revertRate)
        {
            item.enchantLvl = 1;
//            item.statValue1 = item.statValue1 + item.enchantFactor1;
//            item.statValue2 = item.statValue2 + item.enchantFactor2;
        }
        UpdateDescription();
        inventory.UpdateStatSum();
        Managers.Data.SaveGame();
    }

    public void SetInfo(Item _item)
    {
        item = _item;
    }

    public void UpdateFrame()
    {
        LoadFrame();

        Vector2 sizeDelta = GetImage((int)Images.ItemImg).gameObject.GetComponent<RectTransform>().sizeDelta;
        float denominator = item.itemImage[0].rect.width > item.itemImage[0].rect.height? item.itemImage[0].rect.width : item.itemImage[0].rect.height;
        sizeDelta.x = sizeDelta.x * item.itemImage[0].rect.width / denominator - 2*gap.x;
        sizeDelta.y = sizeDelta.y * item.itemImage[0].rect.height / denominator - 2* gap.y;
        GetImage((int)Images.ItemImg).GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }

    private void LoadFrame()
    {
        frameSprite = Managers.Resource.Load<Sprite>($"Images/Frame/{item.rarity}");
        GetImage((int)Images.Frame).sprite = frameSprite;
    }

    void UpdateDescription()
    {
        if (item.itemType == ItemType.None)
            return;
        TextMeshProUGUI name = Get<TextMeshProUGUI>((int)Texts.ItemName);
//        TextMeshProUGUI statChange1 = Get<TextMeshProUGUI>((int)Texts.ItemStatChange1);
//        TextMeshProUGUI statChange2 = Get<TextMeshProUGUI>((int)Texts.ItemStatChange2);
        TextMeshProUGUI enchantPercentage = Get<TextMeshProUGUI>((int)Texts.EnchantPercentage);
        TextMeshProUGUI destroyPercentage = Get<TextMeshProUGUI>((int)Texts.DestroyPercentage);
        TextMeshProUGUI cost = Get<TextMeshProUGUI>((int)Texts.Cost);

        name.text = item.enchantLvl == 0 ? item.itemName : $"{item.itemName} ( +{item.enchantLvl} )";
//        statChange1.text = $"{item.itemStat1.ToString()} : {item.statValue1} > {item.statValue1} + {item.enchantFactor1}";
//        statChange2.text = $"{item.itemStat2.ToString()} : {item.statValue2} > {item.statValue2} + {item.enchantFactor2}";
        enchantPercentage.text = $"강화확률 : {100 - 10 * item.enchantLvl} %";
        destroyPercentage.text = item.enchantLvl >= 10 ? $"파괴확률 : {Mathf.Min(100f, (item.enchantLvl - 9) * 5f)} %" : "파괴확률 : 0 %";
        cost.text = $"소모 소울 : {100 * (item.enchantLvl + 1)}";
    }
}
