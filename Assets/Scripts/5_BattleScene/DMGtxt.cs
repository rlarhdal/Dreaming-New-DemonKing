using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DMGtxt : MonoBehaviour
{
    TextMeshProUGUI DMGTxt;
    Color txtColor;
    Vector3 startPosition = 2 * Vector3.up;
    float speed = .1f;
    float floatingTime = 2f;

    void Awake()
    {
        DMGTxt = GetComponent<TextMeshProUGUI>();
        txtColor = new Color(1f, 0f, 0f, 1f);
    }

    public void SetInfo(float dmgTxt)
    {
        DMGTxt.text = $"-{dmgTxt}";
    }
    void OnEnable()
    {
        StartCoroutine(IncreasingText());
    }

    IEnumerator IncreasingText()
    {
        float time = 0f;
        while (time < floatingTime)
        {
            time += Time.deltaTime;
            transform.position += speed * Vector3.up * Time.deltaTime;
            txtColor.a = 1f - (float)time / floatingTime;
            DMGTxt.color = txtColor;
            yield return null;
        }
        Managers.Pool.Push(gameObject);
    }
}