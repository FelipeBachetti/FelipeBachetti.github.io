using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermaBuffs : MonoBehaviour
{
    [SerializeField] public int health=2, dam=2, Res=2;
    private bool canBuy;
    public Text hText, dText, resText;

    private void Update() {
        hText.text = health.ToString();
        dText.text = dam.ToString();
        resText.text = Res.ToString();
    }
    public void ExtraHealth(){
        canBuy = FindObjectOfType<Currency>().WasteMoney(health);
        if(canBuy){
            FindObjectOfType<PlayerMovement>().ExtraHitPoints();
            canBuy = false;
            health *= 2;
        }
    }
    public void ExtraDamage(){
        canBuy = FindObjectOfType<Currency>().WasteMoney(dam);
        if(canBuy){
            FindObjectOfType<MultiplicadorDano>().ExtraDamage();
            canBuy = false;
            dam *= 2;
        }
    }
    public void Resurrection(){
        canBuy = FindObjectOfType<Currency>().WasteMoney(Res);
        if(canBuy){
            FindObjectOfType<DeathCounter>().AddRes();
            canBuy = false;
            Res *= 2;
        }
    }
}
