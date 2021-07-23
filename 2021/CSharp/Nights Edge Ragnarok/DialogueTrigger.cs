using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool canOpen;
    [SerializeField] private bool triggerOnAwake;

    private void Awake() {
        if (triggerOnAwake){
            FindObjectOfType<DialogSys>().StartDialogue(dialogue);
        }
    }
    private void Update() 
    {
        if (canOpen)
        {
                FindObjectOfType<DialogSys>().StartDialogue(dialogue);
                canOpen = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)    
    {    
        if (other.tag == "Player") 
            {
                canOpen = true;
            }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            FindObjectOfType<DialogSys>().EndDialogue();
            canOpen = false;
        }
    }
}
