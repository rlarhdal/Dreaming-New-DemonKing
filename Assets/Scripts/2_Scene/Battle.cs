using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : BaseScene
{
    void Start()
    {
//        Init();
    }
    protected override void Init()
    {
        base.Init();
        SceneType = SceneType.BattleScene;
        
        // Create pool of Damage text
        if (Managers.Pool.GetOriginal("DMGTxt") == null)
        {
            GameObject dmgObj = Managers.Resource.Load<GameObject>("Prefabs/UI/Popup/DMGtxt");
            Managers.Pool.CreatePool(dmgObj, 20);
        }

        // Create Player
        GameObject player = Managers.Resource.Instantiate("Player");
        player.GetOrAddComponent<Player>().Init();
        player.GetOrAddComponent<PlayerInputHandler>().Init();
        player.GetOrAddComponent<PlayerInventory>().Init();
        Util.FindChild(player,"UnitRoot",true).GetOrAddComponent<PlayerAnimationEvents>().Init();
        player.GetOrAddComponent<PlayerInteraction>().Init();
        
        player.GetOrAddComponent<BehaviourMove>().Init();
        player.GetOrAddComponent<BehaviourAttack>().Init();
        player.GetOrAddComponent<BehaviourSkill>().Init();
        player.GetOrAddComponent<BehaviourPotion>().Init();
        
        // Projectile mother object
        if (GameObject.Find("@Projectiles") == null)
            new GameObject(name: "@Projectiles").AddComponent<ProjectilePool>();
        
        // Map manager >> enemy spawn
        if (GameObject.Find("MapGenerator") == null)
        {
            MapGenerator mapGenerator = new GameObject(name: "MapGenerator").AddComponent<MapGenerator>();
            mapGenerator.SetInfo(GameObject.Find("NavMesh"));
            mapGenerator.Init(); // spawner invoke
        }
        
        
        Managers.Game.player.Interaction.UpdateHPBar();
//        Managers.UI.ShowSceneUI<UI_Battle>();
        Managers.UI.ShowSceneUI<UI_Player>().Init();
        Managers.UI.ShowHUD<UI_HUD>();
        
        
        Managers.Data.LoadGame();
       

        Managers.Sound.PlaySound(Managers.Sound.clipBGM[(int)BGMClip.Town]);
    }

    public override void Clear()
    {

    }
}
