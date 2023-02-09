using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaEnergia : MonoBehaviour
{
    private Light[] lights;
    [SerializeField] Material mat;
    [SerializeField] GameObject next;

    private int fusiveis = 4;
    
    private void Awake() {
        lights = FindObjectsOfType<Light>();
        mat.EnableKeyword("_EMISSION");
        mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        mat.SetColor("_EmissionColor", Color.white);
        //LightsOff();
    }

    public void LightsOff(){
        foreach(Light light in lights){
            if(light != null){
                light.enabled = false;
                mat.DisableKeyword("_EMISSION");
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    public void FusivelCerto(){
        fusiveis--;
        FindObjectOfType<AudioManager>().Play("eletricidade");
        if(fusiveis == 0)
            LightsOn();
    }

    private void LightsOn(){
        foreach(Light light in lights){
            if(light != null){
                light.enabled = true;
                mat.EnableKeyword("_EMISSION");
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                mat.SetColor("_EmissionColor", Color.white);
            }
        }
        FindObjectOfType<QuestSistem>().CompleteQuest();
        next.SetActive(true);
    }
}
