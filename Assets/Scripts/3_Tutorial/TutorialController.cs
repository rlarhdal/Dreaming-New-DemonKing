using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{

    [SerializeField]
    private List<TutorialBase> tutorials;

    private Player player;

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;
    private bool isCompleted = false;

    [HideInInspector] public Stack<GameObject> uis = new Stack<GameObject>();
    [HideInInspector] public UI_Player_Tutorial uI_Player;


    private void Awake()
    {
        player = Managers.Game.player;
    }

    private void Start()
    {
        SetNextTutorial();
    }

    private void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        // ���� Ʃ�丮���� Exit() �޼ҵ� ȣ��
        if( currentTutorial != null )
        {
            currentTutorial.Exit();
        }

        // ������ Ʃ�丮���� �����ߴٸ� CompletedAllTutorials() �޼ҵ� ȣ��
        if ( currentIndex >= tutorials.Count -1)
        {
            CompletedAllTutorials();
            return;
        }

        // ���� Ʃ�丮�� ������ currentTutorial�� ���
        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        // ���� �ٲ� Ʃ�丮���� Enter() �޼ҵ� ȣ��
        currentTutorial.Enter();
    }

    private void CompletedAllTutorials()
    {
        currentTutorial = null ;

        // �ʵ忡 ���ʿ��� ������Ʈ ����
        Poolable[] poolObjs = FindObjectsOfType<Poolable>();
        for (int i = 0; i < poolObjs.Length; i++)
        {
            Destroy(poolObjs[i].gameObject);
        }

        Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name == System.Enum.GetName(typeof(SceneType), SceneType.MaintenanceScene))
        {
            Managers.Direction.DestroyTutorial();
        }
        else
        {
            //SceneManager.UnloadSceneAsync("EndingScene");
            Debug.Log("�̺�Ʈ����");
            Invoke("Result", 5f);
        }
    }

    private void Result()
    {
        Managers.UI.ShowPopupUI<UI_Result>();
    }
}
