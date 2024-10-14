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
    public int stageNum = 0; // ���� ���� ���� ��������

    private UI_Result ui_Result;
    private UI_Boss ui_Boss;

    // battle ui���� ���
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

    // ��Ż �Ա� ��ġ�� ��ȯ
    public Transform GetMapPortal()
    {
        return maps[stageNum].GetComponentInChildren<EntrancePortal>().transform;
    }

    // ���� �� ����
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

        Managers.UI.ShowSceneUI<UI_Battle>(); // ��Ʋ�� UI ����

        StartCoroutine(DelayBuild());
    }

    // ������ �� navMesh Bake
    IEnumerator DelayBuild()
    {
        navMesh.GetComponent<NavMeshBuild>().enabled = true;
        yield return null;
    }

    // ���� ���� ��Ż ��ġ ����
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

    // �ҿ��� ���� Ȯ��
    public int GetSoul()
    {
        return souls;
    }

    // ���� UI��  ĳ��
    public void GetUI(UI_Boss ui)
    {
        ui_Boss = ui;
    }

    // ��ȥ�� ������ ���
    public void AddSoul()
    {
        souls++;
    }

    // ���� �ǰ� �� ���� ü�� UI ���� �ǰ�
    public void Hit(float max, float min)
    {
        ui_Boss.UpdateFillAmount(max, min);
    }

    // �������� Ŭ���� �� ���� ü�� UI ��Ȱ��ȭ
    // ���� Ŭ���� ���� 5�� �� UI Ȱ��ȭ �ǰ� ��
    public void StageClear()
    {
        Managers.UI.ClosePopupUI(ui_Boss);
        //Invoke("ActiveUI", 5.0f);
        Invoke("ActiveEnding", 5.0f);
    }

    // ���� Ŭ���� 5�ʵ� ���â UI ����
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
