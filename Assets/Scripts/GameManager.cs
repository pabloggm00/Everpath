using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetHpPlayer()
    {
        return player.hP;
    }

    public int GetMaxHpPlayer()
    {
        return player.maxHp;
    }

    public int GetManaPlayer()
    {
        return player.mana;
    }

    public int GetMaxManaPlayer()
    {
        return player.maxMana;
    }
}
