using UnityEngine;

public class ForgeNPC : NPC
{
    protected override void Init()
    {
        base.Init();
        data = new NpcData();
        data.name = "ItemForge";
        data.position = new Vector2(17.38f, 3.44f);

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

    public override void ShowUI()
    {
        Managers.UI.ShowPopupUI<UI_EquipmentForce>();
    }

    public override void SetInfo()
    {
    }
}
