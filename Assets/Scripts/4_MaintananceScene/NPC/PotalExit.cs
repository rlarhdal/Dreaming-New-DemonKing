using UnityEngine;

public class PotalExit : NPC
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.UI.ShowPopupUI<UI_Exit>();
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public override void ShowUI()
    {
        Debug.Log("PotalExit");
    }

    public override void SetInfo()
    {

    }
}
