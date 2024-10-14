using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNPC : NPC
{
    protected override void Init()
    {
        base.Init();
        data = new NpcData();
        data.name = "CharacterForce";
        data.position = new Vector2(20f, -9.16f);

        transform.position = data.position;
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
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

    public override void SetInfo()
    {

    }

    public override void ShowUI()
    {
        Managers.UI.ShowPopupUI<UI_CharacterForce>();
    }
}
