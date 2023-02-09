using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyerCube : MonoBehaviour
{
    private List<GameObject> guests = new List<GameObject>();

    public void destroy(){
        foreach(GameObject g in guests)
            g.GetComponent<Guest>().killGuest();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "guest" && !guests.Contains(other.gameObject)){
            guests.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "guest" && guests.Contains(other.gameObject))
            guests.Remove(other.gameObject);
    }
}
