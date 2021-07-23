using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    [SerializeField] public int money;
    public Text moneyText;

    private void Update() {
        moneyText.text = money.ToString();
    }
    
    public void AddMoney(int moneyColected){
        money += moneyColected;
    }
    public bool WasteMoney(int price){
        if (price>money){
            Debug.Log("pobre");
            return false;
        }else{
            FindObjectOfType<AudioManager>().PlaySound("Compra_Loja");
            money -= price;
            return true;
        }
    }
}
