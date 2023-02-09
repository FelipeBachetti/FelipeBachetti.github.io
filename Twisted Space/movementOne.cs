using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementOne : MonoBehaviour
{   
    public Rigidbody2D rb;
    public double speed;
    private Generator gen;
    float vertical;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        gen = FindObjectOfType<Generator>();
        gen.onePos = transform.root.position;
        transform.position = transform.root.position;
    }

    private void Update() {
        vertical = Input.GetAxisRaw("Vertical"); 
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(rb.velocity.x, vertical*((float)speed)*Time.deltaTime);
    }
}
