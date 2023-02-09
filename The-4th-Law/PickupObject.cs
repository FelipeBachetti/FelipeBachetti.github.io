using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private GameObject mainCamera;
	private bool carrying;
	private GameObject carriedObject;
    private Transform GUITool;
	private PlayerMovement playerMovement;

	[SerializeField] float distance;
	[SerializeField] float smooth;
    [SerializeField] Texture2D point;
    [SerializeField] float pointSizeX, pointSizeY;
    [SerializeField] Transform handPos;

	void Awake () {
		mainCamera = GameObject.FindWithTag("MainCamera");
		playerMovement = FindObjectOfType<PlayerMovement>();
	}
	
	
	void Update () {
		if(carrying) {
			carry(carriedObject);
			checkDrop();
		} else {
			pickup();
		}
	}
	
	void carry(GameObject obj) {
		if(obj.GetComponent<Pickupable>().isTool)
			obj.transform.position = Vector3.Lerp (obj.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
		else{
			obj.GetComponent<Rigidbody>().velocity = (handPos.position - obj.transform.position) * smooth;
			playerMovement.canRun = false;
		}
	}
	
	void pickup() {
		if(Input.GetButtonDown("Grab")) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;
			
			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				Pickupable p = hit.collider.GetComponent<Pickupable>();
				if(p != null) {
                    //p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    if(p.isTool){
                        p.gameObject.SetActive(false);
                        GUITool = GameObject.Find(p.name).transform;
                        foreach( Transform child in GUITool){
                            child.gameObject.SetActive(true);
                        }
                    }else{
						Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
						rb.angularDrag = 10f;
						rb.interpolation = RigidbodyInterpolation.Interpolate;
						rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
						rb.useGravity = false;
						rb.isKinematic = false;
                    }
					FindObjectOfType<CameraController>().cameraLimitation = 45f;
					FindObjectOfType<AudioManager>().Play("item");
					carrying = true;
					carriedObject = p.gameObject;
				}
			}
		}
	}
	
	void checkDrop() {
		if(Input.GetButtonDown("Grab")) {
			dropObject();
		}
	}
	
	public void dropObject() {
		playerMovement.canRun = true;
		carrying = false;
		//carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        if(carriedObject.GetComponent<Pickupable>().isTool){
            carriedObject.SetActive(true);
            foreach( Transform child in GUITool){
                child.gameObject.SetActive( false );
            }
        }
        else{
			Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
			rb.angularDrag = .1f;
			rb.interpolation = RigidbodyInterpolation.None;
			rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
			rb.useGravity = true;
        } 
		FindObjectOfType<CameraController>().cameraLimitation = 90f;   
		carriedObject = null;
	}

    private void OnGUI() {
        GUI.DrawTexture(new Rect(Screen.width/2, Screen.height/2, pointSizeX, pointSizeY), point);
    }
}
