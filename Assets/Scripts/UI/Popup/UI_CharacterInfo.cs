using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterInfo : UI_PopUp
{
    PlayerStatHandler statHandler;
    PlayerInventory inventory;
    Item[] items;

    enum Texts
    {
        HPAmount,
        AtkAmount,
        AtkrateAmount,
        SpeedAmount,
        HealAmount,

        EquipmentDescription,
        SoulTxt,
    }

    enum Buttons
    {
        ExitBtn,
    }

    enum Images
    {
        WeaponItem,
        ArmorItem,
        CloakItem
    }

    enum GameObjects
    {
        EquipmentDesc
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        statHandler = Managers.Game.player.StatHandler;
        inventory = Managers.Game.player.Inventory;
        items = new Item[3];
        
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        UpdateStatus();
        UpdateWeapons();
        GetGameObject((int)GameObjects.EquipmentDesc).SetActive(false);

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(data => ClosePopupUI());
        GetImage((int)Images.WeaponItem).gameObject.BindEvent(WeaponDescription);
        GetImage((int)Images.ArmorItem).gameObject.BindEvent(ArmorDescription);
        GetImage((int)Images.CloakItem).gameObject.BindEvent(CloakDescription);
    }

    private void CloakDescription(PointerEventData data)
    {
        ObjSetActive();

        if (items[2].itemStat1 == StatSpecies.plusATKRate)
        {
            GetText((int)Texts.EquipmentDescription).text = string.Format("{0} (+{1}) \n공격속도 : {2}",
            items[2].itemName,
            items[2].enchantLvl,
            items[2].statValue1
            );
        }
        else
        {
            GetText((int)Texts.EquipmentDescription).text = string.Format("{0} (+{1}) \n이동속도 : {2}",
            items[2].itemName,
            items[2].enchantLvl,
            items[2].statValue1
            );
        }
        
    }

    private void ArmorDescription(PointerEventData data)
    {
        ObjSetActive();

        GetText((int)Texts.EquipmentDescription).text = string.Format("{0} (+{1}) \n추가 체력 : {2} \n회복량 : {3}",
            items[1].itemName,
            items[1].enchantLvl,
            items[1].statValue1,
            items[1].statValue2
            );
    }

    private void WeaponDescription(PointerEventData data)
    {
        ObjSetActive();

        GetText((int)Texts.EquipmentDescription).text = string.Format("{0} (+{1}) \n공격력 : {2} \n공격속도 : {3}",
            items[0].itemName,
            items[0].enchantLvl,
            items[0].statValue1,
            items[0].statValue2
            );
    }

    private void ObjSetActive()
    {
        //GetGameObject((int)GameObjects.EquipmentDesc).SetActive(!GetGameObject((int)GameObjects.EquipmentDesc).activeSelf);
        if (GetGameObject((int)GameObjects.EquipmentDesc).activeSelf == false)
        {
            GetGameObject((int)GameObjects.EquipmentDesc).SetActive(true);
        }
    }

    private void UpdateWeapons()
    {
        GetItem();

        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                items[i] = Managers.Data.Item.none;
                GetImage(i).sprite = items[i].itemImage[0];
            }
            else
            {
                Image img = GetImage(i);
                Util.ResizeImage(img, items[i]);
            }
        }
    }

    private void GetItem()
    {
        items[0] = inventory.GetEquippedItem(ItemType.Weapon);
        items[1] = inventory.GetEquippedItem(ItemType.Armor);
        items[2] = inventory.GetEquippedItem(ItemType.Accessory);
    }

    void UpdateStatus()
    {
        // SoulTxt
        Get<TextMeshProUGUI>((int)Texts.SoulTxt).text = inventory.soulCount.ToString();

        // HP
        Get<TextMeshProUGUI>((int)Texts.HPAmount).text =
            statHandler.GetStat<int>(StatSpecies.plusHP).value != 0 ?
                string.Format("{0} (+{1})",
                    statHandler.GetStat<int>(StatSpecies.MaxHP).value,
                    statHandler.GetStat<int>(StatSpecies.plusHP).value
                    ):
                string.Format("{0}",
                    statHandler.GetStat<int>(StatSpecies.HP).value
                    );
        
        // Power
        Get<TextMeshProUGUI>((int)Texts.AtkAmount).text = string.Format("{0} (+{1})",
            statHandler.GetStat<int>(StatSpecies.ATKPower).value,
            statHandler.GetStat<int>(StatSpecies.plusATKPower).value
            );
        
        // Attack rate
        Get<TextMeshProUGUI>((int)Texts.AtkrateAmount).text = string.Format("{0}\u00d7(1+{1})",
            statHandler.GetStat<float>(StatSpecies.ATKRate).value,
            statHandler.GetStat<float>(StatSpecies.plusATKRate).value
        );
        
        // Speed
        Get<TextMeshProUGUI>((int)Texts.SpeedAmount).text = string.Format("{0}\u00d7(1+{1})",
            statHandler.GetStat<float>(StatSpecies.Speed).value,
            statHandler.GetStat<float>(StatSpecies.plusSpeed).value
        );
        
        // Heal Amount
        Get<TextMeshProUGUI>((int)Texts.HealAmount).text =
            (statHandler.GetStat<float>(StatSpecies.HealAmount).value * 100).ToString("N0");
    }
}