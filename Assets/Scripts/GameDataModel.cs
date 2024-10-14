using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameDataModel : MonoBehaviour
{
    int hp;
    int bulletCount;
    int killCount;
    float timerCount;

    public int HP
    {
        get { return hp; }
        set
        {
            if (value < 0)
            {
                hp = 0;
            }
            else
            {
                hp = value;
            }
            OnHPChanged?.Invoke(hp);
        }
    }
    public UnityAction<int> OnHPChanged;

    public int BulletCount
    {
        get { return bulletCount; }
        set
        {
            if (value < 0)
            {
                bulletCount = 0;
            }
            else
            {
                bulletCount = value;
            }
            OnBulletCountChanged?.Invoke(bulletCount);
        }
    }
    public UnityAction<int> OnBulletCountChanged;

    public int KillCount
    {
        get { return killCount; }
        set
        {
            if (value < 0)
            {
                killCount = 0;
            }
            else
            {
                killCount = value;
            }
            OnKillCountChanged?.Invoke(killCount);
        }
    }
    public UnityAction<int> OnKillCountChanged;

    public float TimerCount
    {
        get { return timerCount; }
        set
        {
            if (value < 0)
            {
                timerCount = 0;
            }
            else
            {
                timerCount = value;
            }
            OnTimerCountChanged?.Invoke(timerCount);
        }
    }
    public UnityAction<float> OnTimerCountChanged;
}
