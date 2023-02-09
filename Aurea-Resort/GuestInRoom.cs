using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuestInRoom : GuestState
{
    private float _currentInRoomCooldown;
    private float _satisfactionIncrease;
    private float currentCooldown = 0f;
    private Transform destinationInRoom;
    private List<Transform> roomSquares = new List<Transform>();
    private float sleepingTime;

    public GuestInRoom(GameObject Guest, int TaskNummer, List<EnumReview> DailyReview, 
    float SatisfactionModifier, Transform destination, PersonTracker bedroom,
    float movementCooldown, float WalkingStateCooldown, List<EnumGuestActivity> activityList,
    float PatienceCooldown, float SatisfactionModifierDecrease, float InRoomCooldown, 
    NavMeshAgent agent, float GeneralSatisfaction, EnumGuestActivity activity, int FoodConsumption, float MaxPayment):base(Guest, TaskNummer, 
    DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
    activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment){
        this.State= EnumGuestState.INROOM;
    }


    public override void Enter(){
        destination.GetComponent<BuildingDetails>().guestsInRoom.Add(Guest);
        Guest.GetComponent<CapsuleCollider>().isTrigger = true;
        this._satisfactionIncrease = 100/(TaskNummer-1);
        this._currentInRoomCooldown = InRoomCooldown;
        this.StateStage = EnumGuestStateStage.ACTION;
    }

    public override void Action(){
        if (_currentInRoomCooldown>0 && activity != (EnumGuestActivity)4)
        {
            if(agent!=null && currentCooldown <= 0){
                if(activity != (EnumGuestActivity)2){
                    walkRandomly();
                }else{
                    agent.SetDestination(destination.position);
                }         
            }else{
                currentCooldown -= Time.deltaTime;
            }
            this.StateStage = EnumGuestStateStage.ACTION;
            _currentInRoomCooldown -= Time.deltaTime;
        }else if(activity == (EnumGuestActivity)4 && _currentInRoomCooldown > -15f){
            agent.SetDestination(bedroom.transform.position);
            _currentInRoomCooldown -= Time.deltaTime;
            this.StateStage = EnumGuestStateStage.ACTION;
        }
        else
        {
            this.StateStage = EnumGuestStateStage.CHANGESTATE; 
        }
        

    }

    public override void ChangeState(){
        if(activity == (EnumGuestActivity)3){
            if(ResourcesManager.Instance.food >= FoodConsumption){
                ResourcesManager.Instance.food -= FoodConsumption;
            }else{
                this.DailyReview[DailyReview.Count - 1] = EnumReview.PESSIMO;
                SatisfactionModifier = 0;
            }
        }

        destination.GetComponent<BuildingDetails>().guestsInRoom.Remove(Guest);

        if(activity == (EnumGuestActivity)4){
            Pay();
            DailyReview.Clear();
            activityList = Guest.GetComponent<Guest>().newGuestDay();
            this.NextState = new GuestWalking(Guest, TaskNummer, 
            DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
            activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment);
        }else{
            // Atualizar a satisfação de acordo com o modificador e quantidade de tarefas e manter enquanto o cooldown
            this.GeneralSatisfaction += _satisfactionIncrease * SatisfactionModifier;
            // Volta a mais um ciclo
            this.SatisfactionModifier = 1;
            this.NextState = new GuestWalking(Guest, TaskNummer, 
            DailyReview, SatisfactionModifier,destination, bedroom, movementCooldown, WalkingStateCooldown, 
            activityList, PatienceCooldown, SatisfactionModifierDecrease, InRoomCooldown, agent, GeneralSatisfaction, activity, FoodConsumption, MaxPayment);
        }
        
    }

    private void walkRandomly(){
        currentCooldown = movementCooldown;
        roomSquares.Clear();
        roomSquares = destination.GetComponent<BuildingDetails>().RoomSquares();
        int rand = Random.Range(0, roomSquares.Count);
        agent.SetDestination(new Vector3(roomSquares[rand].position.x + Random.Range(-1f,1f), destination.position.y, roomSquares[rand].position.z + Random.Range(-1f,1f)));
    }

    private void Pay(){
        ResourcesManager.Instance.money += (int)(MaxPayment * GeneralSatisfaction/100);
        GeneralSatisfaction = 0;
    }
}
