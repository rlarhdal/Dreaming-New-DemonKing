using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshBuild : MonoBehaviour
{
    private NavMeshSurface surface;

    void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    // 컴포넌트가 활성화되면 빌드 되게 설정
    private void OnEnable()
    {
        surface.BuildNavMeshAsync();
    }
}
