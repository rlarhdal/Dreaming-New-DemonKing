using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss : UI_PopUp
{
    enum Images
    {
        Back = 0,
        Slider
    }

    enum Texts
    {
        MonsterName = 0,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public void UpdateFillAmount(float max, float min)
    {
        GetImage((int)Images.Slider).fillAmount = min / max;
    }
}
