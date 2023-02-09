using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private string currentMessage;
    public static bool isShowing;
    private bool isReseting;

    [SerializeField] GameObject background;
    [SerializeField] TypeOutScript UIText;
    [SerializeField] List<string> messages = new List<string>();
    [SerializeField] DoorController door;

    public int i = 0;

    private void Start() {
        nextMessage();
    }

    private void displayCurrentMessage(){
        UIText.On = true;
        background.SetActive(true);
        UIText.FinalText = currentMessage;
        UIText.gameObject.GetComponent<Text>().enabled = true;
        isShowing = true;
    }

    public void nextMessage(){
        if(messages[i] == "-"){
            finishTutorial();
        }else if(messages[i] == "open"){
            door.enabled = true;
            FindObjectOfType<QuestSistem>().CompleteQuest();
            finishTutorial();
            }else{
            UIText.reset = true;
            currentMessage = messages[i];
            //displayCurrentMessage();
            UIText.enabled = true;
            isReseting = true;
        }
    }

    private void finishTutorial(){
        background.SetActive(false);
        UIText.enabled = false;
        UIText.gameObject.GetComponent<Text>().enabled = false;
        isShowing = false;
    }

    private void Update() {
        if(isShowing && Input.GetButtonDown("next")){
            i++;
            nextMessage();
        }

        if(isReseting){
            if(!UIText.reset){
                displayCurrentMessage();
                isReseting = false;
            }
        }
    }
}
