using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] List<GameObject> fireList = new List<GameObject>();
    [SerializeField] GameObject next, next2, room;

    private int size;

    public bool isOver;

    private void OnEnable() {
        //FindObjectOfType<EmergencyLights>().StartEmergency(room);
        size = fireList.Count;

        foreach(GameObject fire in fireList){
            fire.SetActive(true);
        }
    }

    public void PutOut() {
        size--;
        if(size == 0){
            //FindObjectOfType<EmergencyLights>().emergency = false;
            isOver= true;
            FindObjectOfType<QuestSistem>().CompleteQuest();
            next.SetActive(true);
            if(next2 != null)
                next2.SetActive(true);
            Destroy(gameObject);
        }
    }
}
