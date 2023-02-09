using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private PlayerMovement player;
    private EnergySlider slider;
    private float currentEnergy, rechargeEnergy;
    private flashlight light;

    [SerializeField] float normalEnergyConsumption, highEnergyConsumption, maxEnergy, jumpEnergy;
    [SerializeField] bool dontStartAtMax;

    public bool isCharging;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        slider = FindObjectOfType<EnergySlider>();
        slider.MaxEnergy(maxEnergy);
        light = FindObjectOfType<flashlight>();
        rechargeEnergy = 20f;
        if(!dontStartAtMax)
            currentEnergy = maxEnergy;
    }

    void Update()
    {
        if(!isCharging){
            if(light.isOn){
                currentEnergy -= highEnergyConsumption * Time.deltaTime;
            }
            if(currentEnergy<=0){
                light.hasEnergy = false;
            }
            /*if(player.isRunning){
                currentEnergy -= highEnergyConsumption * Time.fixedDeltaTime;
            }else{
                currentEnergy -= normalEnergyConsumption * Time.fixedDeltaTime;
            }*/
        }else if(currentEnergy < maxEnergy){
            currentEnergy += rechargeEnergy * Time.deltaTime;
            light.hasEnergy = true;
            FindObjectOfType<AudioManager>().Play("recarga");
        }
        slider.SetEnergy(currentEnergy);
    }

    public void Jumped(){
        currentEnergy -= jumpEnergy;
    }
}
