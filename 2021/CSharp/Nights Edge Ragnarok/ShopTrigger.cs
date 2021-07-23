using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public bool canOpenShop=false;

    private void Update() 
    {
        if (Input.GetButtonDown("Interact") && canOpenShop)
        {
                FindObjectOfType<ShopMenu>().OpenShop();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)    
    {    
        if (other.tag == "Player") 
            {
                canOpenShop = true;
            }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            FindObjectOfType<ShopMenu>().CloseShop();
            canOpenShop = false;
        }
    }
}
