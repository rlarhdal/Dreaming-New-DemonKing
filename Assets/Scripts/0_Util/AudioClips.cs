using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    // ��ü���� ����� Ŭ������
    public AudioClip[] clips;

    public void PlayBGM(BGMClip clip)
    {
        Managers.Sound.PlaySound(clips[(int)clip]);
    }

    public void PlaySFX<T>(T clip) where T : Enum
    {
        Managers.Sound.PlayShot(clips[Convert.ToInt32(clip)]);
    }
}
