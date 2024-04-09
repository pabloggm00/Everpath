using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CategoriaConsumible { Curas, PocionesEstados}
public class Consumibles : MonoBehaviour
{

    public int id;
    public string nombre;
    public Sprite sprite;
    public CategoriaConsumible categoria;
    public int valorCuantia;
    public string description;
    public int oroVenta;
    public int probabilidad;

    [HideInInspector]public bool isConsumible = true;

    public Consumibles(int id, string nombre, Sprite preview, CategoriaConsumible categoria, int valorCuantia, string desc, int oro)
    {
        this.id = id;
        this.nombre = nombre;
        this.sprite = preview;
        this.categoria = categoria;
        this.valorCuantia = valorCuantia;
        this.description = desc;
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
