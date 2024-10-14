using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI_StoreBuy : UI_PopUp
{
    string[] statStr = {"공격력:","공속:","체력:","회복력:","이속:"};
    string[] skillStr = { "암살", "참격", "강타" };
    string[] skillDescPref = { 
        "3초간 은신 후 강력한 공격(공격력의 ", // 단검
        "10 공격력을 가진 참격을  ", // 장검
        "도끼를 휘둘러 공격력의 "  // 도끼
    };
    string[] skillDescSuff = { "배)을 가한다.","회 발사한다.","배의 피해를 입힌다."};
    Item prevItem;
    static Item postItem;
    enum Buttons
    {
        ExitBtn,
        BuyBtn
    }

    enum Texts
    {
        PrevNameTxt,
        PrevStat1Txt,
        PrevStat1Value,
        PrevStat2Txt,
        PrevStat2Value,
        PrevSkill,
        PrevSkillTxt,
        PrevSkillDescription,
        
        PostNameTxt,
        PostStat1Txt,
        PostStat1Value,
        PostStat2Txt,
        PostStat2Value,
        PostSkill,
        PostSkillTxt,
        PostSkillDescription,
        
        CostTxt,
    }

    enum GameObjects
    {
        PrevStat,
        PostStat,
    }

    enum Images
    {
        PrevItemSprite,
        PostItemSprite,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        
        GetButton((int)Buttons.BuyBtn).gameObject.BindEvent(Buy);
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(data => ClosePopupUI());

        UpdatePanel();
    }

    void UpdatePanel()
    {
        prevItem = Managers.Game.player.Inventory.GetEquippedItem(postItem.itemType);
        UpdateTexts();
        UpdateImages();
        UpdateCosts();
    }

    void UpdateCosts()
    {
        Get<TextMeshProUGUI>((int)Texts.CostTxt).text = postItem.soulCost.ToString();
    }
    void UpdateImages()
    {
        // PrevItem
        if (prevItem != null)
        {
            Image prevImage = Get<Image>((int)Images.PrevItemSprite);
            Vector2 sizeDeltaOrigin = prevImage.gameObject.GetComponent<RectTransform>().sizeDelta;
            Vector2 sizeDelta = sizeDeltaOrigin;
            float denom = Mathf.Max(prevItem.itemImage[0].rect.height, prevItem.itemImage[0].rect.width);
            sizeDelta.x *= prevItem.itemImage[0].rect.width / denom;
            sizeDelta.y *= prevItem.itemImage[0].rect.height / denom;
            prevImage.sprite = prevItem.itemImage[0];
            prevImage.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        }
        
        // PostItem
        {
            Image postImage = Get<Image>((int)Images.PostItemSprite);
            Vector2 sizeDeltaOrigin = postImage.gameObject.GetComponent<RectTransform>().sizeDelta;
            Vector2 sizeDelta = sizeDeltaOrigin;
            float denom = Mathf.Max(postItem.itemImage[0].rect.height, postItem.itemImage[0].rect.width);
            sizeDelta.x *= postItem.itemImage[0].rect.width / denom;
            sizeDelta.y *= postItem.itemImage[0].rect.height / denom;
            postImage.sprite = postItem.itemImage[0];
            postImage.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            
        }
        
    }
    void UpdateTexts()
    {
        // Prev Item
        if (prevItem == null)
        {
            Get<GameObject>((int)GameObjects.PrevStat).SetActive(false);
        }
        else
        {
            // Name
            Get<TextMeshProUGUI>((int)Texts.PrevNameTxt).text = string.Format("{0} [{1}]",
                prevItem.itemName, prevItem.rarity.ToString()[0]);

            // Stat  + Skill
            switch (prevItem.itemType)
            {
                case ItemType.Weapon:
                    Get<TextMeshProUGUI>((int)Texts.PrevStat1Txt).text = statStr[0];
                    Get<TextMeshProUGUI>((int)Texts.PrevStat2Txt).text = statStr[1];
                    Get<TextMeshProUGUI>((int)Texts.PrevStat1Value).text =
                        (prevItem.statValue1 + prevItem.enchantLvl * prevItem.enchantFactor1).ToString();
                    Get<TextMeshProUGUI>((int)Texts.PrevStat2Value).text =
                        (prevItem.statValue2 + prevItem.enchantLvl * prevItem.enchantFactor2).ToString();
                    int weaponIdx = prevItem.itemID / 10 % 10 -1;

                    if (weaponIdx != -1)
                    {
                        Get<TextMeshProUGUI>((int)Texts.PrevSkillTxt).text = skillStr[weaponIdx];
                        Get<TextMeshProUGUI>((int)Texts.PrevSkillDescription).text = string.Format("{0} {1} {2}",
                        skillDescPref[weaponIdx],
                        weaponIdx == 1 ? (1.5f + (int)prevItem.rarity).ToString("N1") : weaponIdx == 2 ? (prevItem.rarity + 1).ToString() : (2 + (int)prevItem.rarity).ToString(),
                        skillDescSuff[weaponIdx]
                        );
                    }
                    break;
                case ItemType.Armor:
                    Get<TextMeshProUGUI>((int)Texts.PrevStat1Txt).text = statStr[2];
                    Get<TextMeshProUGUI>((int)Texts.PrevStat2Txt).text = statStr[3];
                    Get<TextMeshProUGUI>((int)Texts.PrevStat1Value).text =
                        (prevItem.statValue1 + prevItem.enchantLvl * prevItem.enchantFactor1).ToString();
                    Get<TextMeshProUGUI>((int)Texts.PrevStat2Value).text =
                        (prevItem.statValue2 + prevItem.enchantLvl * prevItem.enchantFactor2).ToString();
                    Get<TextMeshProUGUI>((int)Texts.PrevSkill).gameObject.SetActive(false);
                    break;
                case ItemType.Accessory:
                    if(prevItem.itemID / 10 % 10 == 1) // atk rate
                        Get<TextMeshProUGUI>((int)Texts.PrevStat1Txt).text = statStr[1];
                    else if (prevItem.itemID / 10 % 10 == 2) // speed
                        Get<TextMeshProUGUI>((int)Texts.PrevStat1Txt).text = statStr[4];
                        
                    Get<TextMeshProUGUI>((int)Texts.PrevStat1Value).text =
                        (prevItem.statValue1 + prevItem.enchantLvl * prevItem.enchantFactor1).ToString();
                    Get<TextMeshProUGUI>((int)Texts.PrevStat2Txt).gameObject.SetActive(false);
                    Get<TextMeshProUGUI>((int)Texts.PrevStat2Value).gameObject.SetActive(false);
                    Get<TextMeshProUGUI>((int)Texts.PrevSkill).gameObject.SetActive(false);
                    break;
            }
        }
        
        // Post Item
            // Name
        Get<TextMeshProUGUI>((int)Texts.PostNameTxt).text = string.Format("{0} [{1}]",
            postItem.itemName, postItem.rarity.ToString()[0]);

        // Stat  + Skill
        switch (postItem.itemType)
        {
            case ItemType.Weapon:
                Get<TextMeshProUGUI>((int)Texts.PostStat1Txt).text = statStr[0];
                Get<TextMeshProUGUI>((int)Texts.PostStat2Txt).text = statStr[1];
                Get<TextMeshProUGUI>((int)Texts.PostStat1Value).text =
                    (postItem.statValue1 + postItem.enchantLvl * postItem.enchantFactor1).ToString();
                Get<TextMeshProUGUI>((int)Texts.PostStat2Value).text =
                    (postItem.statValue2 + postItem.enchantLvl * postItem.enchantFactor2).ToString();
                int postWeaponIdx = (postItem.itemID / 10 % 10) -1;

                Get<TextMeshProUGUI>((int)Texts.PostSkillTxt).text = skillStr[postWeaponIdx];
                Get<TextMeshProUGUI>((int)Texts.PostSkillDescription).text = string.Format("{0} {1} {2}",
                    skillDescPref[postWeaponIdx],
                    postWeaponIdx==1? (1.5f + (int)postItem.rarity).ToString("N1"): postWeaponIdx==2?(postItem.rarity + 1).ToString():(2+(int)postItem.rarity).ToString(),
                    skillDescSuff[postWeaponIdx]
                    );
                break;
            case ItemType.Armor:
                Get<TextMeshProUGUI>((int)Texts.PostStat1Txt).text = statStr[2];
                Get<TextMeshProUGUI>((int)Texts.PostStat2Txt).text = statStr[3];
                Get<TextMeshProUGUI>((int)Texts.PostStat1Value).text =
                    (postItem.statValue1 + postItem.enchantLvl * postItem.enchantFactor1).ToString();
                Get<TextMeshProUGUI>((int)Texts.PostStat2Value).text =
                    (postItem.statValue2 + postItem.enchantLvl * postItem.enchantFactor2).ToString();
                Get<TextMeshProUGUI>((int)Texts.PostSkill).gameObject.SetActive(false);
                break;
            case ItemType.Accessory:
                if(postItem.itemID / 10 % 10 == 1) // atk rate
                    Get<TextMeshProUGUI>((int)Texts.PostStat1Txt).text = statStr[1];
                else if (postItem.itemID / 10 % 10 == 2) // speed
                    Get<TextMeshProUGUI>((int)Texts.PostStat1Txt).text = statStr[4];
                Get<TextMeshProUGUI>((int)Texts.PostStat1Value).text =
                    (postItem.statValue1 + postItem.enchantLvl * postItem.enchantFactor1).ToString();
                Get<TextMeshProUGUI>((int)Texts.PostStat2Txt).gameObject.SetActive(false);
                Get<TextMeshProUGUI>((int)Texts.PostStat2Value).gameObject.SetActive(false);
                Get<TextMeshProUGUI>((int)Texts.PostSkill).gameObject.SetActive(false);
                break;
        }
    }

    public static void SetInfo(Item item)
    {
        postItem = item;
    }

    void Buy(PointerEventData obj)
    {
        if (!Managers.Game.player.Inventory.CheckSoul(postItem.soulCost))
            return;
        Managers.Game.player.Inventory.Equip(postItem);
        Util.FindChild<UI_Store>(Managers.UI.Root).UpdateSoulTxt();
        //Managers.Data.playerData.Save();
        Managers.Data.SaveGame();
        ClosePopupUI();
    }
}