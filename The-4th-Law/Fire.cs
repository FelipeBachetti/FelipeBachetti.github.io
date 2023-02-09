using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] float originalSize, minimumSize, speed;
    [SerializeField] FireManager fireManager;
    
    private Vector3 size;

    public void PutOutFire(){
        size -= new Vector3(speed,speed,speed) * Time.fixedDeltaTime;
        if(size.x > minimumSize){
            foreach(Transform child in transform){
                child.localScale = size;
            }
            transform.localScale = size;
        }else{
            fireManager.PutOut();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        size = new Vector3 (originalSize, originalSize, originalSize);
    }
}
