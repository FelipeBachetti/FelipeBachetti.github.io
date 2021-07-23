using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    public Color damageColor;
    public float damageTime = 0.1f;
    public GameObject damageText;
    public Transform damageTextPos;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    public void TakeDamage(float damage)
    {
        spriteRenderer.color = damageColor;
        Invoke("ReleaseDamage", damageTime);
        GameObject newDamageText = Instantiate(damageText, damageTextPos.position, Quaternion.identity);
        newDamageText.GetComponentInChildren<Text>().text = damage.ToString();
        Destroy(newDamageText, 1); 
    }

    public void ReleaseDamage()
    {
        spriteRenderer.color = defaultColor;
    }
}
