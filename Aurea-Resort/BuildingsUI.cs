using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsUI : MonoBehaviour
{
    public Animator anim;
    public Color color;

    private bool isShowing;

    private Builder builder;

    private void Awake() {
        builder = FindObjectOfType<Builder>();
    }

    private void Update() {
        if(isShowing && color.a<=1f || !isShowing && color.a>=0f){
            foreach(GameObject p in builder.planeList)
                if(p!=null)
                    p.GetComponent<Renderer>().material.color = color;
        }
    }
    
    public void showBuildings(){
        if(!isShowing){
            StartCoroutine(FadePlane(false));
            anim.SetBool("show", true);
            isShowing = true; 
        }else{
            StartCoroutine(FadePlane(true));
            anim.SetBool("show", false);
            isShowing = false;
        }
    }

    IEnumerator FadePlane(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = .6f; i >= 0; i -= Time.deltaTime)
            {
                color = new Color(color.r, color.g, color.b, i);
                yield return null;
            }
            color = new Color(color.r, color.g, color.b, 0f);
        }
        else
        {
            for (float i = 0; i <= .6; i += Time.deltaTime)
            {
                color = new Color(color.r, color.g, color.b, i);
                yield return null;
            }
            color = new Color(color.r, color.g, color.b, 1f);
        }
    }

    public void initialCheck(){
        GameObject[] go = GameObject.FindGameObjectsWithTag("plane"); 
        foreach(GameObject g in go)
            g.GetComponent<Renderer>().material.color = color;
    }
}
