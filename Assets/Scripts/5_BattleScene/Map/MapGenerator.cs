using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    List<GameObject> maps = new List<GameObject>();
    GameObject map;
    public static MapGenerator instance;
    [SerializeField] GameObject navMesh;
    int souls = 0;

    int selectNum = -1;
    int currentNum = -1;
    bool isCompleted = false;
    public int stageNum = 0; // 현재 진행 중인 스테이지

    private UI_Result ui_Result;
    private UI_Boss ui_Boss;

    // battle ui에서 사용
    [HideInInspector]
    public int count;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    private void Start()
    {
        MapBuild();
    }

    public void SetInfo(GameObject go)
    {
        navMesh = go;
    }

    public void Init()
    {
        MapBuild();
    }

    // 포탈 입구 위치를 반환
    public Transform GetMapPortal()
    {
        return maps[stageNum].GetComponentInChildren<EntrancePortal>().transform;
    }

    // 랜덤 맵 생성
    public void MapBuild()
    {
        //int count = Random.Range(3, 5);
        count = Random.Range(3, 5);

        for (int i = 0; i < count; i++)
        {
            while (true)
            {
                selectNum = Random.Range(1, 4);

                if (selectNum != currentNum)
                {
                    map = Instantiate(Resources.Load<GameObject>($"Map/Map{selectNum}"), transform.position + new Vector3(i * 150, 0, 0), Quaternion.identity);
                    maps.Add(map);

                    currentNum = selectNum;
                    break;
                }
            }
        }
        Updates();

        Managers.UI.ShowSceneUI<UI_Battle>(); // 배틀맵 UI 생성

        StartCoroutine(DelayBuild());
    }

    // 생성한 맵 navMesh Bake
    IEnumerator DelayBuild()
    {
        navMesh.GetComponent<NavMeshBuild>().enabled = true;
        yield return null;
    }

    // 맵의 다음 포탈 위치 연결
    void Updates()
    {
        for (int i = 0; i < maps.Count; i++)
        {
            if (i == maps.Count - 1)
            {
                GameObject bossMap = Instantiate(Resources.Load<GameObject>($"Map/BossMap"), transform.position + new Vector3(0, 150, 0), Quaternion.identity);
                maps[i].transform.GetComponentInChildren<EntrancePortal>().GetEntrance(bossMap.transform.GetChild(0).transform);
            }
            else if (i == 0)
            {
                Managers.Game.player.gameObject.transform.position = maps[i].transform.GetChild(0).transform.position;
                maps[i].transform.GetComponentInChildren<EntrancePortal>().GetEntrance(maps[i + 1].transform.GetChild(0).transform);
            }
            else
            {
                maps[i].transform.GetComponentInChildren<EntrancePortal>().GetEntrance(maps[i + 1].transform.GetChild(0).transform);
            }
        }
    }

    // 소울의 개수 확인
    public int GetSoul()
    {
        return souls;
    }

    // 보스 UI를  캐싱
    public void GetUI(UI_Boss ui)
    {
        ui_Boss = ui;
    }

    // 영혼의 개수를 상승
    public void AddSoul()
    {
        souls++;
    }

    // 보스 피격 시 보스 체력 UI 감소 되게
    public void Hit(float max, float min)
    {
        ui_Boss.UpdateFillAmount(max, min);
    }

    // 스테이지 클리어 시 보스 체력 UI 비활성화
    // 보스 클리어 이후 5초 뒤 UI 활성화 되게 함
    public void StageClear()
    {
        Managers.UI.ClosePopupUI(ui_Boss);
        //Invoke("ActiveUI", 5.0f);
        Invoke("ActiveEnding", 5.0f);
    }

    // 보스 클리어 5초뒤 결과창 UI 생성
    private void ActiveUI()
    {
        if (ui_Result != null) return;
        ui_Result = Managers.UI.ShowPopupUI<UI_Result>();
    }

    private void ActiveEnding()
    {
        if (!isCompleted)
        {
            SceneManager.LoadScene("EndingScene", LoadSceneMode.Additive);
            isCompleted = true;
        }
    }
}
