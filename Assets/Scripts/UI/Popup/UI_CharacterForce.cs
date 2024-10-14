using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterForce : UI_PopUp
{
    enum GameObjects
    {
        Slots = 0,
    }

    enum Buttons
    {
        ExitBtn,
    }

    enum Texts
    {
        SoulCnt
    }

    StatBase[] stats = new StatBase[Enum.GetNames(typeof(StatSpecies)).Length];

    private void Start()
    {
         Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        UpdateSoulTxt();
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn); 

        MakeSlots();
    }

    private void ExitBtn(PointerEventData data)
    {
        base.ClosePopupUI();
    }

    public void UpdateSoulTxt()
    {
        GetText((int)Texts.SoulCnt).text = Managers.Game.player.Inventory.soulCount.ToString();
    }

    private void MakeSlots()
    {
        GameObject slots = Get<GameObject>((int)GameObjects.Slots);
        
        // 패널 초기화
        foreach(Transform child in slots.transform)
            Managers.Resource.Destroy(child.gameObject);

        for(int i = 0; i < (int)StatLevel.Idx; i++)
        {
            GameObject stat = Managers.UI.MakeItems<UI_CharacterForce_Item>(parent:slots.transform).gameObject;

            UI_CharacterForce_Item characterStatForce = stat.GetOrAddComponent<UI_CharacterForce_Item>();
            characterStatForce.SetDescription(i);
        }
    }
}