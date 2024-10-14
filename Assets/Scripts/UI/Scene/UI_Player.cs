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

public class UI_Player : UI_Scene
{
    public UnityAction attackAction;
    public UnityAction skillAction;
    public UnityAction potionAction;
    StateMachine stateMachine;
    Coroutine[] uiCoroutines; // 0: attack, 1: skill, 2: potion
    
    Color[] coolTimeImgColor; // 0: can use, 1: cooltime
    Color[] coolTimeTxtColor;

    Color[] colorImgTransparency = new Color[3];

    Item weapon;

    Player player;
    Vector2 sizeDeltaOrigin = Vector2.zero;
    static readonly int AttackSpeedMultiplier = Animator.StringToHash("AttackSpeedMultiplier");
    static readonly int SkillSpeedMultiplier = Animator.StringToHash("SkillSpeedMultiplier");
    BehaviourSkill skill;
    BehaviourPotion potion;

    static readonly int[] CoolDownHashs =
    {
        Animator.StringToHash("AttackCoolDown"),
        Animator.StringToHash("SkillCoolDown"),
        Animator.StringToHash("PotionCoolDown")
    };

    float[] coolTimes;

    enum Images
    {
        AttackBtn,
        InteractBtn,
        SkillBtn_1,
        SkillBtn_2,
        
        AttackIcon,
        InteractIcon,
        Skill1Icon,
        Skill2Icon,
        
        AttackCoolTimeImg,
        Skill1CoolTimeImg,
        Skill2CoolTimeImg,
        Skill2CountBG,
    }

    enum Buttons
    {
        AttackBtn = 0,
        InteractBtn,
        SkillBtn_1,
        SkillBtn_2,
    }

    enum Texts // will be facilitated to index of coroutine 
    {
        AttackCoolTimeTxt,
        Skill1CoolTimeTxt,
        Skill2CoolTimeTxt,
        
        Skill2CountTxt
    }

    void Start()
    {
//        Init();
    }

    void OnDestroy()
    {
        Managers.UI.UIlist.Remove(this);
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        
        coolTimes = new[] { 0f,0f,1f};
        
        uiCoroutines = new Coroutine[3];
        coolTimeImgColor = new[]
        {
            new Color(255f/255f, 255f/255f, 255f/255f, 0f/255f),
            new Color(100f/255f, 100f/255f, 100f/255f, 100f/255f)
        };
        coolTimeTxtColor = new[]
        {
            new Color(100f/255f, 100f/255f, 100f/255f, 0f/255f),
            new Color(50f/255f, 50f/255f, 50f/255f, 255f/255f)
        };

        player = Managers.Game.player;
        weapon = player.Inventory.GetEquippedItem(ItemType.Weapon);
//        image = GetImage((int)Images.AttackIcon);
        stateMachine = Managers.Game.player.StateHandler.stateMachine;
        skill = player.GetOrAddComponent<BehaviourSkill>();
        potion = player.GetOrAddComponent<BehaviourPotion>();
        UIChangeBySceneType();

        player.SpumPrefabs._anim.SetBool(CoolDownHashs[(int)Texts.Skill2CoolTimeTxt], false);
    }

    public void UIChangeBySceneType()
    {
        Item weapon;
        switch (Managers.Scene.CurrentScene.SceneType)
        {
//            case SceneType.Tutorial:
//                GetButton((int)Buttons.AttackBtn).gameObject.BindEvent(AttackBtn);
//                GetButton((int)Buttons.SkillBtn_1).gameObject.BindEvent(SkillBtn_1);
//                GetButton((int)Buttons.SkillBtn_2).gameObject.BindEvent(SkillBtn_2);
//                ChangeImageTransparency(ActionType.Attack,false);
//                ChangeImageTransparency(ActionType.Skill,false);
//                ChangeImageTransparency(ActionType.Potion,false);
//                GetButton((int)Buttons.AttackBtn).gameObject.SetActive(false);
//                GetButton((int)Buttons.SkillBtn_1).gameObject.SetActive(false);
//                GetButton((int)Buttons.SkillBtn_2).gameObject.SetActive(false);
//
//                break;
            case SceneType.MaintenanceScene:
                weapon = player.Inventory.GetEquippedItem(ItemType.Weapon);
//                GetButton((int)Buttons.AttackBtn).gameObject.BindEvent(AttackBtn);
                //GetButton((int)Buttons.SkillBtn_2).gameObject.BindEvent(SkillBtn_2);
                ChangeImageTransparency(ActionType.Potion, false);
                ChangeImageTransparency(ActionType.Skill,false);
                ChangeImageTransparency(ActionType.Interact,false);
                GetButton((int)Buttons.InteractBtn).gameObject.SetActive(true);
                GetButton((int)Buttons.AttackBtn).gameObject.SetActive(false);
                GetButton((int)Buttons.SkillBtn_1).gameObject.SetActive(false);
                GetButton((int)Buttons.SkillBtn_2).gameObject.SetActive(true);
                GetImage((int)Images.Skill2CountBG).gameObject.SetActive(false);
                if (weapon != null)
                {
                    GetButton((int)Buttons.SkillBtn_1).gameObject.SetActive(true);
                    GetButton((int)Buttons.SkillBtn_2).gameObject.SetActive(true);
                    
                    ChangeAttackIcon(weapon);
                    ChangeSkill1Icon(weapon);
                }
                break;
            
            case SceneType.BattleScene:
                weapon = player.Inventory.GetEquippedItem(ItemType.Weapon);
                GetButton((int)Buttons.AttackBtn).gameObject.BindEvent(AttackBtn);
                GetButton((int)Buttons.SkillBtn_1).gameObject.BindEvent(SkillBtn_1);
                GetButton((int)Buttons.SkillBtn_2).gameObject.BindEvent(SkillBtn_2);
                GetButton((int)Buttons.InteractBtn).gameObject.SetActive(false);
                GetButton((int)Buttons.AttackBtn).gameObject.SetActive(true);
                GetImage((int)Images.Skill2CountBG).gameObject.SetActive(true);
                Get<TextMeshProUGUI>((int)Texts.Skill2CountTxt).text = "3 / 3";
                if(weapon == null)
                    GetButton((int)Buttons.SkillBtn_1).gameObject.SetActive(false);
                else
                {
                    GetButton((int)Buttons.SkillBtn_1).gameObject.SetActive(true);
                    ChangeAttackIcon(weapon);
                    ChangeSkill1Icon(weapon);
                }
                GetButton((int)Buttons.SkillBtn_2).gameObject.SetActive(true); // potion
                break;
        }
    }
    private void SkillBtn_2(PointerEventData data)
    {
        if (uiCoroutines[(int)Texts.Skill2CoolTimeTxt] == null &&
            potion.PotionCount > 0)
        {
            potion.DrinkPotion();
            UpdatePotionCount();
            uiCoroutines[(int)Texts.Skill2CoolTimeTxt] =
                StartCoroutine(StartCoolDown(Images.Skill2CoolTimeImg,Texts.Skill2CoolTimeTxt,coolTimes[2]));
        }
    }

    public void UpdatePotionCount()
    {
        Get<TextMeshProUGUI>((int)Texts.Skill2CountTxt).text = $"{potion.PotionCount} / 3";
    }

    private void SkillBtn_1(PointerEventData data)
    {
        FindAnimationClipLength(1,"5_Skill_Normal");
        if (uiCoroutines[(int)Texts.Skill1CoolTimeTxt] == null)
        {
            skill.UseWeaponSkill();
            float cooltime = coolTimes[1] / player.SpumPrefabs._anim.GetFloat(SkillSpeedMultiplier);
            cooltime = skill.item.skillCoolTime;
            Debug.Log(cooltime);
            uiCoroutines[(int)Texts.Skill1CoolTimeTxt] =
                StartCoroutine(StartCoolDown(Images.Skill1CoolTimeImg,Texts.Skill1CoolTimeTxt,cooltime));
        }
    }
    private void AttackBtn(PointerEventData data)
    {
        FindAnimationClipLength(0,"2_Attack_Normal");
        if (uiCoroutines[(int)Texts.AttackCoolTimeTxt] == null)
        {
            float cooltime = coolTimes[0] / player.SpumPrefabs._anim.GetFloat(AttackSpeedMultiplier);
            uiCoroutines[(int)Texts.AttackCoolTimeTxt] =
                StartCoroutine(StartCoolDown(Images.AttackCoolTimeImg,Texts.AttackCoolTimeTxt,cooltime));
            stateMachine.ChangeState(player.StateHandler.GetState(ActionType.Attack));
        }
    }

    void FindAnimationClipLength(int idx, string clipName)
    {
        if (coolTimes[idx] == 0f)
        {
            foreach (var i in player.SpumPrefabs._anim.runtimeAnimatorController.animationClips)
            {
                if (i.name == clipName)
                {
                    coolTimes[idx] = i.length;
                    return;
                }
            }
        }
    }

    // text contains only cooldown UIs, so It can be utilized to index of array
    IEnumerator StartCoolDown(Images image,Texts text, float coolTime)
    {
        if (!player.SpumPrefabs._anim.GetBool(CoolDownHashs[(int)text]))
        {
            player.SpumPrefabs._anim.SetBool(CoolDownHashs[(int)text], true);
            float time = 0f;
            Image coolTimeImg = GetImage((int)image);
            TextMeshProUGUI coolTimeTxt = Get<TextMeshProUGUI>((int)text);
            
            coolTimeImg.color = coolTimeImgColor[1]; // circular slider begun
            coolTimeTxt.color = coolTimeTxtColor[1]; // makes coolTime countdown visible

            while (time < coolTime) // ui update per frame
            {
                time += Time.deltaTime;
                coolTimeImg.fillAmount = 1f - (time / coolTime);
                coolTimeTxt.text = (coolTime - time).ToString("N0");
                if(text == Texts.Skill1CoolTimeTxt && time > coolTimes[1] / player.SpumPrefabs._anim.GetFloat(SkillSpeedMultiplier))
                    player.SpumPrefabs._anim.SetBool(CoolDownHashs[(int)text],false);
                yield return null;
            }

            coolTimeImg.color = coolTimeImgColor[0];
            coolTimeTxt.color = coolTimeTxtColor[0];
            
            if(text != Texts.Skill1CoolTimeTxt)
                player.SpumPrefabs._anim.SetBool(CoolDownHashs[(int)text], false);
            uiCoroutines[(int)text] = null;
                
        }
    }


    public void ChangeSkill1Icon(Item item)
    {
        if (item.skillIcon == null)
            return;
        if(GetImage((int)Images.SkillBtn_1).gameObject.activeSelf == false)
            GetImage((int)Images.SkillBtn_1).gameObject.SetActive(true);
        Image image = GetImage((int)Images.Skill1Icon);
        image.sprite = item.skillIcon;
    }
    public void ChangeAttackIcon(Item item)
    {
        ChangeImageTransparency(ActionType.Attack,true);
        Image image = GetImage((int)Images.AttackIcon);
        float denominator = item.itemImage[0].rect.height;
        if (sizeDeltaOrigin == Vector2.zero)
            sizeDeltaOrigin = image.gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 sizeDelta = sizeDeltaOrigin;
        sizeDelta.x *= item.itemImage[0].rect.width / denominator ;
        sizeDelta.y *= item.itemImage[0].rect.height / denominator;
        image.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        image.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,45f);
        image.sprite = item.itemImage[0];
    }

    public void ChangeImageTransparency(ActionType type, bool active)
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
            case ActionType.Skill:
                Color cS  = GetImage((int)Images.SkillBtn_1).color;
                Color cSI = GetImage((int)Images.Skill1Icon).color;
                
                cS.a  = active ? 1f : 50 / 255f;
                cSI.a = active ? 1f : 50 / 255f;

                GetImage((int)Images.SkillBtn_1).color = cS;
                GetImage((int)Images.Skill1Icon).color = cSI;
                break;
            case ActionType.Potion:
                Color cP  = GetImage((int)Images.SkillBtn_2).color;
                Color cPI = GetImage((int)Images.Skill2Icon).color;
                
                cP.a  = active ? 1f : 50 / 255f;
                cPI.a = active ? 1f : 50 / 255f;

                GetImage((int)Images.SkillBtn_2).color = cP;
                GetImage((int)Images.Skill2Icon).color = cPI;
                break;
            case ActionType.Interact:
                Color cI = GetImage((int)Images.InteractBtn).color;
                Color cII = GetImage((int)Images.InteractIcon).color;

                cI.a = active ? 1f : 50 / 255f;
                cII.a = active ? 1f : 50 / 255f;

                Image image = GetImage((int)Images.InteractBtn);
                GetImage((int)Images.InteractBtn).color = cI;
                GetImage((int)Images.InteractIcon).color = cII;
                break;
        }
    }

    public void SetButtonActivity(ActionType type, bool active)
    {
        if (SceneManager.GetActiveScene().name != "BattleScene")
        {
            if (type == ActionType.Interact)
            {
//                GetButton((int)Buttons.InteractBtn).enabled = active;
            }
        }
    }
    
    public void SetButtonState(ActionType type, bool active)
    {
        ChangeImageTransparency(type,active);
        SetButtonActivity(type,active);
    }
}
