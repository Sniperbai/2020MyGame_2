using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPanel : ViewBase
{
    public Slider slider_boss, slider_defence;

    public void UpdateBossHp(float hp)
    {
        slider_boss.value = hp;
    }

    public void UpdateDefenceHp(float hp)
    {
        slider_defence.value = hp;
    }
}
