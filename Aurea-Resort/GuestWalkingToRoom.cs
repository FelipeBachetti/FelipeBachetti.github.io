using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuestWalkingToRoom : GuestState
{
    public GuestWalkingToRoom(GameObject Guest, int TaskNummer, List<EnumReview> DailyReview, 
    float SatisfactionModifier, Transform destination, PersonTracker bedroom,
    float movementCooldown, float WalkingStateCooldown, List<EnumGuestActivity> activityList,
    float PatienceCooldown, float SatisfactionModifierDecrease, float InRoomCooldown, 
    NavMeshAgent agent, float GeneralSatisfaction, EnumGuestActivity activity, int FoodConsumption, float MaxPayment):base(Guest, TaskNummer, 
    DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
    activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment){
        this.State= EnumGuestState.WALKINGTOROOM;
    }


    public override void Enter(){
        this.StateStage = EnumGuestStateStage.ACTION;
        agent.SetDestination(destination.GetComponent<BuildingDetails>().corridor.transform.position);
    }

    public override void Action(){
        this.StateStage = EnumGuestStateStage.ACTION;

        // Se o player travar
        if (agent.velocity == Vector3.zero)
        {
            agent.ResetPath();
            this.StateStage = EnumGuestStateStage.CHANGESTATE;
        }
        
        if(Vector3.Distance(Guest.transform.position, destination.GetComponent<BuildingDetails>().corridor.transform.position) < 2f)
            this.StateStage = EnumGuestStateStage.CHANGESTATE;
    }

    public override void ChangeState(){
        if (agent.velocity == Vector3.zero)
        {
            // Se está travado por não ter acesso ao local
            this.DailyReview.Add(EnumReview.PESSIMO);
            this.NextState = new GuestWalking(Guest, TaskNummer, 
            DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
            activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment); 
        }else{
            this.NextState = new GuestInLine(Guest, TaskNummer, 
            DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
            activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment);
        }

        
    }

}
