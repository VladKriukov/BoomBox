using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    [SerializeField] float collectingSpeed = 0.1f;
    
    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Collectable")
        {
            other.transform.Translate((transform.position - other.transform.position) * collectingSpeed);
        }    
    }

}
