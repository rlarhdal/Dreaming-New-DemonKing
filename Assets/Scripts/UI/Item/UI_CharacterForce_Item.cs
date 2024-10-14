using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterForce_Item : UIBase
{
    enum GameObjects
    {
        StatIcon = 0,
        StatName,
        StatCurrentLevel,
        StatDescription,
        ChangeAmount,
        CostText,
        LevelBtn
    }

    //string[] statNames = { "체력", "공격력", "공격속도", "이동속도" };
    string[] statNames = { "체력", "공격력", "이동속도" };
    //string[] statDescriptions = { "최대 체력 강화", "공격력 강화", "공격속도 강화", "이동속도 강화" };
    string[] statDescriptions = { "최대 체력 강화", "공격력 강화", "이동속도 강화" };
    Sprite[] statIcons; // 이미지 로드

    Sprite statIcon;
    string costText, statName, statDescription, chageAmount;
    int statCurrentLevel;

    //reinforce
    int index;
    StatLevel type;

    UI_CharacterForce characterForce;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        characterForce = Managers.UI.FindPopUp<UI_CharacterForce>();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        // 고정 desc
        Description();

        // update value
        SetInfo();
        UpdateValue();

        Get<GameObject>((int)GameObjects.LevelBtn).BindEvent(ReinforceBtn);
    }

    private void UpdateImg()
    {
        statIcons = Resources.LoadAll<Sprite>("Images/StatImg");
    }

    private void ReinforceBtn(PointerEventData data)
    {
        if(!Managers.Game.player.Inventory.CheckSoul(statCurrentLevel * 100)) return; // 로직 수정

        characterForce.UpdateSoulTxt();
        Managers.Game.player.StatHandler.ReinforceStat(type);
        Managers.Data.SaveGame();

        SetInfo();
        UpdateValue();
    }

    void Description()
    {
        // 고정
        Get<GameObject>((int)GameObjects.StatIcon).GetComponent<Image>().sprite = statIcon;
        Get<GameObject>((int)GameObjects.StatName).GetComponent<TextMeshProUGUI>().text = statName;
        Get<GameObject>((int)GameObjects.StatDescription).GetComponent<TextMeshProUGUI>().text = statDescription;
    }

    void UpdateValue()
    {
        // 변경
        Get<GameObject>((int)GameObjects.StatCurrentLevel).GetComponent<TextMeshProUGUI>().text = statCurrentLevel.ToString();
        Get<GameObject>((int)GameObjects.ChangeAmount).GetComponent<TextMeshProUGUI>().text = chageAmount;
        Get<GameObject>((int)GameObjects.CostText).GetComponent<TextMeshProUGUI>().text = $"{statCurrentLevel * 100}";
    }

    public void SetInfo()
    {
        float value;

        switch (type)
        {
            case StatLevel.Hp:
                value = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.MaxHP).value;
                chageAmount = $"{value} -> {value + 10}";
                statCurrentLevel = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.HPLevel).value;
                break;
            case StatLevel.Atkpower:
                value = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.ATKPower).value;
                chageAmount = $"{value} -> {value + 1}";
                statCurrentLevel = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.ATKPowerLevel).value;
                break;
            //case StatLevel.AtkRate:
            //    value = Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.ATKRate).value;
            //    chageAmount = $"{value} -> {value + 1}";
            //    statCurrentLevel = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.ATKRateLevel).value;
            //    break;
            case StatLevel.Speed:
                value = Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.Speed).value;
                chageAmount = $"{value.ToString("N1")} -> {(value + 0.1f).ToString("N1")}";
                statCurrentLevel = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.SpeedLevel).value;
                break;
        }
    }

    public void SetDescription(int idx)
    {
        // 이미지 업로드
        UpdateImg();

        statIcon = statIcons[idx];
        statName = statNames[idx];
        statDescription = statDescriptions[idx];

        switch (idx)
        {
            case 0:
                type = StatLevel.Hp;
                break;
            case 1:
                type = StatLevel.Atkpower;
                break;
            //case 2:
            //    type = StatLevel.AtkRate;
            //    break;
            case 2:
                type = StatLevel.Speed;
                break;
        }
    }
}
