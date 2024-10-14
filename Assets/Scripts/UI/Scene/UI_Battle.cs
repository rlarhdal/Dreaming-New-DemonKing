using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Battle : UI_Scene
{
    enum Texts
    {
        ClearText
    }

    enum BackImages
    {
        Progress_Boss,
        Progress1,
        Progress2,
        Progress3,
        Progress4,
    }

    enum PointImages
    {
        ClearImg1,
        ClearImg2,
        ClearImg3,
        ClearImg4,
        ClearImgBoss,
    }

    enum GameObjects
    {
        Progress
    }

    enum Images
    {
        Progress_Boss,
        Progress1,
        Progress2,
        Progress3,
        Progress4,

        ClearImg1,
        ClearImg2,
        ClearImg3,
        ClearImg4,
        ClearImgBoss,
    }

    List<GameObject> backImgs;
    List<GameObject> poinImgs;
    int index = 0;
    GameObject stageClearText;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        stageClearText = GetText((int)Texts.ClearText).gameObject;

        stageClearText.SetActive(false);
        ImageDistribution(); // 이미지 분배
        InitImage(MapGenerator.instance.count); // 초기화
    }

    void OnDestroy()
    {
        Managers.UI.UIlist.Remove(this);
    }

    private void ImageDistribution()
    {
        backImgs = new List<GameObject>();
        for (int i = 0; i < (int)Images.ClearImg1; i++)
        {
            backImgs.Add(GetImage(i).gameObject);
        }

        int cnt = backImgs.Count;
        poinImgs = new List<GameObject>();
        for (int i = cnt; i < ((int)Images.ClearImgBoss) + 1; i++)
        {
            poinImgs.Add(GetImage(i).gameObject);
        }
    }

    public void InitImage(int cnt)
    {
        // 이미지 초기화
        for (int i = 0; i < poinImgs.Count; i++)
        {
            poinImgs[i].SetActive(false);
        }
        for (int i = 0; i < backImgs.Count; i++)
        {
            backImgs[i].SetActive(false);
        }

        // 스테이지 이미지 활성화
        for (int i = 0; i < cnt + 1; i++)
        {
            backImgs[i].SetActive(true);
        }
    }

    public void SetClear()
    {
        stageClearText.SetActive(true);

        poinImgs[index].SetActive(true);
        index++;

        //stageClearText.GetComponent<UpdateColorAlpha>().UpdateCoroutine();
    }

    public void StageUI()
    {
        GetGameObject((int)GameObjects.Progress).gameObject.SetActive(false);
    }
}
