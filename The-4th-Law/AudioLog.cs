using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLog : MonoBehaviour
{
    private AudioManager audioManager;
    private float distance = 6f, timer;
    private GameObject player;
    private bool isPlaying, hasListened;
    private Outline outline;

    [SerializeField] string name;
    [SerializeField] GameObject emission, toActivate, toActivate2, toActivate3;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        outline = GetComponent<Outline>();
    }

    private void Update() {
        if(Vector3.Distance(player.transform.position, transform.position) < distance && !hasListened){
            outline.enabled = true;
        }else{
            outline.enabled = false;
        }

        if(isPlaying){
            timer -= Time.deltaTime;
            if(timer <= 0){
                emission.SetActive(false);
                isPlaying = false;
                timer = 0;
            }
        }
    }

    public void Play(){
        if(Vector3.Distance(player.transform.position, transform.position) < distance && !isPlaying){
            hasListened = true;
            emission.SetActive(true);
            timer = audioManager.Play(name);
            isPlaying = true;

            if(toActivate != null){
                toActivate.SetActive(true);
                if(toActivate2 != null)
                    toActivate2.SetActive(true);
                if(toActivate3 != null)
                    toActivate3.SetActive(true);
                FindObjectOfType<QuestSistem>().CompleteQuest();
            }  
        }
    }


}
