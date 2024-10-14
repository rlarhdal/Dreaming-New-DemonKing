
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class NPC : MonoBehaviour
{
    protected NpcData data;
    protected BehaviourInteract behaviourInteract;

    protected void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        behaviourInteract = Managers.Game.player.GetComponent<BehaviourInteract>();
    }

    public class NpcData
    {
        public string name {  get; set; }
//        public string[] lines { get; set; }
        public Vector2 position { get; set; }
    }

    public abstract void SetInfo();

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.UI.FindUI<UI_Player>().ChangeImageTransparency(ActionType.Interact,true);
            Managers.UI.FindUI<UI_Player>().SetButtonActivity(ActionType.Interact,true);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.UI.FindUI<UI_Player>()?.ChangeImageTransparency(ActionType.Interact,false);
            Managers.UI.FindUI<UI_Player>()?.SetButtonActivity(ActionType.Interact,false);
        }
    }

    public abstract void ShowUI();
    
}
