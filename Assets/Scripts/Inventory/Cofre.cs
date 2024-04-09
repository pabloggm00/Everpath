using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{

    public int oro;
    public List<Consumibles> pociones;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int SumarCofre()
    {
        if (pociones.Count > 0)
        {
            for (int i = 0; i < pociones.Count; i++)
            {
                InventoryManager.instance.AddConsumible(pociones[i]);
            }
        }

        return oro;
    }
}
