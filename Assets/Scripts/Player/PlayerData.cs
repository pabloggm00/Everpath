using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int hP;
    public int maxHp;
    public int damage;
    public int defensa;
    public int mana;
    public int maxMana;
    public int velocidad;
    public int nivel;
    public int oro;
    public float positionX;
    public float positionY;
    public float positionZ;

    public PlayerData(PlayerController player)
    {
        hP = player.hP;
        maxHp = player.maxHp;
        damage = player.damage;
        defensa = player.defensa;
        mana = player.mana;
        maxMana = player.maxMana;
        velocidad = player.velocidad;
        nivel = player.nivel;
        oro = player.oro;
        positionX = player.transform.position.x;
        positionY = player.transform.position.y;
        positionZ = player.transform.position.z;
    }
}
