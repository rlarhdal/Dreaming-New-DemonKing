using UnityEngine;

public class MapNPC : NPC
{
    protected override void Init()
    {
        base.Init();
        data = new NpcData();
        data.name = "MapNPC";
        data.position = new Vector2(-.5f, 14.37f);

        transform.position = data.position;
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Map]);
            Managers.UI.ShowPopupUI<UI_Map>();
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
        Managers.UI.ShowPopupUI<UI_Map>();
    }

    public override void SetInfo()
    {

    }
}
