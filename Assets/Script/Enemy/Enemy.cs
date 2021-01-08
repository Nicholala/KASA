using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("属性")]
    public int health;
    public int Attackdamage;
    public float flashtime;

    public SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        originalColor = sr.color;
        Initialize();
    }

    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }

    protected virtual void Initialize()
    {

    }
    protected virtual void FSMUpdate()
    {

    }
    protected virtual void FSMFixedUpdate()
    {

    }

    public virtual void TakenDamage(int damage)
    {
        health -= damage;
        FlashColor(flashtime);
        GameController.cameraShake.Shake();
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
