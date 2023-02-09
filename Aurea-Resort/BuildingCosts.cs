using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCosts : MonoBehaviour
{
    [SerializeField] Building building;
    void OnEnable()
    {
        ResourcesManager.Instance.money -= building.money;
        ResourcesManager.Instance.materials -= building.materials;
        ResourcesManager.Instance.energy += building.energy;
        ResourcesManager.Instance.prodFood += building.food;
        ResourcesManager.Instance.maintenance += building.maintenance;
        ResourcesManager.Instance.storage += building.storage;
    }

    private void OnDisable() {
        ResourcesManager.Instance.energy -= building.energy;
        ResourcesManager.Instance.prodFood -= building.food;
        ResourcesManager.Instance.maintenance -= building.maintenance;
        ResourcesManager.Instance.storage -= building.storage;
    }
}
