using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPlayerHUD : MonoBehaviour
{
    public GameObject objetoPadre;
    public TMP_Text lvlPlayer;
    public TMP_Text exp;
    public Slider sliderLvl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (objetoPadre.activeSelf)
        {
            lvlPlayer.text = LevelSystem.instance.GetLvlPlayer().ToString();
            exp.text = LevelSystem.instance.GetExpActualPlayer().ToString() + "/" + LevelSystem.instance.GetExpNeedNextLevel().ToString();

            sliderLvl.value = LevelSystem.instance.GetExpActualPlayer();
            sliderLvl.maxValue = LevelSystem.instance.GetExpNeedNextLevel();
            sliderLvl.minValue = LevelSystem.instance.GetExpDifference();
        }
        
        
    }
}
