using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDetectorTubes : MonoBehaviour
{
    [SerializeField] string name;
    //[SerializeField] GameObject offObject;

    private Vector3 pos;
    //private CaixaEnergia caixa;
    private PickupObject pickObj;
    private Outline outline;
    private bool isWaitingFix;

    private void Awake() {
        pos = transform.position;
        //caixa = FindObjectOfType<CaixaEnergia>();
        pickObj = FindObjectOfType<PickupObject>();
    }

    private void OnTriggerEnter(Collider other) {
        Pickupable pipe = other.GetComponent<Pickupable>();
        if(pipe != null && pipe.gameObject.tag != "off"){
            if(pipe.name == name){
                pipe.GetComponent<Collider>().enabled = false;
                pickObj.dropObject();
                //offObject = pipe.gameObject;
                GetComponent<Collider>().isTrigger  = false;
                pipe.transform.position = pos;
                pipe.GetComponent<Rigidbody>().isKinematic = true;
                outline = pipe.GetComponent<Outline>();
                outline.OutlineColor = Color.red;
                isWaitingFix = true;
                pipe.enabled  = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void Fixed(){
        if(isWaitingFix){
            outline.enabled = false;
            FindObjectOfType<PipeManager>().LessOnePipe();
            Destroy(gameObject);
        }
    }
}
