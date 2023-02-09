using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuestInLine : GuestState
{
    private float currentPatienceCooldown;


    public GuestInLine(GameObject Guest, int TaskNummer, List<EnumReview> DailyReview, 
    float SatisfactionModifier, Transform destination, PersonTracker bedroom,
    float movementCooldown, float WalkingStateCooldown, List<EnumGuestActivity> activityList,
    float PatienceCooldown, float SatisfactionModifierDecrease, float InRoomCooldown, 
    NavMeshAgent agent, float GeneralSatisfaction, EnumGuestActivity activity, int FoodConsumption, float MaxPayment):base(Guest, TaskNummer, 
    DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
    activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment){
        this.State= EnumGuestState.INLINE;
    }


    public override void Enter(){
        Guest.GetComponent<CapsuleCollider>().isTrigger = false;
        currentPatienceCooldown = PatienceCooldown;
        this.StateStage = EnumGuestStateStage.ACTION;
        SatisfactionModifier = 1f;
    }

    public override void Action(){
        if(destination.GetComponent<BuildingDetails>().hasSpace(Guest)){
            this.StateStage = EnumGuestStateStage.CHANGESTATE;
            return;
        }else if(SatisfactionModifier <= 0){
            SatisfactionModifier = 0;
            
        }else if(currentPatienceCooldown>0){
            currentPatienceCooldown -= Time.deltaTime;
            
        }else{
            SatisfactionModifier -= SatisfactionModifierDecrease;
            currentPatienceCooldown = PatienceCooldown;
            
        }
        this.StateStage = EnumGuestStateStage.ACTION;
        

    }

    public override void ChangeState(){
        if(SatisfactionModifier > 0.85){
            this.DailyReview.Add(EnumReview.MUITOBOM);
        }else if(SatisfactionModifier > 0.7){
            this.DailyReview.Add(EnumReview.BOM);
        }else if(SatisfactionModifier > 0.55){
            this.DailyReview.Add(EnumReview.MEDIANO);
        }else if(SatisfactionModifier > 0.4){
            Debug.Log("check");
            this.DailyReview.Add(EnumReview.RUIM);
        }else{
            this.DailyReview.Add(EnumReview.PESSIMO);
        }

        // Atualizar a UI

        // Se o quarto possui vaga
        Guest.GetComponent<CapsuleCollider>().isTrigger = false;
        this.NextState = new GuestInRoom(Guest, TaskNummer, 
        DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
        activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment);
    }
}
