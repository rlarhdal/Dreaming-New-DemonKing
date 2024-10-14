using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBossAnim : MonoBehaviour
{
    private Animator bossDie;
    public bool isCompleted;
    public AudioClips clips;

    private void Awake()
    {
        bossDie = GetComponent<Animator>();
        clips = GetComponent<AudioClips>();

        // 애니메이션 초기화
        bossDie.SetBool("Talking", false);
        bossDie.SetBool("Die", false);
    }

    public void BossTalk()
    {
        bossDie.SetBool("Talking", true);
    }

    public bool EndAnimation()
    {
        return isCompleted = true;
    }
    public void BossDie()
    {
        bossDie.SetBool("Die", true);
    }

    public bool EndDieAnimation()
    {
        return isCompleted = true;
    }

    public void PlaySfx()
    {
        clips.PlaySFX(EnemyClip.Attack);
    }
}
