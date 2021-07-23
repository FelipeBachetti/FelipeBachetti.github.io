using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    
    private GameObject player;
    public GameObject spawn;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            FindObjectOfType<DeathCounter>().Died();
            player.transform.position = spawn.transform.position;
        }
    }
}
