using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    // �÷��̾ �ʸ��� �ִ� Ż��η� �̵��ϸ� ��ȯ�ϴ� UI���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.UI.ShowPopupUI<UI_BackToMain>();
        }
    }
}
