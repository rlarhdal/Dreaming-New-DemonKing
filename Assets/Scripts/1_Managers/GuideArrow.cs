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
        // 타겟 정보가 있다면
        if(targetPos != null)
        {
            // 타겟을 향해 가리키게
            transform.right = (targetPos.position - transform.position).normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetPos == null) return;

        if (collision.gameObject == targetPos.gameObject)
        {
            Debug.Log("목표에 도착");
            this.gameObject.SetActive(false);
            targetPos = null;
        }
    }
}
