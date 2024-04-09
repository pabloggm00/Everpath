using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varios : MonoBehaviour
{

    public int id;
    public string nombre;
    public Sprite sprite;
    public int dineroVenta;
    public int valorCuantia;
    public int probabilidad;

    public Varios(int id, string nombre, Sprite sprite, int dineroVenta, int cuantia)
    {
        this.id = id;
        this.nombre = nombre;
        this.sprite = sprite;
        this.dineroVenta = dineroVenta;
        this.valorCuantia = cuantia;
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
