using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float OriginalMoveSpeed, groundDrag, runSpeed;
    [SerializeField] Transform orientation;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Animator animator;

    float horizontalInput, verticalInput, playerHeight, crouchingMoveSpeed, moveSpeed, timer;
    bool grounded, isRunning;
    Vector3 moveDirection, originalPos;
    Rigidbody rb;
    private int count;

    public bool canStand, canRun, isCrouching;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = OriginalMoveSpeed;
        crouchingMoveSpeed = moveSpeed/1.5f;
        rb.drag = groundDrag;
        animator = GetComponent<Animator>();
        canStand = true;
        originalPos = transform.position;
    }

    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
         
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        if(!isCrouching){
            
            if(Input.GetButton("Sprint")){
                moveSpeed = runSpeed;
                isRunning = true;
            }else{
                moveSpeed = OriginalMoveSpeed;
                isRunning = false;
            }
        }

        if((Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0) && (Mathf.Abs(originalPos.x - transform.position.x) > .1f || Mathf.Abs(originalPos.z - transform.position.z) > .1f))
            Passos();

        originalPos = transform.position;

        if(Input.GetButtonDown("Crouch") && !isCrouching){
            animator.SetBool("crouch", true);
            isCrouching = true;
            moveSpeed = crouchingMoveSpeed;
        }
        else if(Input.GetButtonDown("Crouch") && isCrouching && canStand){
            animator.SetBool("crouch", false);
            isCrouching = false;
            moveSpeed = OriginalMoveSpeed;
        }

        if(timer>0)
            timer-=Time.deltaTime;
    }

    private void FixedUpdate() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

    void Passos(){
        if(timer<=0){
            count = newCount();
            if(isRunning)
                timer = FindObjectOfType<AudioManager>().Play(count.ToString(), 1.1f) - .2f;
            else
                timer = FindObjectOfType<AudioManager>().Play(count.ToString(), 1.1f) - .2f;
        }
    }

    int newCount(){
        int i_;
        i_ = (int)Mathf.Abs(Random.Range(-0.5f,3.5f));
        if(count == i_)
            return newCount();
        else
            return i_;
    }
}
