using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int nivel;
    public int hP;
    public int damage;
    public int defensa;
    public int velocidad;
    public Sprite enemySprite;

    [Header("Loot")]
    public int minLoot;
    public int maxLoot;
    public int oro;
    public int expLoot;
    public float probabilidadLoot;
    public Varios loot;
    //tengo que agregar script de varios (que se venden en tiendas)  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TakingDamage(int damage)
    {
        hP = (hP - Mathf.Abs(damage - defensa));
      
        //GetComponentInChildren<DamageNumber>().RotateNumber(1);
        GetComponentInChildren<DamageNumber>().SetNumber(Mathf.Abs(damage - defensa));

        //GetComponentInChildren<Animator>().SetTrigger("Attack");
       

        if (hP <= 0)
        {
            return true;
        }

        return false;
    }

    

}
