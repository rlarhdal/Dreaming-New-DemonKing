using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour, IDamagable
{
    public float interactionLength;
    int objCnt; // objects under interaction
    GameObject targetObject;
    Collider2D collider2D;
    (Vector2 offset,Vector2 size) defaultColliderConfig; // at battle map
    (Vector2 offset,Vector2 size) wideColliderConfig; // at home
    public bool IsHome { get; set; }
    UI_Player uiPlayer;

    PlayerStatHandler statHandler;

    LayerMask mask;
    bool isActive;

    public UnityAction dieEvent;
    public UnityAction hitEvent;
    public bool invincibility = false;

    Image HPbar;
    Canvas worldUICanvas;
    GameObject dmgTxt;
    public void Init()
    {
        Managers.Game.player.Interaction = this;
        interactionLength = 10f;
        objCnt = 0;
        collider2D = GetComponent<BoxCollider2D>();
        defaultColliderConfig = (new Vector2(0f,0.7f), new Vector2(1f,1.3f));
        wideColliderConfig = (new Vector2(0f,0.7f), new Vector2(5f,1.3f));
        IsHome = true;
//        ChangeInteractionRange(!IsHome);
        uiPlayer = Managers.UI.FindUI<UI_Player>();

        statHandler = Managers.Game.player.StatHandler;
        mask = LayerMask.GetMask("UI");
        worldUICanvas = GetComponentInChildren<Canvas>();
        UpdateHPBar();
        dmgTxt = Managers.Pool.GetOriginal("DMGtxt");
    }


    void Update()
    {
        var hits = Physics2D.CircleCastAll(transform.position, interactionLength, Vector2.zero, 0, mask);
        if (hits.Length > 0 && IsHome)
        {
            isActive = Array.Exists(hits,hit => hit.collider.CompareTag(nameof(Tags.UI)));
            uiPlayer.SetButtonState(ActionType.Interact,isActive);
        }
        else
            isActive = false;
        
        if(uiPlayer == null)
            uiPlayer = Managers.UI.FindUI<UI_Player>();
//        uiPlayer.SetButtonState(ActionType.Interact,IsHome && isActive);
    }

    public void ChangeInteractionRange(bool ishome)
    {
        if (ishome)
        {
            collider2D.offset = wideColliderConfig.offset;
//            collider2D.size = wideColliderConfig.size;
        }
        else
        {
            collider2D.offset = defaultColliderConfig.offset;
//            collider2D.size = defaultColliderConfig.size;
        }
    }

    public void ApplyDamage(float dmg)
    {
//        Debug.Log("Test");
        if (invincibility)
            return;
        if (statHandler == null)
            statHandler = Managers.Game.player.StatHandler;
        Stat<int> statHP = statHandler.GetStat<int>(StatSpecies.HP);

        float HP = Mathf.Max(statHP.value - dmg, 0); // damage calculation 
        if (HP == 0)
        {
            statHP.value = 0;
            dieEvent?.Invoke();
        }
        else
        {
            statHP.value = (int)HP;
            if (dmgTxt != null)
            {
                Poolable curDMGtxt = Managers.Pool.Pop(dmgTxt, worldUICanvas.gameObject.transform);
                curDMGtxt.gameObject.transform.position = transform.position + 1.7f * Vector3.up ;
                curDMGtxt.gameObject.GetComponent<DMGtxt>().SetInfo(dmg);
                //     .GetComponent<DMGtxt>().SetInfo(dmg);

            }
            hitEvent?.Invoke();
        }
        UpdateHPBar();
    }

    public void UpdateHPBar()
    {
        if (HPbar == null)
            HPbar = Util.FindChild(gameObject,"HPImg",true).GetComponent<Image>();
        HPbar.fillAmount = (float)(statHandler.GetStat<int>(StatSpecies.HP).value) / 
                           (float)(statHandler.GetStat<int>(StatSpecies.MaxHP).value + statHandler.GetStat<int>(StatSpecies.plusHP).value);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            
        }
    }

    public void ChangeInvincibility(bool opt) => invincibility = opt;
}