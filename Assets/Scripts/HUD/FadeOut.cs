using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{

    public GameObject fadePadre;

    public void Apagar()
    {
        fadePadre.SetActive(false);
    }
}
