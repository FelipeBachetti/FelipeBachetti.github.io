using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnumGuestState
{
    WALKING, WALKINGTOROOM, INLINE, INROOM
}

public enum EnumGuestStateStage 
{
    ENTER, ACTION, CHANGESTATE
}

public enum EnumGuestActivity
{
    NULL, ENTERTAINMENT, BATHROOM, FOOD, SLEEP
}

public abstract class GuestState
{
    #region Funcionamento da maquina de estados
        protected EnumGuestState State;
        protected EnumGuestStateStage StateStage;
        // definido em alguma função antes de ser chamado
        protected GuestState NextState;
    #endregion

    #region Construção do Hóspede
        //objeto que armazena o objeto da classe
        protected GameObject Guest;
        // Valor de salas passadas no dia
        protected int TaskNummer;
        // Reviews diárias sobre cada quarto que passa
        public List<EnumReview> DailyReview;
        // Valor do modificador dos acréscimos de satisfação por quarto
        [Range(0.0f, 100.0f)]
        public float GeneralSatisfaction;
        // Valor do modificador dos acréscimos de satisfação por quarto
        protected float SatisfactionModifier = 1;
        //Lista de tarefas diárias
        public List<EnumGuestActivity> activityList;
        public EnumGuestActivity activity;
        public float MaxPayment;
        
    #endregion

    #region Variaveis movimento
        public Builder builder;
        public NavMeshAgent agent;
        protected Transform destination;
        protected bool rotate;
        public PersonTracker bedroom; 
    #endregion

    #region Variaveis GuestWalking
        protected float currentCooldown;
        protected float movementCooldown;
        protected float previousRemainingDistance;

        // Duração do cooldown para a troca de estado
        protected float WalkingStateCooldown;
        protected float StateCurrentCooldown;
    #endregion

    #region Variaveis GuestWalkingToRoom
    
        
    #endregion

    #region Variaveis GuestInLine
        protected float PatienceCooldown;
        protected float SatisfactionModifierDecrease;
    #endregion

    #region Variaveis GuestInRoom
        protected float InRoomCooldown;
        protected int FoodConsumption;
    #endregion

    // Construtor que manterá/comunicará os dados entre as trocas de estado
    public GuestState(GameObject Guest, int TaskNummer, List<EnumReview> DailyReview, 
    float SatisfactionModifier, Transform destination, PersonTracker bedroom,
    float movementCooldown, float WalkingStateCooldown, List<EnumGuestActivity> activityList,
    float PatienceCooldown, float SatisfactionModifierDecrease, float InRoomCooldown, 
    NavMeshAgent agent, float GeneralSatisfaction, EnumGuestActivity activity, int FoodConsumption, float MaxPayment){
        this.StateStage = EnumGuestStateStage.ENTER;

        this.Guest = Guest;
        this.TaskNummer = TaskNummer;
        this.DailyReview = DailyReview;
        this.SatisfactionModifier = SatisfactionModifier;
        this.destination = destination;
        this.bedroom = bedroom;
        this.movementCooldown = movementCooldown;
        this.WalkingStateCooldown = WalkingStateCooldown;
        this.activityList = activityList;
        this.PatienceCooldown = PatienceCooldown; 
        this.SatisfactionModifierDecrease = SatisfactionModifierDecrease;
        this.InRoomCooldown = InRoomCooldown;
        this.agent = agent;
        this.GeneralSatisfaction = GeneralSatisfaction;
        this.activity = activity;
        this.FoodConsumption = FoodConsumption;
        this.MaxPayment = MaxPayment;
    }

    public GuestState StateProcess(){
        if(this.StateStage == EnumGuestStateStage.ENTER){
            Enter();
        }
        else if(this.StateStage == EnumGuestStateStage.ACTION)
        {
            Action();
        }
        else
        {
            ChangeState();
            return this.NextState;      
        }

        return this;
    }

    public virtual void Enter(){
        this.StateStage = EnumGuestStateStage.ACTION;
    }

    public virtual void Action(){
        this.StateStage = EnumGuestStateStage.ACTION;
        /*when done
        this.StateStage = EnumGuestStateStage.CHANGESTATE;
        */
    }

    public virtual void ChangeState(){

    }


    public EnumGuestState getState(){
        return this.State;
    }

}
