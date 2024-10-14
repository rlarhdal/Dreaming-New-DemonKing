using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Result : UI_PopUp
{
    enum Texts
    {
        Title = 0,
        SoulCnt,
        LevelCnt,
        DuplicateText
    }

    enum Buttons
    {
        BackToMaintenance = 0
    }

    enum GameObjects
    {
        Clear,
        Failed
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.BackToMaintenance).gameObject.BindEvent(BackToMaintenance);

        // ����� ���� Text ����
        if (PlayerPrefs.HasKey("Stage1"))
        {
            GetText((int)Texts.DuplicateText).text = "�ߺ� Ŭ����� ������ �����մϴ�.";
            GetText((int)Texts.SoulCnt).text = MapGenerator.instance.GetSoul().ToString() + " => " + (MapGenerator.instance.GetSoul() * 0.7).ToString();
        }
        else
        {
            PlayerPrefs.SetInt("Stage1", 0);
            GetText((int)Texts.SoulCnt).text = MapGenerator.instance.GetSoul().ToString();
        }
    }

    private void BackToMaintenance(PointerEventData data)
    {
        
        if (PlayerPrefs.HasKey("Stage1"))
        {
            Managers.Data.GiveSouls((int)(MapGenerator.instance.GetSoul() * 0.7f));
        }
        else
        {
            Managers.Data.GiveSouls(MapGenerator.instance.GetSoul());
        }
        Managers.Scene.LoadScene(SceneType.MaintenanceScene);

        Managers.Direction.isDemoCompleted = true;
    }
    
}
