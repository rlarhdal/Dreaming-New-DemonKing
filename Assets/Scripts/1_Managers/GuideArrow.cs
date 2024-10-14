using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrow : MonoBehaviour
{
    private Transform targetPos = null;

    public void GetTarget(Transform target)
    {
        gameObject.SetActive(true);
        targetPos = target;
    }

    private void FixedUpdate()
    {
        // Ÿ�� ������ �ִٸ�
        if(targetPos != null)
        {
            // Ÿ���� ���� ����Ű��
            transform.right = (targetPos.position - transform.position).normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetPos == null) return;

        if (collision.gameObject == targetPos.gameObject)
        {
            Debug.Log("��ǥ�� ����");
            this.gameObject.SetActive(false);
            targetPos = null;
        }
    }
}
