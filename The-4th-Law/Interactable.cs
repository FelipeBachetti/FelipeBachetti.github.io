using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private GameObject mainCamera;

    private void Awake() {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void Update()
    {
       if(Input.GetMouseButtonDown(0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;
			
			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
                GeniusClick click = hit.collider.GetComponent<GeniusClick>();
				if(click != null)
					click.MouseClick(); 
                AudioLog audioClick = hit.collider.GetComponent<AudioLog>();
                if(audioClick != null)
					audioClick.Play(); 
            }
        }
    }
}
