using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    
    [SerializeField] Vector3 direction;

    void OnCollisionStay(Collision other) {
        if(other.collider.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction);
        }    
    }

}
