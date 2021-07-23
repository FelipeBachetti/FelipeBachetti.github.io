using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSys : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;
    private Queue<string> names;
    public Animator animator;

    void Start()
    {
       sentences = new Queue<string>();
       names = new Queue<string>(); 
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DisplayNextSentence();
        }
    }
    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        sentences.Clear();
        names.Clear();
        
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }
        DisplayNextSentence(); 
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        nameText.text = name;
        dialogueText.text = sentence;
    }
    public void EndDialogue(){
    animator.SetBool("isOpen", false);
    }

}
