using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class guestMovement : MonoBehaviour
{
    private Builder builder;
    private NavMeshAgent agent;
    private bool moveRandomly=true;
    private float currentCooldown;
    private Transform destination;
    private float previousRemainingDistance;
    private float distToGround;
    private bool rotate;

    public PersonTracker bedroom;

    [SerializeField] float movementCooldown;

    private void Start() {
        builder = FindObjectOfType<Builder>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(moveRandomly && agent!=null && Mathf.Abs(previousRemainingDistance - agent.remainingDistance)<=.1f){
            walkRandomly();
        }

        if(rotate){
            transform.Rotate(new Vector3(Random.Range(20f, 30f),0f,Random.Range(20f, 30f))*Time.deltaTime);
        }
    }

    private void walkRandomly(){
        if(currentCooldown<=0){
            currentCooldown = movementCooldown;
            destination = builder.corridorList[Random.Range(0, builder.corridorList.Count)];
            agent.SetDestination(new Vector3(destination.position.x + Random.Range(-1,1), destination.position.y, destination.position.z + Random.Range(-1,1)));
        }else{
            currentCooldown -= Time.deltaTime;
        }
    }

    public void killGuest(){
        Destroy(agent);
        agent = null;
        rotate = true;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-50,50), Random.Range(-50,50), Random.Range(-50,50)), ForceMode.Force);
        StartCoroutine(fadeGuest());
    }

    IEnumerator fadeGuest(){
        for (float i = 1; i >= 0; i -= Time.deltaTime/5)
            {
                gameObject.transform.localScale = new Vector3(i,i,i);
                yield return null;
            }
        bedroom.spawn(true);
        Destroy(gameObject);
    }
}
