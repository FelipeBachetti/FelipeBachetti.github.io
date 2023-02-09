using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaveDeFenda : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;

    void Update()
    {
       if(Input.GetMouseButtonDown(0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;
			
			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
                Prego prego = hit.collider.GetComponent<Prego>();
				if(prego != null)
					prego.Remove();  
            }
        }
    }
}
