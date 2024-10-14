using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum BlinkingType
{
    Text,
    Image
}

public class ArrowBlink : MonoBehaviour
{
    [SerializeField]
    private float fadeTime; // 페이드 되는 시간
    [SerializeField]
    BlinkingType type = BlinkingType.Text;
    [SerializeField]
    GameObject obj;

    private Image image;
    private TextMeshProUGUI text;
    private Color objColor;

    private void Awake()
    {
        switch (type)
        {
            case BlinkingType.Text:
                text = GetComponent<TextMeshProUGUI>();
                objColor = text.color;
                break;

             case BlinkingType.Image:
                image = GetComponent<Image>();
                objColor = image.color;
                break;
        }
    }

    private void OnEnable()
    {
        // Fade 효과를 In -> Out 무한 반복한다.
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));    // Fade In

            yield return StartCoroutine(Fade(0, 1));    // Fade Out
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = objColor;
            color.a = Mathf.Lerp(start, end, percent);
            objColor = color;
            text.color = objColor;

            yield return null;
        }
    }
}

