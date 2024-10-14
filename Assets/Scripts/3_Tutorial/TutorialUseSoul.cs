using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUseSoul : TutorialBase
{
    public override void Enter()
    {

    }

    public override void Execute(TutorialController controller)
    {
        Item[] items = Managers.Game.player.Inventory.items;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                controller.SetNextTutorial();
            }
            else
            {
                continue;
            }
        }
    }

    public override void Exit()
    {

    }
}
