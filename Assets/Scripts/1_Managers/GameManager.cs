using System;
using UnityEngine;

public class GameManager
{
    static bool isExist = false;
    public Player player { get; set; }

    public void Init()
    {
        if (isExist == true)
        {
            return;
        }
        isExist = true;
    }
}