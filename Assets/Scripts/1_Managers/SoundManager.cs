using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

public enum SFXClip
{
    Button = 0,
    Choice,
    Map,
    StageSelected,
    StartEffect,
}

public enum BGMClip
{
    Intro = 0,
    Maintenance,
    Town,
    Boss
}

public enum EnemyClip
{
    Attack = 0,
    Hit,
    Die
}

public enum EventBoss
{
    Falling,
    Die
}

public class SoundManager
{
    public AudioMixer mixer;

    public AudioClip[] clipBGM;
    private AudioSource sourceSFX;
    private AudioSource sourceBGM;

    public AudioClips sfxClips;

    public void Init()
    {
        GameObject root = GameObject.Find("@SoundManager");

        Object.DontDestroyOnLoad(root);

        GameObject bgm = Managers.Resource.Instantiate("Sound/BGMSource", root.transform);
        GameObject sfx = Managers.Resource.Instantiate("Sound/SFXSource", root.transform);
        mixer = Resources.Load("AudioMixer/AudioMixer") as AudioMixer;

        // clipSFX�� �� ������Ʈ�� Ŭ������ �߰��� �߱� ������ BGMŬ���� ������
        clipBGM = bgm.GetComponent<AudioClips>().clips;
        sourceBGM = bgm.GetComponent<AudioSource>();
        sourceSFX = sfx.GetComponent<AudioSource>();
        sfxClips = sfx.GetComponent<AudioClips>();
    }

    public void VolumeInit()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            SetMasterVolum(PlayerPrefs.GetFloat("MasterVolume"));
        }

        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume"));
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }
    }

    // Master�� ����� ����
    public void SetMasterVolum(float sliderValue)
    {
        mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }

    // Master�� ���Ұ� ���
    public void MasterVolumMute(bool toggle)
    {
        AudioListener.volume = toggle ? 0 : 1;
    }

    // BGM�� ���Ұ� ���
    public void BGMVolumMute(bool toggle)
    {
        sourceBGM.mute = toggle;
    }

    // SFX�� ���Ұ� ���
    public void SFXVolumMute(bool toggle)
    {
        sourceSFX.mute = toggle;
    }

    // BGM�� ����� ����
    public void SetBGMVolume(float sliderValue)
    {
        mixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
    }

    // SFX�� ����� ����
    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }

    // BGM ����(SoundManager�� BGMŬ���� �����Ƿ� �ٸ� �Ŵ������� ���� ����)
    public void PlaySound(AudioClip clip)
    {
        sourceBGM.Stop();
        sourceBGM.clip = clip;
        sourceBGM.Play();
    }

    // SFX ����(�̱������� �� ������Ʈ�� �ҷ��� �����Ŵ)
    public void PlayShot(AudioClip clip)
    {
        sourceSFX.PlayOneShot(clip);
    }
}
