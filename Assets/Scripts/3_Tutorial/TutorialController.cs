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
        // 현재 튜토리얼의 Exit() 메소드 호출
        if( currentTutorial != null )
        {
            currentTutorial.Exit();
        }

        // 마지막 튜토리얼을 진행했다면 CompletedAllTutorials() 메소드 호출
        if ( currentIndex >= tutorials.Count -1)
        {
            CompletedAllTutorials();
            return;
        }

        // 다음 튜토리얼 과정을 currentTutorial로 등록
        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        // 새로 바뀐 튜토리얼의 Enter() 메소드 호출
        currentTutorial.Enter();
    }

    private void CompletedAllTutorials()
    {
        currentTutorial = null ;

        // 필드에 불필요한 오브젝트 삭제
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
            Debug.Log("이벤트씬끝");
            Invoke("Result", 5f);
        }
    }

    private void Result()
    {
        Managers.UI.ShowPopupUI<UI_Result>();
    }
}
