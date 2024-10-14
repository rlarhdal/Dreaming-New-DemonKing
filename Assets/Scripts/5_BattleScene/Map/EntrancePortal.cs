using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrancePortal : MonoBehaviour
{
    [SerializeField] Transform entrance;

    // 포탈의 위치를 가져온다.
    public void GetEntrance(Transform transform)
    {
        entrance = transform;
    }

    // 플레이어가 충돌하면 포션의 개수를 다시 3개로 만든다.
    // 플레이어의 위치를 다음 맵의 입구로 이동시킨다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = entrance.position;
            collision.gameObject.GetComponent<BehaviourPotion>().PotionCount = 3;
            Managers.UI.FindUI<UI_Player>().UpdatePotionCount();
            MapGenerator.instance.stageNum++;
        }
    }
}
