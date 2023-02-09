using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] float timer, initialSpeedTimer;
    private float speedTimer, rollTimer;
    [SerializeField] private double speed, speedy = 300;
    [HideInInspector] public int diceNum;
    private bool p1, p2, p3, p4, p5, p6, p7, p8, rollingDice;
    [SerializeField] Transform p1Pos, p2Pos, p3Pos, p4Pos, p5Pos, p6Pos, p7Pos, p8Pos;
    private ObstacleConfig[] scriptablePool;
    private ObstacleConfig settings;
    [SerializeField] GameObject prefab;
    private ChangeDice changer;
    private int ammount, rollAmmount;
    [SerializeField] Animator anim;
    [SerializeField] movementOne move;
    public Vector2 onePos;

    private void Awake() {
        diceNum = 2;
        changer = FindObjectOfType<ChangeDice>();
    }

    private void Update() {
        if(timer<=0){
            spawnObstacle();
        }else if (!rollingDice){
            timer -= Time.deltaTime;
        }

        if(speedTimer<=0){
            speed *= 1.04;
            speedy *= 1.02;
            speedTimer = initialSpeedTimer;
        }else{
            speedTimer -= Time.deltaTime;
        }

        if(rollingDice && rollTimer > 0){
            rollTimer -= Time.deltaTime;
        }else if (rollingDice){
            RollTheDice();
        }
    }

    private void spawnObstacle(){
        switch(diceNum){
            case 1:
                scriptablePool = Resources.LoadAll<ObstacleConfig>( "diceone/");
                FindObjectOfType<movementOne>().speed = speedy;
                break;
            case 2:
                scriptablePool = Resources.LoadAll<ObstacleConfig>( "dicetwo/");
                break;
            case 3:
                scriptablePool = Resources.LoadAll<ObstacleConfig>("dicethree/");
                break;
            case 4:
                scriptablePool = Resources.LoadAll<ObstacleConfig>("dicefour/");
                break;
            case 5:
                scriptablePool = Resources.LoadAll<ObstacleConfig>("dicefive/");
                break;
            case 6:
                scriptablePool = Resources.LoadAll<ObstacleConfig>("dicesix/");
                break;
        }
        settings = scriptablePool[Random.Range(0,scriptablePool.Length)];
        if(ammount<6){
            if(settings.p1){
                settings.prefab1.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab1.GetComponent<ObsMovement>().directionY = settings.directionY1; 
                Instantiate(settings.prefab1, p1Pos.position, Quaternion.identity);
            }
            if(settings.p2){
                settings.prefab2.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab2.GetComponent<ObsMovement>().directionY = settings.directionY2; 
                Instantiate(settings.prefab2, p2Pos.position, Quaternion.identity);
            }
            if(settings.p3){
                settings.prefab3.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab3.GetComponent<ObsMovement>().directionY = settings.directionY3; 
                Instantiate(settings.prefab3, p3Pos.position, Quaternion.identity);
            }
            if(settings.p4){
                settings.prefab4.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab4.GetComponent<ObsMovement>().directionY = settings.directionY4; 
                Instantiate(settings.prefab4, p4Pos.position, Quaternion.identity);
            }
            if(settings.p5){
                settings.prefab5.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab5.GetComponent<ObsMovement>().directionY = settings.directionY5; 
                Instantiate(settings.prefab5, p5Pos.position, Quaternion.identity);
            }
            if(settings.p6){
                settings.prefab6.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab6.GetComponent<ObsMovement>().directionY = settings.directionY6; 
                Instantiate(settings.prefab6, p6Pos.position, Quaternion.identity);
            }
            if(settings.p7){
                settings.prefab7.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab7.GetComponent<ObsMovement>().directionY = settings.directionY7; 
                Instantiate(settings.prefab7, p7Pos.position, Quaternion.identity);
            }
            if(settings.p8){
                settings.prefab8.GetComponent<ObsMovement>().speed = (float) speed;
                settings.prefab8.GetComponent<ObsMovement>().directionY = settings.directionY8; 
                Instantiate(settings.prefab8, p8Pos.position, Quaternion.identity);
            }
        }

        if((settings.change && ammount > 1) || (ammount > 5)){
            Invoke("TextAnim", 2f);
            Invoke("RollTheDice", 4f);
            ammount = 0;
        }else if(settings.change && ammount <= 1){
            spawnObstacle();
        }else{
            ammount++;
        }
        timer = settings.timeForNext;
    }

    private void RollTheDice(){
        move.transform.position = onePos;
        move.rb.velocity = new Vector2(0f,0f);

        int newSide = Random.Range(1,7);
        rollingDice = true;
        if(newSide == diceNum){
            RollTheDice();
        }else if(rollTimer <= 0 && rollAmmount < 10){
            changer.ChangeSide(newSide);
            diceNum = newSide;
            rollAmmount++;
            rollTimer = .2f;
        }else if(rollTimer <= 0 && rollAmmount >= 10){
            changer.ChangeSide(newSide);
            diceNum = newSide;
            rollingDice = false;
            rollAmmount = 0;
            rollTimer = .2f;
        }
    }

    private void TextAnim(){
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obs");
        foreach(GameObject obs in obstacles)
        {
            Destroy(obs);
        }
        anim.SetTrigger("RollTheDice");
    }

}
