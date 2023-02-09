using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingConfiguration")]
public class Building : ScriptableObject
{
    public int money, materials, maintenance, food, energy, storage;
    public GameObject prefab;
}
