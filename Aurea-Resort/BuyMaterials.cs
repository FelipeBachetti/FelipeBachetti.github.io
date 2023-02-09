using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMaterials : MonoBehaviour
{
    public void buy(){
        ResourcesManager manager = FindObjectOfType<ResourcesManager>();
        if(manager.getMoney() >= 200 && 50 + manager.getMaterials() <= manager.getStorage()){
            manager.setMoney(-200, true);
            manager.setMaterials(100, true);
        }
    }
}
