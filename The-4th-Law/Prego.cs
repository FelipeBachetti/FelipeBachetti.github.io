using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prego : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] vent gradeVent;

    private bool isRemoving;
    private float currentTime;

    private void Awake() {
        currentTime = time;
    }

    private void Update() {
        if(isRemoving){
            currentTime -= Time.deltaTime;
            if(currentTime<=0){
                isRemoving = false;
                gradeVent.RemovePrego();
                FindObjectOfType<AudioManager>().StopSound("Chave de fenda");
                Destroy(gameObject);
            }
        }
    }

    public void Remove(){
        FindObjectOfType<AudioManager>().Play("Chave de fenda");
        isRemoving = true;
    }
}
