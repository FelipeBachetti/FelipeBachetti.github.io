using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionDetector : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] GameObject onObject, offObject;

    private Vector3 pos;
    private CaixaEnergia caixa;
    private PickupObject pickObj;

    private void Awake() {
        pos = onObject.transform.position;
        caixa = FindObjectOfType<CaixaEnergia>();
        pickObj = FindObjectOfType<PickupObject>();
    }

    private void Update() {
        if(Vector3.Distance(offObject.transform.position, transform.position)> .3f){
            GetComponent<Collider>().enabled  = true;
        }else{
            GetComponent<Collider>().enabled  = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Pickupable fus = other.GetComponent<Pickupable>();
        if(fus != null && fus.gameObject.tag != "off"){
            fus.transform.position = pos;
            pickObj.dropObject();
            offObject = fus.gameObject;
            if(fus.name == name){
                Debug.Log("b");
                caixa.FusivelCerto();
                Destroy(fus);
            }
        }
    }
}
