using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentObjective : MonoBehaviour
{
    private GameObject[] enemies;
    private Text objective;
    private BoxCollider2D col;

    private void Start() {
        col = GameObject.Find("LevelLoader").GetComponent<BoxCollider2D>();
        objective = GameObject.Find("Objective").GetComponent<Text>();
    }
    private void Update() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length > 0){
            objective.text = "Extermine";
            col.enabled = false;
        }
        else if (enemies.Length == 0){
            objective.text = "Continue";
            col.enabled = true;
        }
    } 
}
