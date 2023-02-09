using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTracker : MonoBehaviour
{
    [SerializeField] Transform startingPosition;
    [SerializeField] GameObject person;

    private Transform tracker;

    public void spawn(bool spawning){
        if(spawning){
            tracker = Instantiate(person, new Vector3(startingPosition.position.x, startingPosition.position.y+2, startingPosition.position.z), Quaternion.identity).transform;
            tracker.GetComponent<Guest>().bedroom = this;
        }else
            if(tracker != null)
                Destroy(tracker);
    }

    private void OnDestroy() {
        if(tracker != null)
            Destroy(tracker.gameObject);
    }
}
