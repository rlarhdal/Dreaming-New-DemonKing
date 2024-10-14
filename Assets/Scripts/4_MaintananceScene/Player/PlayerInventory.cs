using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public int soulCount;
    [SerializeField]
    public Item[] items;

    Player player;
    PlayerStatHandler statHandler;

    Dictionary<ItemType, SpriteRenderer[]> renderers;

    public void Init()
    {
        Managers.Game.player.Inventory = this;
        player = Managers.Game.player;
        statHandler = player.StatHandler;
        
        soulCount = 10000;
        // total count of equipments : 3
        items = new Item[Enum.GetNames(typeof(ItemType)).Length];
        renderers = new Dictionary<ItemType, SpriteRenderer[]>();
        renderers[ItemType.Weapon] = new SpriteRenderer[2];
        renderers[ItemType.Weapon][0] =
            Util.FindChild<SpriteRenderer>(player.gameObject, "L_Weapon", true);
        renderers[ItemType.Weapon][1] =
            Util.FindChild<SpriteRenderer>(player.gameObject, "R_Weapon", true);
        
        renderers[ItemType.Armor] = new SpriteRenderer[3];
        renderers[ItemType.Armor][0] = 
            Util.FindChild<SpriteRenderer>(player.gameObject, "BodyArmor", true);
        renderers[ItemType.Armor][1] = 
            Util.FindChild<SpriteRenderer>(player.gameObject, "21_LCArm", true);
        renderers[ItemType.Armor][2] = 
            Util.FindChild<SpriteRenderer>(player.gameObject, "-19_RCArm", true);

        renderers[ItemType.Accessory] = new SpriteRenderer[1];
        renderers[ItemType.Accessory][0] = 
            Util.FindChild<SpriteRenderer>(player.gameObject,"Back",true);
        // call data from Save file
    }

    public void AddSoul(int amount)
    {
        soulCount += amount;
    }

    /// <summary>
    /// ???? ????
    /// </summary>
    /// <param name="amount">??????</param>
    public bool CheckSoul(int amount)
    {
        if (soulCount >= amount)
        {
            soulCount -= amount;
            return true;
        }
        return false;
    }

    public void ReplaceItem(Item item)
    {
        Equip(item);
    }

    public void UnEquip(ItemType type)
    {
        Item item = GetEquippedItem(type);
        switch (type)
        {
            case ItemType.Weapon: // plusDMG(int), plusATKRate(float)
                statHandler.GetStat<int>(item.itemStat1).value -=((int)item.statValue1);
                statHandler.GetStat<float>(item.itemStat2).value -=(item.statValue2);
                break;
            case ItemType.Armor:  // plusHP (int), plusSpeed  (float)
                statHandler.GetStat<int>(item.itemStat1).value -=((int)item.statValue1);
                statHandler.GetStat<float>(item.itemStat2).value -=(item.statValue2);
                break;
            case ItemType.Accessory: // plusATKRate(float), plusSpeed(float)
                statHandler.GetStat<float>(item.itemStat1).value -=(item.statValue1);
                statHandler.GetStat<float>(item.itemStat2).value -=(item.statValue2);
                break;
        }
        item = null;
    }
    public void Equip(Item item)
    {
        // check the equippied item and do unequip process
        if (GetEquippedItem(item.itemType) != null)
            UnEquip(item.itemType);
        
        // after confirming the slot blank, fill the input item on the equip slot
        items[(int)item.itemType] = Item.Clone(item); // deep copy the item to the player inventory

        switch (item.itemType)
        {
            case ItemType.Weapon:
                // UI setting
                renderers[ItemType.Weapon][0].sprite = item.itemImage[0];
                if (Managers.UI.FindUI<UI_Player>() != null)
                {
                    Managers.UI.FindUI<UI_Player>().ChangeAttackIcon(item);
                    Managers.UI.FindUI<UI_Player>().ChangeSkill1Icon(item);
                }
                player.gameObject.GetComponentInChildren<BehaviourSkill>().SetItem(item);
                break;
            
            case ItemType.Armor:
                renderers[ItemType.Armor][0].sprite = item.itemImage[1];
                renderers[ItemType.Armor][1].sprite = item.itemImage[2];
                renderers[ItemType.Armor][2].sprite = item.itemImage[3];
                break;
            
            case ItemType.Accessory:
                renderers[ItemType.Accessory][0].sprite = item.itemImage[0];
                break;
        }
        UpdateStatSum();
        player.AnimationEvents.UpdateParameter();
    }

    public Item GetEquippedItem(ItemType type)
    {
        if (items != null) return items[(int)type];
        else return null;
    }


    public void RemoveItem(Item item)
    {
        if(item != null)
        {
            UnEquip(item.itemType);
            items[(int)item.itemType] = null;
        }
    }

    public void UpdateStatSum()
    {
        // plus stats
        int power = 0;//statHandler.GetStat<int>(StatSpecies.plusATKPower).value;
        int HP = 0;//statHandler.GetStat<int>(StatSpecies.plusHP).value;
        float attackRate = 0f;//statHandler.GetStat<float>(StatSpecies.plusATKRate).value;
        float healAmount = .1f;//statHandler.GetStat<float>(StatSpecies.HealAmount).value;
        float moveSpeed = 0f;//statHandler.GetStat<float>(StatSpecies.plusSpeed).value;
        
        foreach (var item in items)
        {
            if (item == null)
                continue;
            switch (item.itemType)
            {
                case ItemType.Weapon:
                    power += (int)(item.statValue1 + item.enchantLvl * item.enchantFactor1);
                    attackRate += item.statValue2 + item.enchantLvl * item.enchantFactor2;
                    break;
                case ItemType.Armor:
                    HP += (int)(item.statValue1 + item.enchantLvl * item.enchantFactor1);
                    healAmount += item.statValue2 + item.enchantLvl * item.enchantFactor2;
                    break;
                case ItemType.Accessory:
                    if(item.itemStat1 == StatSpecies.plusATKRate)
                        attackRate += item.statValue1 + item.enchantLvl * item.enchantFactor1;
                    if(item.itemStat1 == StatSpecies.plusSpeed)
                        moveSpeed  += item.statValue1 + item.enchantLvl * item.enchantFactor1;
                    break;
                default:
                    break;
            }
        }

        statHandler.GetStat<int>(StatSpecies.plusATKPower).value = power;
        statHandler.GetStat<int>(StatSpecies.plusHP).value = HP;
        statHandler.GetStat<int>(StatSpecies.HP).value = HP + statHandler.GetStat<int>(StatSpecies.MaxHP).value;
        statHandler.GetStat<float>(StatSpecies.plusATKRate).value = attackRate;
        statHandler.GetStat<float>(StatSpecies.plusSpeed).value = moveSpeed;
        statHandler.GetStat<float>(StatSpecies.HealAmount).value = healAmount;
        Managers.Game.player.Interaction.UpdateHPBar();
    }
    //public void EnchantItem(ItemType type)
    //{
    //    float p = 1f;
    //    if(items[(int)type].enchantmentLvl != 0)
    //        p = 1f / items[(int)type].enchantmentLvl;
    //    float rnd = Random.Range(0f, 1f);
    //    if (rnd < p)
    //    {
    //        items[(int)type].enchantmentLvl++;
    //    }
    //    else if (rnd < (1-p)*0.5)
    //    {
    //        items[(int)type].enchantValue--;
    //    }
    //    items[(int)type].enchantValue = 1 / p;
    //}
}