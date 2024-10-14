using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshBuild : MonoBehaviour
{
    private NavMeshSurface surface;

    void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    // ������Ʈ�� Ȱ��ȭ�Ǹ� ���� �ǰ� ����
    private void OnEnable()
    {
        surface.BuildNavMeshAsync();
    }
}
