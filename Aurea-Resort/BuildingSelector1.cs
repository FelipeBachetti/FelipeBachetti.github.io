using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector1 : MonoBehaviour
{
    private Builder builder;

    [SerializeField] Building building;

    private void Awake() {
        builder = FindObjectOfType<Builder>();    
    }

    public void callBuilder(){
        builder.selectBuilding(building);
    }
}
