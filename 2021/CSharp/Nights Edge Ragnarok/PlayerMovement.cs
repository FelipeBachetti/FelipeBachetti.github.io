using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    public bool melee;
    public Bow bow;

    [Header("Mana and health")]
    public HealthBar healthBar;
    public ManaBar manaBar;
    [SerializeField] float initialPlayerHitPoints;
    public float playerMana, playerHitPoints;
    public float manaRegen;
    [SerializeField] float initialRegenTimer=2f;
    private float regenTimer;
    private bool isLosingMana;

    [Header("Horizontal vars")]
    [SerializeField] float speed = 100f;
    [SerializeField] float dampeningTime = 0.15f;
    private float xmovement;
    public Rigidbody2D rb2D;
    public bool FacingRight = true;
    private float currentSpeed = 0f;

    [Header("Vertical vars")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] float jumpForce;
    [SerializeField] float Radius;
    [SerializeField] LayerMask whatIsGround;
    public bool grounded;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool stoppedJumping;

    [Header("Dash Vars")]
    [SerializeField] float dashSpeed;
    [SerializeField] float startDashTime;
    private float dashTime;
    private bool isDashing = false;
    private int dashDir;
    public float dashManaCost = 0.15f;

    [Header("Specials")]
    public MeleeAttack attack;
    public bool fogo;
    [SerializeField]
    ExplosaoBachetiana explosaoCode;
    public bool explosao;
    public bool execute = false;
    public bool bencaoCura = false;
    public bool espetada = false;
    public float descontoDeMana = 0;

    private void Awake()
    {
        melee = true;
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
        dashTime = startDashTime;
        ResetHealth();
        playerMana = 1f;
        manaBar.SetMaxMana(playerMana);
    }

    private void Update()
    {
        playerMana = manaBar.GetCurrentMana();
        if(playerHitPoints<=0){
            FindObjectOfType<DeathCounter>().Died();
        }
        
        //jump
        grounded = Physics2D.OverlapCircle(GroundCheck.position, Radius, whatIsGround);
        xmovement = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.C))
        {
            melee = !melee;
            bow.gameObject.SetActive(!bow.gameObject.activeSelf);
        }
        if (Input.GetAxisRaw("Horizontal") != 0 && grounded)
        {
            anim.SetBool("isMoving", true);
        }
        if (grounded)
        {
            jumpTimeCounter = jumpTime;
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isMoving", false);
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            stoppedJumping = false;
        }

        if (Input.GetButton("Jump") && !stoppedJumping && jumpTimeCounter > 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
        if (Input.GetButtonDown("Dash") && playerMana>=0.2f)
        {
            anim.SetBool("Dash", true);
            isDashing = true;
            rb2D.velocity = Vector2.zero;
            dashTime = startDashTime;
            rbMass();
        }
        if (FacingRight)
        {
            dashDir = 1;
        }
        else
        {
            dashDir=-1;
        }
        if (isDashing)
        {
            if (dashTime>0)
            {
                LoseMana(dashManaCost);
                rb2D.velocity = transform.right*dashSpeed*dashDir;
                dashTime -= Time.deltaTime;
            }
            else
            {
                isDashing = false;
                anim.SetBool("Dash", false);
            }   
        }
        if (playerMana < manaBar.GetMaxMana() && regenTimer<=0f){
            playerMana += manaRegen * Time.deltaTime;
            manaBar.SetMana(playerMana);
        }
        if (isLosingMana){
            regenTimer -= Time.fixedDeltaTime;
        }

        if (Input.GetButtonDown("FireSpecial"))
        {
            SpecialMove();
        }
    }
    private void FixedUpdate() 
    {
        //Horizontal movement
        if (!Mathf.Approximately(xmovement, 0f) && !isDashing)
        {
            rb2D.velocity = new Vector2(xmovement*speed, rb2D.velocity.y);
            Flip (xmovement);
        }
        else
        {
            StopBody();
            anim.SetBool("isMoving", false);
        }
    }
    private void Flip(float horizontal)
    {
        if (((horizontal < 0) && FacingRight) || ((horizontal > 0) && !FacingRight))
        {
            FacingRight = !FacingRight;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }
    }
    protected void StopBody ()
    {
        Vector2 dampenedVelocity = new Vector2(Mathf.SmoothDamp(rb2D.velocity.x, 0f, ref currentSpeed, dampeningTime), rb2D.velocity.y);
        rb2D.velocity = dampenedVelocity;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(GroundCheck.position, Radius);
    }
    public void TakeDamage(float damage)
    {
        FindObjectOfType<DamageToPlayer>().TakeDamage(damage);
        playerHitPoints -= damage * Time.fixedDeltaTime;
        healthBar.SetHealth(playerHitPoints);
    }
    public void LoseMana(float lostMana)
    {
        playerMana -= lostMana;
        regenTimer = initialRegenTimer;
        isLosingMana = true;
        manaBar.SetMana(playerMana);
    }
    public void rbMass(){
        rb2D.mass = 100;
        Debug.Log("1");
        Invoke("rbNormalMass", 1f);
    }
    public void rbNormalMass()
    {
        rb2D.mass = 1;
        Debug.Log("2");
    }

    public void SpecialMove()
    {
        if (espetada && (playerMana >= 1 - descontoDeMana) /* && currenteMana = manaNecessitadaProGolpe - descontoDeMana */)
        {
            StartCoroutine(SetEspetada());
            LoseMana(1 - descontoDeMana);
        }else if (fogo && (playerMana >= 0.3 - descontoDeMana) /* && currenteMana = manaNecessitadaProGolpe - descontoDeMana */)
        {
            StartCoroutine(SetFogo());
            LoseMana(0.3f - descontoDeMana);
        }
        else if (explosao && (playerMana >= 1 - descontoDeMana) /* && currenteMana = manaNecessitadaProGolpe - descontoDeMana */)
        {
            StartCoroutine(SetExplosion());
            LoseMana(1 - descontoDeMana);
        }
        else if (execute && (playerMana >= manaBar.GetMaxMana() - descontoDeMana) /* && currenteMana = manaNecessitadaProGolpe - descontoDeMana */)
        {
            StartCoroutine(SetExecute());
            LoseMana(manaBar.GetMaxMana() - descontoDeMana);
        }
        else if (bencaoCura && (playerMana >= 0.5 - descontoDeMana) /* && currenteMana = manaNecessitadaProGolpe - descontoDeMana */)
        {
            float max = healthBar.GetMaxHealth();
            float now = healthBar.GetCurrentHealth();
            healthBar.SetHealth((float)(now + max * 0.25));
            LoseMana(0.5f - descontoDeMana);
        }
    }

    IEnumerator SetEspetada()
    {
        attack.espetada = true;
        yield return new WaitForSeconds(0.5f);
        attack.espetada = false;
    }

    IEnumerator SetFogo()
    {
        attack.fogo = true;
        yield return new WaitForSeconds(0.5f);
        attack.fogo = false;
    }

    IEnumerator SetExecute()
    {
        attack.execute = true;
        yield return new WaitForSeconds(0.5f);
        attack.execute = false;
    }

    IEnumerator SetExplosion()
    {
        explosaoCode.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        explosaoCode.gameObject.SetActive(false);
    }
    public void ResetHealth(){
        playerHitPoints = initialPlayerHitPoints;
        healthBar.SetMaxHealth(playerHitPoints);
    }
    public void ExtraHitPoints(){
        initialPlayerHitPoints += .25f;
        playerHitPoints = initialPlayerHitPoints;
        healthBar.SetMaxHealth(playerHitPoints);
    }
}
