using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    private Light light;

    public bool isOn, hasEnergy = true;

    private void Awake() {
        light = GetComponent<Light>();
    }

    private void Update() {
        if(Input.GetButtonDown("Flashlight") && hasEnergy){
            ControlLight();
        }else if(!hasEnergy){
            light.enabled = false;
            isOn = false;
        }
    }

    private void ControlLight(){
        if(isOn){
            light.enabled = false;
            isOn = false;
        }else{
            light.enabled = true;
            isOn = true;
        }
    }
}
