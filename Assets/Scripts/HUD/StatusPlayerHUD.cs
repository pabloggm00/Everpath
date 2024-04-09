using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPlayerHUD : MonoBehaviour
{

    public Slider sliderVidaPlayer;
    public TMP_Text textoVidaPlayer;
    public Slider sliderManaPlayer;
    public TMP_Text textoManaPlayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderVidaPlayer.value = GameManager.instance.GetHpPlayer();
        sliderManaPlayer.value = GameManager.instance.GetManaPlayer();
        textoVidaPlayer.text = GameManager.instance.GetHpPlayer().ToString() + "/" + GameManager.instance.GetMaxHpPlayer().ToString();
        textoManaPlayer.text = GameManager.instance.GetManaPlayer().ToString() + "/" + GameManager.instance.GetMaxManaPlayer().ToString();
        sliderVidaPlayer.maxValue = GameManager.instance.GetMaxHpPlayer();
        sliderManaPlayer.maxValue = GameManager.instance.GetMaxManaPlayer();

        
    }
}
