using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public bool HasSaveData = false;

    public List<StatData> saveStats;
    public List<ItemData> equippedItems;
    public int souls;

    public PlayerData()
    {
        saveStats = new List<StatData>();
        equippedItems = new List<ItemData>();
    }

    [Serializable]
    public class StatData
    {
        public StatSpecies species;
        public int intValue;
        public float floatValue;
        public string type;

        public StatData(StatSpecies species, int value)
        {
            this.species = species;
            this.intValue = value;
            this.type = "int";
        }

        public StatData(StatSpecies species, float value)
        {
            this.species = species;
            this.floatValue = value;
            this.type = "float";
        }
    }

    [Serializable]
    public class ItemData
    {
        public int itemType;
        public int itemID;

        public ItemData(ItemType itemType, Item item)
        {
            this.itemType = (int)itemType;
            this.itemID = item.itemID;
        }

        public ItemData(ItemType itemType, int itemID)
        {
            this.itemType = (int)itemType;
            this.itemID = itemID;
        }
    }

    public void Save()
    {
        if (Managers.Game.player == null)
        {
            return;
        }

        if (HasSaveData)
        {
            saveStats.Clear();
            equippedItems.Clear();
        }

        SavePlayerStat();
        SavePlayerItem();

        HasSaveData = true;
    }

    public void SavePlayerStat()
    {
        PlayerStatHandler stats = Managers.Game.player.StatHandler;

        saveStats.Add(new StatData(StatSpecies.LV, stats.GetStat<int>(StatSpecies.LV).value));
        //saveStats.Add(new StatData(StatSpecies.Name, stats.GetStat<string>(StatSpecies.Name).value));
        saveStats.Add(new StatData(StatSpecies.HP, stats.GetStat<int>(StatSpecies.HP).value));
        saveStats.Add(new StatData(StatSpecies.MaxHP, stats.GetStat<int>(StatSpecies.MaxHP).value));
        saveStats.Add(new StatData(StatSpecies.ATKPower, stats.GetStat<int>(StatSpecies.ATKPower).value));
        saveStats.Add(new StatData(StatSpecies.Exp, stats.GetStat<int>(StatSpecies.Exp).value));
        saveStats.Add(new StatData(StatSpecies.MaxExp, stats.GetStat<int>(StatSpecies.MaxExp).value));
        saveStats.Add(new StatData(StatSpecies.HPLevel, stats.GetStat<int>(StatSpecies.HPLevel).value));
        saveStats.Add(new StatData(StatSpecies.ATKPowerLevel, stats.GetStat<int>(StatSpecies.ATKPowerLevel).value));
        saveStats.Add(new StatData(StatSpecies.ATKRateLevel, stats.GetStat<int>(StatSpecies.ATKRateLevel).value));
        saveStats.Add(new StatData(StatSpecies.SpeedLevel, stats.GetStat<int>(StatSpecies.SpeedLevel).value));

        saveStats.Add(new StatData(StatSpecies.ATKRate, stats.GetStat<float>(StatSpecies.ATKRate).value));
        saveStats.Add(new StatData(StatSpecies.Speed, stats.GetStat<float>(StatSpecies.Speed).value));
    }

    public void SavePlayerItem()
    {
        PlayerInventory inventory = Managers.Game.player.Inventory;

        equippedItems.Clear();

        var weapon = inventory.GetEquippedItem(ItemType.Weapon);
        if (weapon != null)
        {
            equippedItems.Add(new ItemData(ItemType.Weapon, weapon.itemID));
        }

        var armor = inventory.GetEquippedItem(ItemType.Armor);
        if (armor != null)
        {
            equippedItems.Add(new ItemData(ItemType.Armor, armor.itemID));
        }

        var accessory = inventory.GetEquippedItem(ItemType.Accessory);
        if (accessory != null)
        {
            equippedItems.Add(new ItemData(ItemType.Accessory, accessory.itemID));
        }

        souls = inventory.soulCount;
    }

    public void LoadPlayerStats()
    {
        PlayerStatHandler stats = Managers.Game.player.StatHandler;

        foreach (var statData in saveStats)
        {
            if (statData.type == "int")
            {
                stats.GetStat<int>(statData.species).value = statData.intValue;
            }
            else if (statData.type == "float")
            {
                stats.GetStat<float>(statData.species).value = statData.floatValue;
            }
        }
    }

    public void LoadPlayerItems()
    {
        PlayerInventory inventory = Managers.Game.player.Inventory;

        if (equippedItems == null)
        {
            return;
        }
        var equippedItemsCopy = new List<ItemData>(equippedItems);

        foreach (var itemData in equippedItemsCopy)
        {
            if (itemData == null)
            {
                continue;
            }
            //ItemType itemType = (ItemType)itemData.itemType;
            Item item = Managers.Data.Item.GetItemByID(itemData.itemID);
            if (item == null)
            {
                continue;
            }

            switch (item.itemType)
            {
                case ItemType.Weapon:
                    inventory.Equip(item);
                    break;
                case ItemType.Armor:
                    inventory.Equip(item);
                    break;
                case ItemType.Accessory:
                    inventory.Equip(item);
                    break;

            }
        }

        Debug.Log($"Weapon equipped: {inventory.GetEquippedItem(ItemType.Weapon)?.name}");
        Debug.Log($"Armor equipped: {inventory.GetEquippedItem(ItemType.Armor)?.name}");
        Debug.Log($"Accessory equipped: {inventory.GetEquippedItem(ItemType.Accessory)?.name}");

        inventory.soulCount = souls;
    }
}
