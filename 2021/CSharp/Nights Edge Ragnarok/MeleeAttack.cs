using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private float damage = 1f;
    private bool slowDown;
    private AudioClip hitSound;
    [SerializeField]
    GameObject spear;
    public bool execute = false;
    public bool fogo = false;
    public bool espetada = false;
    public MultiplicadorDano extraDam;
    public float manaRecover = 0.05f;
    public ManaBar mana;

    public void SetAttack(Hit hit)
    {
        damage = hit.damage * extraDam.dam;
        slowDown = hit.slowDown;
        hitSound = hit.hitSound;
    }

    private void Update()
    {
        if (espetada)
        {
            StartCoroutine(PlayEspetada());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Damage enemy = other.GetComponent<Damage>();
        EnemyController enemyCont = other.GetComponent<EnemyController>();
        if (other.tag == "Enemy"){
            if (enemy != null)
            {
                if (enemyCont.deathShadow)
                {
                    if (other.GetComponent<CircleCollider2D>() == other)
                    {
                        mana.SetMana(mana.GetCurrentMana() + manaRecover * 2);
                        enemy.TakeDamage(damage * 2);
                        enemyCont.DecreaseHealth(damage * 2);
                    }
                    else
                    {
                        mana.SetMana(mana.GetCurrentMana() + manaRecover);
                        enemy.TakeDamage(damage);
                        enemyCont.DecreaseHealth(damage);
                        if (fogo)
                        {
                            if (other.GetComponent<FogoRezhendia>() == null)
                            {
                                FogoRezhendia dps = other.gameObject.AddComponent<FogoRezhendia>();
                                dps.damage = (float)(2 + 0.1 * damage);
                                dps.applyEveryNSeconds = 1f;
                                dps.applyDamageNTimes = 3;
                                dps.delay = 1f;
                            }
                        }
                    }
                }
                else
                {
                    mana.SetMana(mana.GetCurrentMana() + manaRecover);
                    enemy.TakeDamage(damage);
                    enemyCont.DecreaseHealth(damage);
                    if (enemyCont.enemyHitPoints <= enemyCont.initialEnemyHitPoints * 0.3 && execute)
                    {
                        enemyCont.Die();
                    }
                    if (fogo)
                    {
                        if (other.GetComponent<FogoRezhendia>() == null)
                        {
                            FogoRezhendia dps = other.gameObject.AddComponent<FogoRezhendia>();
                            dps.damage = (float)(2 + 0.1 * damage);
                            dps.applyEveryNSeconds = 1f;
                            dps.applyDamageNTimes = 3;
                            dps.delay = 1f;
                        }
                    }
                }
                //FindObjectOfType<EnemyController>().DecreaseHealth(damage);
                if (slowDown)
                {
                    SlowDown.instance.SetSlowDown();
                }
            }
        }
    }

    IEnumerator PlayEspetada()
    {
        yield return new WaitForSeconds(0.15f);
        spear.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        spear.SetActive(false);
    }

}
