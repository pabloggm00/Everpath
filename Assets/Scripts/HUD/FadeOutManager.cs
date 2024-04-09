using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutManager : MonoBehaviour
{

    public static FadeOutManager instance;
    public GameObject fadePadre;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarFadeOut() {

        fadePadre.SetActive(true);

    }
}
