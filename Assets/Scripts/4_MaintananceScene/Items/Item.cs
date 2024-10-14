using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum ItemType { Weapon, Armor, Accessory, None }
public enum ArmorPart{Body,Left,Right}
public enum ItemRarity { Normal, Rare, Epic, Legendary, None }

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite[] itemImage;
    public string description;
    public ItemType itemType;
    public ItemRarity rarity;
    public int soulCost;

    public int enchantLvl;
    
    public StatSpecies itemStat1;
    public float statValue1;
    public float enchantFactor1;
    
    public StatSpecies itemStat2;
    public float statValue2;
    public float enchantFactor2;

    public Sprite attackIcon;
    public Sprite skillIcon;
    public UnityAction<ItemRarity> skill;
    public float skillCoolTime;

    bool isSell = false;

    public static Item Clone(Item _item)
    {
        Item item = new Item();
        item.itemID = _item.itemID;
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.description = _item.description;
        item.itemType = _item.itemType;
        item.rarity = _item.rarity;
        item.soulCost = _item.soulCost;
        item.enchantLvl = _item.enchantLvl;
        item.itemStat1 = _item.itemStat1;
        item.statValue1 = _item.statValue1;
        item.enchantFactor1 = _item.enchantFactor1;
        item.itemStat2 = _item.itemStat2;
        item.statValue2 = _item.statValue2;
        item.enchantFactor2 = _item.enchantFactor2;
        item.skill = _item.skill;
        item.skillIcon = _item.skillIcon;
        item.attackIcon = _item.attackIcon;
        item.skillCoolTime = _item.skillCoolTime;

        return item;
    }

    public void UseSkill()
    {
        skill?.Invoke(rarity);
    }
}

public enum ItemStat
{
    None,
    DMG,
    rate,
    Def,
    HP,
    AttackSpeed,
    MoveSpeed,
}

