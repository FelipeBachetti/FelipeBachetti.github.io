using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float damage,
    cooldownAttack, respawnDelay;

    public float initialEnemyHitPoints, enemyHitPoints;

    [SerializeField] private int respawns;

    private float cooldownAttackTimer;

    private bool dsDead = false;

    [SerializeField] private Transform respawner;

    private bool attacking, FacingRight;

    private GameObject player;

    [SerializeField] private GameObject alive;

    private Rigidbody2D rb;

    private Animator anim;

    public AIPath aiPath;

    public AIDestinationSetter target;

    [SerializeField] private bool hasWalk, hasAttack, hasDeath, isWarrior;
    [SerializeField] public bool deathShadow;
    [SerializeField] int dsReward;

    private void Awake() 
    {
        rb = alive.GetComponent<Rigidbody2D>();
        anim = alive.GetComponent<Animator>();
        enemyHitPoints = initialEnemyHitPoints;
        target.target = GameObject.FindWithTag("Player").transform;
        dsDead = false;
    }
    private void Update() 
    {
        if(aiPath.desiredVelocity.x >= 0.01 || aiPath.desiredVelocity.x <= -0.01){
            if (hasWalk){
                anim.SetBool("isWalking", true);
            }
        }
            else{
                if (hasWalk){
                    anim.SetBool("isWalking", false);
                }
            }

        if (((aiPath.desiredVelocity.x < 0) && FacingRight) || ((aiPath.desiredVelocity.x > 0) && !FacingRight))
        {
            FacingRight = !FacingRight;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }
        
        if (enemyHitPoints <= 0)
        {
            if (alive.name == "DeathShadow" && !dsDead)
            {
                FindObjectOfType<Currency>().AddMoney(dsReward);
                dsDead = true;
            }
            FindObjectOfType<AudioManager>().PlaySound("Inimigo_Morto_1");
            if (hasDeath){
                anim.SetBool("isDead", true);
            }
            Invoke("Dead", 1);
        }
        if (cooldownAttackTimer > 0)
        {
            cooldownAttackTimer -= Time.fixedDeltaTime;
        }
        if (attacking && cooldownAttackTimer<=0) 
            {
                FindObjectOfType<AudioManager>().PlaySound("Ataque_1");
                FindObjectOfType<PlayerMovement>().TakeDamage(damage);
                if (hasAttack){
                anim.SetBool("isAttacking", true);
                }
                cooldownAttackTimer = cooldownAttack;
                attacking = false;
                Invoke("attackAnim", 1f);
            }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player")
        {
            attacking = true;
        }
    }
    public void DecreaseHealth(float damage)
    {
        enemyHitPoints -= damage;
    }
    public float GetCurrentHealth()
    {
        return enemyHitPoints;
    }
    private void Dead(){
        if (respawns > 0)
        {
            enemyHitPoints = initialEnemyHitPoints;
            alive.SetActive(false);
            alive.transform.position = respawner.transform.position;
            Invoke("Respawn", respawnDelay);
        }else{
            Destroy(alive);
        }
    }
    private void Respawn()
    {
        alive.SetActive(true);
        respawns--;
    }

    public void Die()
    {
        FindObjectOfType<AudioManager>().PlaySound("Inimigo_Morto_1");
        if (hasDeath)
        {
            anim.SetBool("isDead", true);
        }
        Invoke("Dead", 1);
    }
    private void attackAnim(){
        anim.SetBool("isAttacking", false);
    }
}
