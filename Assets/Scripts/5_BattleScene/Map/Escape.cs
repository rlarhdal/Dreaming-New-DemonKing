using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    // 플레이어가 맵마다 있는 탈출로로 이동하면 귀환하는 UI출력
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.UI.ShowPopupUI<UI_BackToMain>();
        }
    }
}
