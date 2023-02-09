using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] int i;
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            TutorialManager tutorial = FindObjectOfType<TutorialManager>();
            tutorial.i = i;
            tutorial.nextMessage();
            Destroy(gameObject);
        }
    }
}
