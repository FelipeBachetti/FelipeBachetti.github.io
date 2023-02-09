using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extintor : MonoBehaviour
{
    [SerializeField] GameObject particleSys;

    void Update()
    {
        if(Input.GetButton("Interact")){
            particleSys.SetActive(true);
            FindObjectOfType<AudioManager>().Play("Extintor meio");
        }
        if(Input.GetButtonUp("Interact")){
            particleSys.SetActive(false);
            FindObjectOfType<AudioManager>().StopSound("Extintor meio");
        }
    }

}
