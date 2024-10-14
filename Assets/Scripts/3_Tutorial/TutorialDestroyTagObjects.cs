using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class TutorialDestroyTagObjects : TutorialBase
{
    [SerializeField] private GameObject monster;

    [SerializeField] private List<GameObject> objects;

    private bool isDead = false;

    public override void Enter()
    {
        Managers.Scene.CurrentScene.SceneType = SceneType.BattleScene;
        Managers.UI.FindUI<UI_Player>().UIChangeBySceneType();

        Managers.Direction.PlayerAction(true);

        // 파괴해야할 오브젝트들을 활성화
        monster.SetActive(true);
        objects.Add(monster);
    }

    public override void Execute(TutorialController controller)
    {
        if(monster.GetComponentInChildren<Enemy>().status.health == 0)
        {
            Invoke("Dead", 2f);
        }

        if (objects.Count == 0)
        {
            controller.SetNextTutorial();
        }
    }

    public void Dead()
    {
        objects.Clear();
        monster.SetActive(false);
    }

    public override void Exit()
    {
        Managers.Scene.CurrentScene.SceneType = SceneType.MaintenanceScene;
        Managers.UI.FindUI<UI_Player>().UIChangeBySceneType();

        Managers.Direction.PlayerAction(false);
    }
}
