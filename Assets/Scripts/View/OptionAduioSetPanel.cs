using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionAduioSetPanel : ViewBase
{
    #region 字段

    public Slider slider_music;              //背景音乐
    public Slider slider_sound;              //背景音效

    #endregion

    #region 事件监听

    public void OnMusicValueChange( float f ) {

        //对音量进行保存
        PlayerPrefs.SetFloat(Const.MusicVolume,f);

        //修改音量大小
        AudioManager.Instance.ChangeMusicVolume(f);

    }

    public void OnSoundValueChange( float f ) {

        //对音效进行保存
        PlayerPrefs.SetFloat(Const.SoundVolume, f);

        //修改音效大小
        AudioManager.Instance.ChangeSoundVolume(f);
    }

    #endregion

    #region 方法重写

    public override void Show()
    {
        base.Show();

        //获取到当前保存的音量大小 进行赋值
        slider_music.value = PlayerPrefs.GetFloat(Const.MusicVolume, 0);

        slider_sound.value = PlayerPrefs.GetFloat(Const.SoundVolume, 0);

    }

    #endregion

}


