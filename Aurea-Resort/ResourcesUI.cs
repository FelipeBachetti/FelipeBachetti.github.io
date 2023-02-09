using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ResourcesUI : MonoBehaviour
{
    [SerializeField] TMP_Text moneyUI, foodUI, energyUI, materialsUI, hapinessUI;

    private ResourcesManager resources;

    private void Awake() {
        resources = FindObjectOfType<ResourcesManager>();
    }

    private void Update() {
        if(resources.getMoney() > 1000)
            moneyUI.text = string.Format("{0:f1}", ((float)resources.getMoney())/1000) +"k";
        else
            moneyUI.text = resources.getMoney().ToString();
        if(resources.getFood() > 1000)
            foodUI.text = string.Format("{0:f1}", ((float)resources.getFood())/1000) +"k";
        else
            foodUI.text = resources.getFood().ToString();
        energyUI.text = resources.getEnergy().ToString();
        if(resources.getMaterials() > 1000)
            materialsUI.text = string.Format("{0:f1}", ((float)resources.getMaterials())/1000) + "k / " + (resources.getStorage()/1000).ToString() + "k";
        else
            materialsUI.text = resources.getMaterials().ToString() + " / " + (resources.getStorage()/1000).ToString() + "k";
        
    }
}
