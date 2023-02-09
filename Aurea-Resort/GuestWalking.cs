using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class GuestWalking : GuestState
{
    private float StateCurrentCooldown;
    private float currentCooldown;
    private List<GameObject> list;

    // É necessário o contrutor
    public GuestWalking(GameObject Guest, int TaskNummer, List<EnumReview> DailyReview, 
    float SatisfactionModifier, Transform destination, PersonTracker bedroom,
    float movementCooldown, float WalkingStateCooldown, List<EnumGuestActivity> activityList,
    float PatienceCooldown, float SatisfactionModifierDecrease, float InRoomCooldown, 
    NavMeshAgent agent, float GeneralSatisfaction, EnumGuestActivity activity, int FoodConsumption, float MaxPayment):base(Guest, TaskNummer, 
    DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
    activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment){
        this.State= EnumGuestState.WALKING;
    }

    public override void Enter(){
        StateCurrentCooldown = WalkingStateCooldown;
        this.StateStage = EnumGuestStateStage.ACTION;
    }


    public override void Action(){

        if (StateCurrentCooldown>0 /*||  não existe outra tarefa no dia */)
        {
            if(agent!=null && Mathf.Abs(previousRemainingDistance - agent.remainingDistance)<=.1f){
                walkRandomly();
            }
            this.StateStage = EnumGuestStateStage.ACTION;
            StateCurrentCooldown -= Time.deltaTime;
        }
        else
        {
            if(findRoom()){
                this.StateStage = EnumGuestStateStage.CHANGESTATE; 
            }else{
                // Passar para a próxima tarefa

                
                StateCurrentCooldown = WalkingStateCooldown;
            }
        }
    }

    public override void ChangeState(){
        // Realizar a troca de estado para Walking to Room
        this.NextState = new GuestWalkingToRoom(Guest, TaskNummer, 
        DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
        activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment);
    }

    private void walkRandomly(){
        if(currentCooldown<=0){
            currentCooldown = movementCooldown;
            destination = Builder.Instance.corridorList[Random.Range(0, Builder.Instance.corridorList.Count)];
            agent.SetDestination(new Vector3(destination.position.x + Random.Range(-1,1), destination.position.y, destination.position.z + Random.Range(-1,1)));
        }else{
            currentCooldown -= Time.deltaTime;
        }
        previousRemainingDistance = agent.remainingDistance;
    }

    private bool findRoom(){
        activity = 0;
        for(int i = 0; i < activityList.Count; i++){
            if(activityList[i] != (EnumGuestActivity)0){
                activity = activityList[i];
                activityList[i] = 0;
                break;
            }
        }

        if(activity == (EnumGuestActivity)4){
            destination = bedroom.transform;
            bedroom.GetComponent<BuildingDetails>().guestQueue.Enqueue(Guest);
            return true;
        }
            
        List<BuildingDetails> list = new List<BuildingDetails>();
        BuildingDetails details;
        if(activity != 0){
            foreach(GameObject g in Builder.Instance.buildings){
                details = g.GetComponent<BuildingDetails>();
                if(details.function == activity)
                    list.Add(details);
            }
        }else{

            return false;
        }
        if(list.Count == 0){
            this.DailyReview.Add(EnumReview.PESSIMO);
            return false;
        }

        //ordena por tamanho da fila primeiro e depois por distancia (ainda precisa ser testado)
        list = list.OrderBy(q => q.guestQueue.Count).ThenBy(d => Vector3.Distance(Guest.transform.position, d.transform.position)).ToList();

        destination = list[0].transform;
        list[0].guestQueue.Enqueue(Guest);

        return true;
    }
}
