using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMap : MonoBehaviour
{
    BossSpawner spawn; // 스폰 정보
    bool spawnCheck = false; // 보스 스폰 여부

    private void Awake()
    {
        spawn = GetComponentInChildren<BossSpawner>();
    }

    // 보스 스폰 지점에 플레이어가 충돌이 되면 보스를 스폰 하고 BGM을 변경
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spawnCheck == false)
        {
            // 보스 ui 생성
            MapGenerator.instance.GetUI(Managers.UI.ShowPopupUI<UI_Boss>());
            // 스태이지 UI 끄기
            Managers.UI.FindUI<UI_Battle>().StageUI();

            spawn.SpawnStart();
            Managers.Sound.PlaySound(Managers.Sound.clipBGM[(int)BGMClip.Boss]);
            spawnCheck = true;
        }
    }
}
