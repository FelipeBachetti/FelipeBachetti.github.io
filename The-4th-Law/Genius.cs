using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Genius : MonoBehaviour
{
    [SerializeField] List<GameObject> offList = new List<GameObject>();
    [SerializeField] List<GameObject> onList = new List<GameObject>();
    [SerializeField] List<string> nameList = new List<string>();
    [SerializeField] TMP_Text text;
    [SerializeField] List<string> orderList = new List<string>();

    [SerializeField] Transform character;
    [SerializeField] float distance;
    [SerializeField] GameObject door;

    private bool waitingInput, startTimer;
    private int currentInput, currentOutput;
    private float timer;

    public bool isRunning;
    public int size;

    private void Awake() {
        character = FindObjectOfType<PlayerMovement>().transform;
    }

    public void GeniusInput(int n){
        if(waitingInput){    
            if(nameList[n] == orderList[currentInput]){
                TurnOn(n);
                currentInput++;
            }else{
                text.text = "Errado";
                Invoke("Restart", 1f);
            }
        }else if(n == -1){
            isRunning = true;
            StartCoroutine(StartOrder());
        }
        if(currentInput == size){
            door.GetComponent<DoorController>().enabled = true;
            text.text = "Certo";
            isRunning = false;
            currentInput = 0;
            waitingInput = false;
        }
    }

    public void TurnOn(int code){
        onList[code].SetActive(true);
        Invoke("TurnOff", .9f);
    }

    public int IdentifyName(string name){
        for(int i = 0; i < 4; i++){
            if(nameList[i] == name){
                return i;
            }else{
                i++;
            }
        }
        return 0;
    }
    
    public void TurnOff(){
        foreach(GameObject image in onList){
            image.SetActive(false);
        }
    }

    private void Restart(){
        StartCoroutine(StartOrder());
    }

    IEnumerator StartOrder(){
        //FindObjectOfType<CameraController>().canLook = false;
        //FindObjectOfType<PlayerMovement>().enabled = false;
        //FindObjectOfType<PickupObject>().enabled = false;
        waitingInput = false;
        currentInput = 0;
        text.text = "Espere";
        for(currentOutput=0; currentOutput < size; currentOutput++){
            yield return new WaitForSeconds(.3f);
            int newColor = Random.Range(0,4);
            TurnOn(newColor);
            orderList[currentOutput] = nameList[newColor];
            yield return new WaitForSeconds(.9f);
        }
        text.text = "Vai!";
        waitingInput = true;
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    private void Update() {
        if (Vector3.Distance(character.transform.position, transform.position) <= distance)
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Canvas>(). enabled = false;
        }
    }


}
