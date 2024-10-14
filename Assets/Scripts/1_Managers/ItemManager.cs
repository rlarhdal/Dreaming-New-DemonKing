using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager
{
    List<Item> items;
    ItemSkills itemSkills;
    public Item none;
    public Item weaponStick;

    public (bool isSold, Item item)[] itemsOnStore;

    public ItemManager()
    {
        itemSkills = new ItemSkills();
        LoadItems();
        itemsOnStore = new (bool isSold, Item item)[3];
    }

    void LoadItems()
    {
        items = new List<Item>();
        Item[] accessories = Resources.LoadAll<Item>("ItemData/Accessory");
        Item[] armors = Resources.LoadAll<Item>("ItemData/Armor");
        Item[] weapons = Resources.LoadAll<Item>("ItemData/Weapon");
        Array.ForEach(accessories, element => items.Add(element));
        Array.ForEach(armors,element => items.Add(element));
        Array.ForEach(weapons,element =>
        {
            SetSkills(element);
            items.Add(element);
        });
        none = Resources.Load<Item>("ItemData/None");
        weaponStick = Managers.Resource.Load<Item>("ItemData/defaultWeapon");
    }

    // refactoring essential
    void SetSkills(Item item)
    {
        if (item.itemType != ItemType.Weapon)
            return;
        switch ((item.itemID / 10) % 10)
        {
            case 1:
                item.skill += itemSkills.Assassinate;
                item.skillCoolTime = 5f;
                break;
            case 2:
                item.skill += itemSkills.SwordAura;
                item.skillCoolTime = 2f;
                break;
            case 3:
                item.skill += itemSkills.Smite;
                item.skillCoolTime = 7f;
                break;
        }
    }

    public List<Item> GetRandomItems(int count)
    {
        List<Item> selectedItems = new List<Item>();
        for (int i = 0; i < count; i++)
        {
            selectedItems.Add(GetRandomItem());
        }
        return selectedItems;
    }


    private Item GetRandomItem()
    {
        ItemRarity selectedRarity;
        List<Item> filteredItems;

        // ???? ???? ???? ?? ??, ?? ??
        do
        {
            selectedRarity = GetRandomRarity();
            filteredItems = items.FindAll(item => item.rarity == selectedRarity);
        }while (filteredItems.Count < 4);
        
        Item item = Item.Clone(filteredItems[Random.Range(0, filteredItems.Count)]);
        return item;
    }

    private ItemRarity GetRandomRarity()
    {
        float rand = Random.Range(0f, 1f);

        if (rand <= 0.02f) return ItemRarity.Legendary;
        else if (rand <= 0.12f) return ItemRarity.Epic;
        else if (rand <= 0.27f) return ItemRarity.Rare;
        else return ItemRarity.Normal;
    }

    public Item GetItemByID(int id)
    {
        Item item = items.FirstOrDefault(item => item.itemID == id);

        if (id == 101)
            item = weaponStick;

        return item;
    }
}
