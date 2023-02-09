using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour
{
    #region Hóspede
        private GuestState _currentState;
    #endregion

    #region Construção do Hóspede
        [Header("construção de hóspede")]
        // Valor de salas passadas no dia
        [SerializeField] private int _taskNummer;

        private GameObject _guest;
        // Reviews diárias sobre cada quarto que passa
        public List<EnumReview> _dailyReview = new List<EnumReview>();
        // Valor da satisfação geral do hóspede
        [Range(0.0f, 100.0f)]
        public float _generalSatisfaction = 0f;
        // Valor do modificador dos acréscimos de satisfação por quarto
        private float _satisfactionModifier = 1;
        public List<EnumGuestActivity> activityList;
        public EnumGuestActivity activity;
        public float _maxPayment;
    #endregion

    #region Variaveis movimento
        public PersonTracker bedroom;
        private Builder builder;
        private UnityEngine.AI.NavMeshAgent agent;
        private Transform destination;
        private bool rotate;
    #endregion
    
    #region Variaveis GuestWalking
        [Header("Estado (andando aleatoriamente)")]
        // Duração do cooldown para a troca de estado
        [SerializeField] private float WalkingStateCooldown;
        // Duração do cooldown para um novo movimento
        [SerializeField] private float movementCooldown;
        
        private float previousRemainingDistance;
        private float distToGround;
        
    #endregion

    #region Variaveis GuestInLine
        [Header("construção de hóspede")]
        [SerializeField] private float _patienceCooldown;
        [Range(0f,1f)]
        [SerializeField] private float _satisfactionDecreaseModifier;
    #endregion

    #region Variaveis GuestInRoom
        [SerializeField] private float _inRoomCooldown;
        [SerializeField] private int _foodConsumption;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Prepara as variáveis necessárias para serem inicializadas
        _guest = gameObject;

        GuestBuilder();
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentState != null){
            _currentState = _currentState.StateProcess();
            _generalSatisfaction = _currentState.GeneralSatisfaction;
            _dailyReview = _currentState.DailyReview;
            activityList = _currentState.activityList;
            activity = _currentState.activity;

            if(rotate){
                gameObject.transform.Rotate(new Vector3(Random.Range(20f, 30f),0f,Random.Range(20f, 30f))*Time.deltaTime);
            }
        }
    }

    public void GuestBuilder(){
        // Define as funções aleatórias de um hóspede a ser criado
        // tarefas diárias

        _currentState = null;

        activityList = newGuestDay();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        _currentState = new GuestWalking(_guest, _taskNummer, _dailyReview, _satisfactionModifier,
        destination, bedroom, movementCooldown, WalkingStateCooldown, activityList, _patienceCooldown,
        _satisfactionDecreaseModifier, _inRoomCooldown, agent, _generalSatisfaction, activity, _foodConsumption, _maxPayment);

    }

    public List<EnumGuestActivity> newGuestDay(){
        List<EnumGuestActivity> activityListAux = new List<EnumGuestActivity>();
        EnumGuestActivity previous = 0;
        for(int i = 0; i < _taskNummer; i++){
            EnumGuestActivity r;
            if(i == _taskNummer - 1)
                r = (EnumGuestActivity)4;
            else
                r = (EnumGuestActivity)Random.Range(1,4);
                
            if(previous == r){
                i--;
            }else{
                previous = r;
                activityListAux.Add(r);
            }
        }

        return activityListAux;
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

    private void OnMouseDown() {
        CameraController.Instance.SetFollow(transform);
    }
}
