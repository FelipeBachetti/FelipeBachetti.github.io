using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtintorParticula : MonoBehaviour
{
    ParticleSystem ps;

    private void Awake() {
        ps = GetComponent<ParticleSystem>();
    }
    
    private void OnParticleCollision(GameObject other) {
        Fire f = other.GetComponent<Fire>();
        if(f != null){
            f.PutOutFire();
        }
    }
}
