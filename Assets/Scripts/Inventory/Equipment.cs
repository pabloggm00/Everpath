using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CategoriaEquimpent {Arma, Casco, Pechera, Pantalones, Botas};
public enum Rareza {Nula, Gris, Verde, Azul, Morado, Dorada};

public class Equipment : MonoBehaviour
{

    public CategoriaEquimpent tipoDeEquipamiento;
    public Rareza rareza;


    [Header("Stats For Player")]
    public string nameEquipment;
    public int id;
    public int damage;
    public int vida;
    public int defensa;
    public int mana;
    public int velocidad;
    public int oroVenta;

    [Header("Inventory")]
    public Sprite sprite;

    [HideInInspector] public bool isEquipado;
    [HideInInspector] public bool isEquipment;


    public Equipment(string nameEquipment, int id, int damage, int defensa, int vida, int mana, int velocidad, Sprite sprite, bool isEquipado, Rareza rareza, CategoriaEquimpent categoria, int oro)
    {
        this.nameEquipment = nameEquipment;
        this.id = id;
        this.damage = damage;
        this.defensa = defensa;
        this.vida = vida;
        this.mana = mana;
        this.velocidad = velocidad;
        this.sprite = sprite;
        this.isEquipado = isEquipado;
        this.rareza = rareza;
        this.tipoDeEquipamiento = categoria;
        this.oroVenta = oro;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
