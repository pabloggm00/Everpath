using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TiendaHUD : MonoBehaviour
{

    public GameObject panelTienda;
    public List<TMP_Text> nombres;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AbrirTienda()
    {
        panelTienda.SetActive(true);
    }
}
