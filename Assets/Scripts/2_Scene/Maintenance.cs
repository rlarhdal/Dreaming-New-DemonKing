using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Maintenance : BaseScene
{
    void Start()
    {
//        Init();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = SceneType.MaintenanceScene;

        // Create Player
        GameObject player = Managers.Resource.Instantiate("Player");
       
        player.GetOrAddComponent<Player>().Init();
        player.GetOrAddComponent<PlayerInputHandler>().Init();
        player.GetOrAddComponent<PlayerInventory>().Init();
        Util.FindChild(player, "UnitRoot", true).GetOrAddComponent<PlayerAnimationEvents>().Init();
        player.GetOrAddComponent<PlayerInteraction>().Init();
        Managers.Resource.Load<Material>("SPUM/Materials/SpriteDiffuse").color = Color.white;

        player.GetOrAddComponent<BehaviourMove>().Init();
        player.GetOrAddComponent<BehaviourAttack>().Init();
        player.GetOrAddComponent<BehaviourSkill>().Init();
        player.GetOrAddComponent<BehaviourPotion>().Init();
        player.GetOrAddComponent<BehaviourInteract>().Init();

        // 튜토리얼 생성
        Managers.Camera.Init();
        Managers.Direction.CheckTutorial();

        // UI ????
        Managers.UI.ShowSceneUI<UI_Maintenance>();
        Managers.UI.ShowHUD<UI_HUD>();
        Managers.UI.ShowSceneUI<UI_Player>().Init();

        // ???? ????
        Managers.Sound.PlaySound(Managers.Sound.clipBGM[(int)BGMClip.Maintenance]);

        // NPC ????
        InstantiateNPC();
        
        // Create Player
        InstantiatePlayer();
       Managers.Data.LoadGame();

        if (Managers.Direction.isDemoCompleted)
        {
            Managers.UI.ShowPopupUI<UI_Event>();
        }
    }

    public override void Clear()
    {
    }

    void InstantiatePlayer()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj == null)
        {
            playerObj = Managers.Resource.Instantiate("Player", null, "");
        }
        playerObj.transform.position = Vector3.zero;
        // Player HP recover
        Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.HP).value =
            Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.MaxHP).value +
            Managers.Game.player.StatHandler.GetStat<int>(StatSpecies.plusHP).value;
    }


    /// <summary>
    /// npc ????
    /// </summary>
    private void InstantiateNPC()
    {
        GameObject root = GameObject.Find("NPCs");
        if(root == null)
        {
            root = new GameObject { name = "NPCs" };
        }

        GameObject[] npcs = Resources.LoadAll<GameObject>("Prefabs/NPC");


        for (int i = 0; i < npcs.Length; i++)
        {
            GameObject obj = Instantiate(npcs[i]);
            obj.transform.parent = root.transform;

            CinemachineVirtualCamera cam = obj.GetComponentInChildren<CinemachineVirtualCamera>();
            cam.gameObject.SetActive(false);

            Managers.Direction.npcs[i] = obj.transform;
            Managers.Camera.virtualCams[i] = cam;
        }
    }
}
