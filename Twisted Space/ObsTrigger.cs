using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            FindObjectOfType<PauseMenu>().Die();
        }
    }
}
