using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAtoB : MonoBehaviour
{
    private LineRenderer line;
    Vector2 start;
    Transform end;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = 2;
        line.enabled = false;
    }

    // 보스의 공격에서 보스와 플레이어의 사이를 연결하는 Line에 대한 설정
    public void StartPos(Vector2 start, Transform end)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        this.end = end;
    }

    private void Update()
    {
        // 라인 컴포넌트가 활성화일 경우 Line의 마지막 방향을 정해준다.
        if (line.enabled)
        {
            line.SetPosition(1, end.position);
        }
    }

    // Line 컴포넌트를 비활성화 처리
    public void ChangeEnabled()
    {
        line.enabled = false;
    }
}
