using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using static PlayerData;

public class DataManager
{
    public PlayerData playerData;
    private ItemManager itemManager;

    public ItemManager Item => itemManager;

    public void Init()
    {
        if (itemManager == null)
            itemManager = new ItemManager();

        if (playerData == null)
            playerData = new PlayerData();

        LoadGame();
    }

    public void GiveSouls(int amount)
    {
        Managers.Game.player.Inventory.AddSoul(amount);
        SaveGame();
    }

    public void SetLvl(int value)
    {
        SaveGame();
    }

    //public void EquipItem(Item newItem)
    //{
    //    Managers.Game.player.Inventory.Equip(newItem);
    //    SaveGame();
    //}

    public void SaveGame()
    {
        playerData.Save();

        string jsonData = JsonUtility.ToJson(playerData, true);

        string filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        Debug.Log($"Player data saved successfully to {filePath}");
        try
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"Player data saved successfully to {filePath}");
            //playerData.HasSaveData = true;
        }
        catch (Exception ex)
        {
            Debug.Log($"Failed to save player data: {ex.Message}");
        }
    }

    public void LoadGame()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "playerData.json");



        if (!File.Exists(filePath))
        {
            Debug.Log("Save file not found!");
            return;
        }

        string jsonData = File.ReadAllText(filePath);

        try
        {
            if (playerData == null)
            {
                playerData = new PlayerData();
            }

            JsonUtility.FromJsonOverwrite(jsonData, playerData);


            playerData.LoadPlayerStats();
            playerData.LoadPlayerItems();
        }
        catch (Exception ex)
        {
            Debug.Log($"Failed to load player data: {ex.Message}");
        }

    }

}
