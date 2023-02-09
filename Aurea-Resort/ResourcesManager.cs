using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { get; private set; }

    public int money, food, energy, materials, hospedes, maintenance, prodFood, storage;

    [SerializeField] int startingMoney, startingFood, startingEnergy, startingMaterials, startingStorage;

    [HideInInspector] public int n_Hosp, n_Ger, n_Coz, n_Ent;

    [SerializeField] int hospGanho, hospCome, hospEnt;

    [SerializeField] float tempo;

    [SerializeField] GameObject lostPanel;

    private float tempoAtual;

    private void Awake() {
        Instance = this;

        money = startingMoney;
        food = startingFood;
        energy = startingEnergy;
        materials = startingMaterials;
        storage = startingStorage;
        tempoAtual = tempo;
    }

    private void Update() {

        if(money < 0){
            lostPanel.SetActive(true);
            //Time.timeScale = 0f;
        }

        if(materials > storage)
            materials = storage;
    }

    public int getMoney(){
        return money;
    }

    public void setMoney(int money, bool change){
        if(change)
            this.money += money;
        else
            this.money = money; 
    }

    public int getMaintenance(){
        return maintenance;
    }

    public int getFood(){
        return food;
    }

    public int getEnergy(){
        return energy;
    }

    public int getMaterials(){
        return materials;
    }

    public int getStorage(){
        return storage;
    }

    public void setMaterials(int materials, bool change){
        if(change)
            this.materials += materials;
        else
            this.materials = materials; 
    }

    public int getProdFood(){
        return prodFood;
    }

    public void setProdFood(int prodFood, bool change){
        if(change)
            this.prodFood += prodFood;
        else
            this.prodFood = prodFood; 
    }

    public bool newBuilding(Building b){
        if(money >= b.money && materials >= b.materials && energy >= (b.energy*-1)){
            return true;
        }else{
            return false;
        }
    }

    public void foodIncrease(){
        food += prodFood;
    }

    

    public void MaintenanceCost(){
        money -= maintenance;
    }
}
