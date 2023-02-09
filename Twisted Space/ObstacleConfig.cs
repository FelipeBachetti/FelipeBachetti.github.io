using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleConfiguration")]
public class ObstacleConfig : ScriptableObject
{
    public int diceNum, timeForNext;
    public bool change;
    [Header("Ponto 1:")]
    public bool p1;
    public GameObject prefab1;
    public float directionY1;
    [Header("Ponto 2:")]
    public bool p2;
    public GameObject prefab2;
    public float directionY2;
    [Header("Ponto 3:")]
    public bool p3;
    public GameObject prefab3;
    public float directionY3;
    [Header("Ponto 4:")]
    public bool p4;
    public GameObject prefab4;
    public float directionY4;
    [Header("Ponto 5:")]
    public bool p5;
    public GameObject prefab5;
    public float directionY5;
    [Header("Ponto 6:")]
    public bool p6;
    public GameObject prefab6;
    public float directionY6;
    [Header("Ponto 7:")]
    public bool p7;
    public GameObject prefab7;
    public float directionY7;
    [Header("Ponto 8:")]
    public bool p8;
    public GameObject prefab8;
    public float directionY8;
}

