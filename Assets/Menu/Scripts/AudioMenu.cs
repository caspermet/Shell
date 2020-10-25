using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public Slider[] volumeSLiders;

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            volumeSLiders[0].value = AudioManager.instance.masterVolumePercent;
            volumeSLiders[1].value = AudioManager.instance.musicVolumePercent;
            volumeSLiders[2].value = AudioManager.instance.sfxVolumePercent;
        }
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }
}
