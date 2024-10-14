using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Options : UI_PopUp
{
    enum Buttons
    {
        ExitBtn,
        GameExitBtn,
        Tutorial
    }

    enum Sliders
    {
        MasterSlider = 0,
        MusicSlider,
        SFXSlider
    }

    Slider master;
    Slider bgm;
    Slider sfx;

    private void Start()
    {
        Init();
        Time.timeScale = 0f;
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn);
        GetButton((int)Buttons.GameExitBtn).gameObject.BindEvent(GameExitBtn);
        GetButton((int)Buttons.Tutorial).gameObject.BindEvent(Tutorial);
        GetSlider((int)Sliders.MasterSlider).gameObject.BindEvent(MasterSlider);
        GetSlider((int)Sliders.MusicSlider).gameObject.BindEvent(MusicSlider);
        GetSlider((int)Sliders.SFXSlider).gameObject.BindEvent(SFXSlider);

        master = GetSlider((int)Sliders.MasterSlider);
        bgm = GetSlider((int)Sliders.MusicSlider);
        sfx = GetSlider((int)Sliders.SFXSlider);

        VolumeInit();
    }

    private void VolumeInit()
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            master.value = PlayerPrefs.GetFloat("MasterVolume");
            Managers.Sound.SetMasterVolum(master.value);
        }

        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgm.value = PlayerPrefs.GetFloat("BGMVolume");
            Managers.Sound.SetBGMVolume(bgm.value);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfx.value = PlayerPrefs.GetFloat("SFXVolume");
            Managers.Sound.SetSFXVolume(sfx.value);
        }
    }

    private void SFXSlider(PointerEventData data)
    {
        Managers.Sound.SetSFXVolume(sfx.value);
    }

    private void MusicSlider(PointerEventData data)
    {
        Managers.Sound.SetBGMVolume(bgm.value);
    }

    private void MasterSlider(PointerEventData data)
    {
        Managers.Sound.SetMasterVolum(master.value);
    }

    private void SetVolumes()
    {
        PlayerPrefs.SetFloat("MasterVolume", master.value);
        PlayerPrefs.SetFloat("BGMVolume", bgm.value);
        PlayerPrefs.SetFloat("SFXVolume", sfx.value);
    }

    private void Tutorial(PointerEventData data)
    {
        Managers.Direction.isTutorialCompleted = false;
        Managers.Direction.CheckTutorial();
    }

    private void ExitBtn(PointerEventData data)
    {
        SetVolumes();
        base.ClosePopupUI();
        Time.timeScale = 1.0f;
    }

    private void GameExitBtn(PointerEventData data)
    {
        Managers.Data.SaveGame();
        SetVolumes();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
