using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UI_Player_Tutorial : UI_Scene
{
    bool canAttack;
    bool canSkill1;
    bool canSkill2;

    Coroutine[] coroutine; // 0: attack, 1: skill, 2: potion
    public float[] coolTimes;
    
    Color[] coolTimeImgColor; // 0: can use, 1: cooltime
    Color[] coolTimeTxtColor;

    Color[] colorImgTransparency = new Color[3];
    Image image;
    Player player;
    enum Images
    {
        BackImg = 0,
        Joystick,
        
        // yjkim
        AttackBtn,
        SkillBtn_1,
        SkillBtn_2,
        AttackIcon,
        AttackCoolTimeImg,
    }

    enum Buttons
    {
        AttackBtn = 0,
    }

    enum Texts // will be facilitated to index of coroutine 
    {
        AttackCoolTimeTxt,
    }

    void Start()
    {
        Managers.UI.UIlist.Add(this);
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        
        GetButton((int)Buttons.AttackBtn).gameObject.BindEvent(AttackBtn);
//        GetButton((int)Buttons.SkillBtn_1).gameObject.BindEvent(SkillBtn_1);
//        GetButton((int)Buttons.SkillBtn_2).gameObject.BindEvent(SkillBtn_2);
        player = GameObject.Find("Player").GetComponent<Player>();

//        Managers.Scene.SceneChangeEvents += UIChangeBySceneType;
        UIChangeBySceneType();

        coolTimes = new float[3] { 1f,5f,1f};
        coroutine = new Coroutine[3];
        coolTimeImgColor = new Color[2]
        {
            new Color(255f/255f, 255f/255f, 255f/255f, 0f/255f),
            new Color(100f/255f, 100f/255f, 100f/255f, 100f/255f)
        };
        coolTimeTxtColor = new Color[2]
        {
            new Color(100f/255f, 100f/255f, 100f/255f, 0f/255f),
            new Color(50f/255f, 50f/255f, 50f/255f, 255f/255f)
        };
//        image = GetImage((int)Images.AttackIcon);
    }

    public void UIChangeBySceneType()
    {
        switch (Managers.Scene.CurrentScene.SceneType)
        {
            case SceneType.Tutorial:
                ChangeImageTransparency(ActionType.Attack,false);
                ChangeImageTransparency(ActionType.Skill,false);
                ChangeImageTransparency(ActionType.Potion,false);
//                GetButton((int)Buttons.AttackBtn).gameObject.SetActive(false);
//                GetButton((int)Buttons.SkillBtn_1).gameObject.SetActive(false);
//                GetButton((int)Buttons.SkillBtn_2).gameObject.SetActive(false);
                break;
            case SceneType.MaintenanceScene:
                ChangeImageTransparency(ActionType.Attack,false);
                ChangeImageTransparency(ActionType.Skill,false);
                ChangeImageTransparency(ActionType.Potion,false);
//                GetButton((int)Buttons.AttackBtn).gameObject.SetActive(false);
//                GetButton((int)Buttons.SkillBtn_1).gameObject.SetActive(false);
//                GetButton((int)Buttons.SkillBtn_2).gameObject.SetActive(false);
                break;
        }
    }
    private void AttackBtn(PointerEventData data)
    {
        if (coroutine[(int)Texts.AttackCoolTimeTxt] == null)
        {
            Managers.Game.player.SpumPrefabs._anim.SetTrigger("Attack");
            coroutine[(int)Texts.AttackCoolTimeTxt] =
                StartCoroutine(StartCoolDown(Images.AttackCoolTimeImg,Texts.AttackCoolTimeTxt,coolTimes[0]));
        }
    }

    IEnumerator StartCoolDown(Images image,Texts text, float coolTime)
    {
        ChangeAnimationParameter(text, true);
        float time = 0f;
        Image coolTimeImg = GetImage((int)image);
        TextMeshProUGUI coolTimeTxt = Get<TextMeshProUGUI>((int)text);
        coolTimeImg.color = coolTimeImgColor[1];
        coolTimeTxt.color = coolTimeTxtColor[1];
        while (time < coolTime)
        {
            time += Time.deltaTime;
            coolTimeImg.fillAmount = 1f - (time / coolTime);
            coolTimeTxt.text = (coolTime - time).ToString("N0");
            yield return null;
        }

        coolTimeImg.color = coolTimeImgColor[0];
        coolTimeTxt.color = coolTimeTxtColor[0];
        coroutine[(int)text] = null;
        ChangeAnimationParameter(text, false);
    }

    void ChangeAnimationParameter(Texts text, bool state)
    {
        switch (text)
        {
            case Texts.AttackCoolTimeTxt:
                Managers.Game.player.SpumPrefabs._anim.SetBool("AttackCoolDown",state);
                break;
        }
    }
    public void ChangeAttackIcon(Item item)
    {
        image = GetImage((int)Images.AttackIcon);
        Debug.Log(image.gameObject.name);
        float denominator = item.itemImage[0].rect.height;
        Vector2 sizeDelta = image.gameObject.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x *= item.itemImage[0].rect.width / denominator ;
        sizeDelta.y *= item.itemImage[0].rect.height / denominator;
        image.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        image.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,45f);
        image.sprite = item.itemImage[0];
    }

    void ChangeImageTransparency(ActionType type, bool active)
    {
        switch(type)
        {
            case ActionType.Attack:
                Color cA  = GetImage((int)Images.AttackBtn).color;
                Color cAI = GetImage((int)Images.AttackIcon).color;
                
                cA.a  = active ? 1f : 50 / 255f;
                cAI.a = active ? 1f : 50 / 255f;

                GetImage((int)Images.AttackBtn).color = cA;
                GetImage((int)Images.AttackIcon).color = cAI;
                break;
        }
    }

    void SetButtonActivity(ActionType type, bool active)
    {
        if (SceneManager.GetActiveScene().name != "BattleScene")
        {
        }
    }
    
    public void SetButtonState(ActionType type, bool active)
    {
        ChangeImageTransparency(type,active);
        SetButtonActivity(type,active);
    }
}
