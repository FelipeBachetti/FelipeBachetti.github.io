using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private GameObject player;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
        player.transform.position = gameObject.transform.position;
    }
}
