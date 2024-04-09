using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRange : MonoBehaviour
{

    bool isActive;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool EnemyCanDo()
    {
        return isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            Debug.Log("HolaS");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isActive = true;
            Debug.Log("HolaS");
        }
    }
}
