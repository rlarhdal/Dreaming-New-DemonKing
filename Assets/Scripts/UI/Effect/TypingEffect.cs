using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    private bool isTypingEffect;

    [SerializeField] private float typingSpeed = 1f;

    public bool IsTypingEffect(string dialogue, TextMeshProUGUI textDialogues)
    {
        StartCoroutine(TypingText(dialogue, textDialogues));
        
        return true;
    }

    private IEnumerator TypingText(string dialogue, TextMeshProUGUI textDialogues)
    {
        int index = 0;

        isTypingEffect = true;

        // �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
        while (index < dialogue.Length)
        {
            textDialogues.text = dialogue.Substring(0, index);

            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;
    }
}
