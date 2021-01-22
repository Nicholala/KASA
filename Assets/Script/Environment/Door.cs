using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("属性")]
    public int health;
    public float flashtime;
    public Rigidbody2D Rig;

    public SpriteRenderer sr;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakenDamage(int damage)
    {
        health -= damage;
        FlashColor(flashtime);
        GameController.cameraShake.Shake();
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }
}
