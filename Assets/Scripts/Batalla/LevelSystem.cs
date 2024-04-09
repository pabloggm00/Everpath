using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{

    public static LevelSystem instance;
    [HideInInspector] public PlayerController player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public int GetLvlPlayer()
    {
        return player.nivel;
    }

    public int GetExpActualPlayer()
    {
        return player.expActual;
    }

    public int GetExpNeedNextLevel()
    {
        return player.expNeedPassLevel;
    }

    public int GetExpDifference()
    {
        return player.differenceExp;
    }
}
