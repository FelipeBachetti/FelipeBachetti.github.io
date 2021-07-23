using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombo : MonoBehaviour
{
    public Combo[] combos;
    private Animator anim;
    private bool startCombo;
    public List<string> currentCombo;
    private float comboTimer;
    private Hit currentHit, nextHit;
    public bool canHit=true;
    private bool resetCombo=false;
    public MeleeAttack attack;
    public PlayerMovement pm;
    int i=0;
    
    // Start is called before the first frame update
    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }
    void CheckInputs()
    {
        if(pm.melee)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && !canHit)
            {
                resetCombo = true;
            }
            for (int i = 0; i < combos.Length; i++)
            {
                if (combos[i].hits.Length > currentCombo.Count)
                {
                    if (Input.GetButtonDown(combos[i].hits[currentCombo.Count].inputButton))
                    {
                        if (currentCombo.Count == 0)
                        {
                            PlayHit(combos[i].hits[currentCombo.Count]);
                            break;
                        }
                        else
                        {
                            bool comboMatch = false;
                            for (int n = 0; n < currentCombo.Count; n++)
                            {
                                if (currentCombo[n] != combos[i].hits[n].inputButton)
                                {
                                    comboMatch = false;
                                    break;
                                }
                                else
                                {
                                    comboMatch = true;
                                }
                            }
                            if (comboMatch && canHit)
                            {
                                nextHit = combos[i].hits[currentCombo.Count];
                                canHit = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (startCombo)
            {
                comboTimer += Time.deltaTime;
                if (comboTimer >= currentHit.animationTime && !canHit)
                {
                    PlayHit(nextHit);
                    if (resetCombo)
                    {
                        canHit = false;
                        CancelInvoke();
                        Invoke("ResetCombo", currentHit.animationTime);
                    }
                }

                if (comboTimer >= currentHit.resetTime)
                {
                    ResetCombo();
                }
            }
        }
    }

    void PlayHit(Hit hit)
    {
        if (i==0){
            FindObjectOfType<AudioManager>().PlaySound("Espadada_1");
            i++;
        }
        else if(i==1){
            FindObjectOfType<AudioManager>().PlaySound("Espadada_2");
            i++;
        }
        else if(i==2){
            FindObjectOfType<AudioManager>().PlaySound("Espadada_3");
            i=0;
        }
        comboTimer = 0;
        attack.SetAttack(hit);
        anim.Play(hit.animation);
        startCombo = true;
        currentCombo.Add(hit.inputButton);
        currentHit = hit;
        canHit=true;
    }
    void ResetCombo()
    {
        startCombo = false;
        comboTimer = 0;
        currentCombo.Clear();
        anim.Rebind();
        canHit=true;
    }
}
