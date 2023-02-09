using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeniusClick : MonoBehaviour
{
    [SerializeField] Genius genius;
    [SerializeField] int code;

    public void MouseClick() {
        if(code != -1 || !genius.isRunning && code == -1){
            genius.GeniusInput(code);
        }
    }
}
