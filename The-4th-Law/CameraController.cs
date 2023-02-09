using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float senX, senY;
    public Transform orient;
    private float xRot, yRot;

    public float cameraLimitation = 90f;
    public bool canLook;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canLook = true;
    }

    void Update()
    {
        if(canLook){
            float mouseX = Input.GetAxisRaw("Mouse X") * senX * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * senY * Time.deltaTime;

            yRot += mouseX;
            xRot -= mouseY;

            xRot = Mathf.Clamp(xRot, -90, cameraLimitation);


            transform.rotation = Quaternion.Euler(xRot, yRot, 0);
            orient.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }
}
