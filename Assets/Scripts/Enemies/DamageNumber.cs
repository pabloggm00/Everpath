using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DamageNumber : MonoBehaviour
{
    TMP_Text number;
    public Color textColor;
    public float fadeTime = 1;
    public float moveUpTime = 1;

    float elapsedTime;
    bool fadeOut = false;
    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        number = GetComponent<TMP_Text>();
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (fadeOut)
        {
            HideNumber();
        }
        
    }


    public void SetNumber(float number)
    {
        this.number.text = number.ToString();
        ShowNumber();
    }

    public void RotateNumber(int pos)
    {
        if (pos == 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (pos == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void ShowNumber()
    {

        elapsedTime = 0;

        this.number.gameObject.SetActive(true);
        this.number.color = textColor;
        fadeOut = true;
    }

    private void HideNumber()
    {
        
        elapsedTime += Time.deltaTime;

        this.number.color = new Color(this.number.color.r, this.number.color.g, this.number.color.b, Mathf.Lerp(1, 0, elapsedTime / fadeTime));
        this.number.transform.position = new Vector3(this.number.transform.position.x, this.number.transform.position.y + Time.deltaTime * moveUpTime, this.number.transform.position.z);

        if (elapsedTime >= fadeTime)
        {
            fadeOut = false;
            elapsedTime = 0;
            this.number.transform.localPosition = initialPosition;
        }

    }
}
