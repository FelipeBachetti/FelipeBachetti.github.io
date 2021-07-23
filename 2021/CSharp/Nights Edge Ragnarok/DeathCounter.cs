using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    private int Deaths=-1, cargasRes, cargasResAtual;
    public GameObject deathScreen, player;
    public Text text;

    private void Awake() {
        Hub();
    }
    public void Died(){
        deathScreen.SetActive(true);
        text.text = "Cargas de ressurreição restantes: " + cargasResAtual.ToString();
        FindObjectOfType<PlayerMovement>().ResetHealth();
        Time.timeScale = 0f;
    }
    public void Hub(){
        Time.timeScale = 1f;
        Deaths++;
        SceneManager.LoadScene("hub");
        deathScreen.SetActive(false);
        Invoke("HubEvents", 1f);
        cargasResAtual = cargasRes;
    }
    public void UseRes(){
        if (cargasResAtual>0){
            FindObjectOfType<AudioManager>().PlaySound("Ressuscitado");
            cargasResAtual--;
            Time.timeScale = 1f;
            deathScreen.SetActive(false);
        }
    }
    public void AddRes(){
        cargasRes++;
        cargasResAtual = cargasRes;
    }
    private void HubEvents(){
        if (Deaths > 0){
            GameObject.Find("ShopCol").GetComponent<BoxCollider2D>().enabled = true;
        }
        if (Deaths == 0){
            GameObject.Find("Morte0").GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (Deaths == 1){
            GameObject.Find("Morte1").GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (Deaths == 2){

        }
    }
    
}
