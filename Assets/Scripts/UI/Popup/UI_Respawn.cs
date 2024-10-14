using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Respawn : UI_PopUp
{
    public string playerName;
    public string enemyName;
    public int currentSouls;
    public int costSouls;
    
    enum Texts
    {
        DieMessage,
        CurrentSoulTxt,
        CostSoulTxt,
        ResultSoulTxt
    }
    enum Buttons
    {
        Respawn,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        
        GetButton((int)Buttons.Respawn).gameObject.BindEvent(Respawn);

        // Get<TextMeshProUGUI>((int)Texts.DieMessage).text = $"<name>은 <unit>에게 토벌당했습니다.";
        Get<TextMeshProUGUI>((int)Texts.CostSoulTxt).text = MapGenerator.instance.GetSoul().ToString();
        Get<TextMeshProUGUI>((int)Texts.CurrentSoulTxt).text = Managers.Game.player.Inventory.soulCount.ToString();
        Get<TextMeshProUGUI>((int)Texts.ResultSoulTxt).text = (MapGenerator.instance.GetSoul() + Managers.Game.player.Inventory.soulCount).ToString();
    }

    void Respawn(PointerEventData obj)
    {
        //        if(Managers.Game.player.isDie)
        Managers.Data.GiveSouls(MapGenerator.instance.GetSoul());
        Managers.Game.player.StateHandler.stateMachine.ChangeState(Managers.Game.player.StateHandler.GetState(ActionType.Die), true);
        
        Stat<int> mHP = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.MaxHP);
        Stat<int> pHP = Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.plusHP);
        
        Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.HP).value = mHP.value + pHP.value;
        Managers.Game.player.Interaction.UpdateHPBar();
        
        Time.timeScale = 1f;
        Managers.Scene.LoadScene(SceneType.MaintenanceScene);
    }
    private void BackToMaintenance(PointerEventData data)
    {
        Managers.Data.GiveSouls(MapGenerator.instance.GetSoul());
        Managers.Scene.LoadScene(SceneType.MaintenanceScene);
    }
}