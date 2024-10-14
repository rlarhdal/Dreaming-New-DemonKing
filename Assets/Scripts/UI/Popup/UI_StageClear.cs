using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StageClear : UI_PopUp
{
    private float alpha = 1f;
    WaitForFixedUpdate timer = new WaitForFixedUpdate();

    enum Texts
    {
        ClearText = 0,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));

        StartCoroutine(UpdateColorAlpha());
        Managers.Game.player.guide.GetTarget(MapGenerator.instance.GetMapPortal());
    }

    IEnumerator UpdateColorAlpha()
    {
        Color textColor = new Color(1f, 1f, 1f, 1f);

        while (alpha > 0)
        {
            textColor.a = alpha;
            GetText((int)Texts.ClearText).color = textColor;

            yield return timer;

            alpha -= 0.01f;
        }

        Managers.UI.ClosePopupUI(this);
    }
}
