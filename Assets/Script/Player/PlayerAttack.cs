using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackStartTime = 0f;
    public float attackIntervalTime;
    public int damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(Time.time - attackStartTime > attackIntervalTime)
            {
                attackStartTime = Time.time;
                Debug.Log("!");
                other.GetComponent<Enemy>().TakenDamage(damage);              
            }  
        }
        if (other.gameObject.CompareTag("Door"))
        {
            if (Time.time - attackStartTime > attackIntervalTime)
            {
                attackStartTime = Time.time;
                Debug.Log("!");
                other.GetComponent<Door>().TakenDamage(damage);
            }
        }
    }

    void FixedUpdate()
    {

    }
}
