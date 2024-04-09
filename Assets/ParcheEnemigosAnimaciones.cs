using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcheEnemigosAnimaciones : MonoBehaviour
{


    Enemy enemigoPadre;

    // Start is called before the first frame update
    void Start()
    {
        enemigoPadre = GetComponentInParent<Enemy>();
    }

    public void ActivarAnimacion()
    {

    }
}
