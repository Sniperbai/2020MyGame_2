using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    AudioSource audio_music;
    AudioSource audio_sound;

    protected override void Awake()
    {
        base.Awake();

        audio_music = transform.Find("Music").GetComponent<AudioSource>();
        audio_sound = transform.Find("Sound").GetComponent<AudioSource>();

        //初始化数据
        audio_music.volume = PlayerPrefs.GetFloat(Const.MusicVolume, 0);
        audio_sound.volume = PlayerPrefs.GetFloat(Const.SoundVolume, 0);
    }

    public void ChangeMusicVolume(float value)
    {
        audio_music.volume = value;
    }

    public void ChangeSoundVolume(float value)
    {
        audio_sound.volume = value;
    }

    public void PlayMusic(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        PlayMusic(audioClip);
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return;
        }
        audio_music.clip = audioClip;
        audio_music.loop = true;
        audio_music.Play();
    }

    public void PlaySound(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        PlaySound(audioClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return;
        }
        audio_sound.PlayOneShot(audioClip);
    }
}
