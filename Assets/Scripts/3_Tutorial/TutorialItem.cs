using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItem : TutorialBase
{
    [SerializeField] private GameObject stick;

    public override void Enter()
    {

    }

    public override void Execute(TutorialController controller)
    {
        if (GiveItem())
        {
            controller.SetNextTutorial();
        }
    }

    private bool GiveItem()
    {
        Item item = Managers.Resource.Load<Item>("ItemData/defaultWeapon");
        Managers.Game.player.Inventory.Equip(item);
        Managers.Data.SaveGame();

        return true;
    }

    public override void Exit()
    {
        stick.SetActive(false);
        Managers.UI.CloseAllPopupUI();
    }
}
