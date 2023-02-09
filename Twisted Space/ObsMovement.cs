using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsMovement : MonoBehaviour
{   
    [SerializeField] private float destroyTime;
    private Rigidbody2D rb;
    public float speed, directionY = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(-speed*Time.fixedDeltaTime, speed*Time.fixedDeltaTime*directionY); 
    }

    private void Update() {
        Destroy(gameObject, destroyTime);
    }
}
