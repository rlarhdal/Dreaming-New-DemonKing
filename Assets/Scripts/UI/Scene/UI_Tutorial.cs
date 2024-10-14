using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Tutorial : UI_Scene
{
    enum Buttons
    {
        SkipBtn,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        
        GetButton((int)Buttons.SkipBtn).gameObject.BindEvent(Skip);
        canvas.sortingOrder = 25;
    }

    void Skip(PointerEventData obj)
    {
        Managers.Direction.DestroyTutorial();
        Managers.Scene.CurrentScene.SceneType = SceneType.MaintenanceScene;
        Managers.UI.FindUI<UI_Player>().UIChangeBySceneType();
    }
}