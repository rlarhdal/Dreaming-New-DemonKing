using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public (StageSkillPassive skill, int lvl)[] StageSkillsPassive;
    public (StageSkillRelatedToAttack skill, int lvl)[] StageSkillsRelatedToAttack;
    static Player instance;

    public PlayerAnimationEvents AnimationEvents { get; set; }
    public PlayerInputHandler InputHandler { get; set; }
    public PlayerStateHandler StateHandler { get; set; }
    public PlayerStatHandler StatHandler { get; set; }
    public PlayerInventory Inventory { get; set; }
    public SPUM_Prefabs SpumPrefabs { get; private set; }
    
    public PlayerInteraction Interaction { get; set; }

    public Material mat;

    public GuideArrow guide;

    // 움직임 조작
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool canHeal = true;
    [HideInInspector] public bool useSkill = true;

    public void Init()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        Managers.Game.player = this;
        SpumPrefabs = GetComponentInChildren<SPUM_Prefabs>();
        StateHandler = new PlayerStateHandler();
        StatHandler = new PlayerStatHandler();
        StageSkillsPassive = new (StageSkillPassive skill, int lvl)[3];
        StageSkillsRelatedToAttack = new (StageSkillRelatedToAttack skill, int lvl)[3];
        guide = GetComponentInChildren<GuideArrow>();
        guide.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            Managers.Data.Init();
        }

        mat = Managers.Resource.Load<Material>("SPUM/Materials/SpriteDiffuse");
    }

    public void ClearStageSkills()
    {
        Array.Clear(StageSkillsPassive,0,3);
        Array.Clear(StageSkillsRelatedToAttack,0,3);
    }

    void OnDestroy()
    {
        Managers.Data.playerData.Save();
    }
}