using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionManager
{
    [HideInInspector]
    public bool isTutorialCompleted = false;
    [HideInInspector]
    public bool isDemoCompleted = false;
    // transform
    [HideInInspector] public Transform[] npcs;

    private GameObject tutorial;


    public void Init()
    {
        npcs = new Transform[4];
    }

    // Ʃ�丮�� �����Ͱ� �ֳ� Ȯ��
    public void CheckTutorial()
    {
        Managers.Game.player.transform.position = Vector3.zero;

        if (!PlayerPrefs.HasKey("TutorialComplete") || !isTutorialCompleted)
        {
            InstantiateTutorial();
        }
    }

    // Ʃ�丮�� ����
    public void InstantiateTutorial()
    {
        tutorial = Managers.Resource.Instantiate("Tutorial");
        PlayerAction(false);
    }

    // Player �ൿ ����
    public void PlayerAction(bool isActive)
    {
        Managers.Game.player.canMove = isActive;
        Managers.Game.player.canAttack = isActive;
        Managers.Game.player.canHeal = isActive;
        Managers.Game.player.useSkill = isActive;
    }

    private void SaveCompleteState()
    {
        isTutorialCompleted = true;
        PlayerPrefs.SetInt("TutorialComplete", System.Convert.ToInt16(isTutorialCompleted));
    }

    public void DestroyTutorial()
    {
        SaveCompleteState();

        GameObject.Destroy(tutorial);

        // �÷��̾� ��ġ �ʱ�ȭ
        Managers.Game.player.transform.position = Vector2.zero;
        PlayerAction(true);
        Managers.Camera.playerCam.gameObject.SetActive(true);
    }
}
