using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenPortal : MonoBehaviour
{
    // 충돌 객체가 CitizenMovement를 가지고 있을 때 시민을 풀로 이동시킨다.
    // 그리고 맵의 총 객체 수를 -1 한다.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<CitizenMovement>(out CitizenMovement movement))
        {
            if (collision.transform.root.gameObject.TryGetComponent<NormalMapManager>(out NormalMapManager normalMapManager))
            {
                normalMapManager.SubtractCount();
            }
            Managers.Pool.Push(collision.transform.parent.gameObject);
        }
    }
}
