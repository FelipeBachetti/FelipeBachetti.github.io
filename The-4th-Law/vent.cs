using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vent : MonoBehaviour
{
    private Rigidbody rb;
    private float pregos = 4;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void RemovePrego(){
        pregos--;
        if(pregos == 0){
            rb.useGravity = true;
            rb.isKinematic = false;
        }

    }
}
