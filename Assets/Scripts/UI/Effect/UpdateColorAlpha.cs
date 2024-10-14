using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateColorAlpha : MonoBehaviour
{
    enum Type
    {
        Text,
        Image
    }

    [SerializeField] float alpha = 1f;
    [SerializeField] GameObject go;
    [SerializeField] Type type;

    WaitForFixedUpdate timer = new WaitForFixedUpdate();
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case Type.Text:
                color = go.GetComponent<TextMeshProUGUI>().color;
                break;
            case Type.Image:
                color = go.GetComponent<Image>().color;
                break;
        }
    }

    public void UpdateCoroutine()
    {
        StartCoroutine(UpdateAlpha());
    }

    IEnumerator UpdateAlpha()
    {
        Color ChageColor = new Color(1f, 1f, 1f, 1f);

        while (alpha > 0)
        {
            ChageColor.a = alpha;
            color = ChageColor;

            yield return timer;

            alpha -= 0.01f;
        }

        gameObject.SetActive(false);
    }
}
