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

    // ������ ���ݿ��� ������ �÷��̾��� ���̸� �����ϴ� Line�� ���� ����
    public void StartPos(Vector2 start, Transform end)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        this.end = end;
    }

    private void Update()
    {
        // ���� ������Ʈ�� Ȱ��ȭ�� ��� Line�� ������ ������ �����ش�.
        if (line.enabled)
        {
            line.SetPosition(1, end.position);
        }
    }

    // Line ������Ʈ�� ��Ȱ��ȭ ó��
    public void ChangeEnabled()
    {
        line.enabled = false;
    }
}
