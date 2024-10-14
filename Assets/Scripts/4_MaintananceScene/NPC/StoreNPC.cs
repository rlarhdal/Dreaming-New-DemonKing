using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoreNPC : NPC
{
    protected override void Init()
    {
        base.Init();
        data = new NpcData();
        data.name = "Merchant";
        data.position = new Vector2(-18.5f, -2.43f);

        transform.position = data.position;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            behaviourInteract.SetInteractNPC(this);
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Player"))
        {
            behaviourInteract.SetInteractNPC(null);
        }
    }

    public override void ShowUI()
    {
        Managers.UI.ShowPopupUI<UI_Store>();
    }

    public override void SetInfo()
    {
    }
}
