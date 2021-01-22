using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemySnake : Enemy
{   
    public Transform[] moveDes;
    public Transform playerTrans;
    public float speed;
    public bool isMovingLeft = true;
    public float waitTime;
    private int i = 0;

    [Header("击飞")]
    Vector2 oriVelocity;
    public Vector2 dir;
    public float dashSpeed;
    public float beingAttackTime;
    public float DragMaxForce;
    public float DragDuration;

    protected override void Initialize()
    {
        base.Initialize();
    }
     
    protected override void FSMFixedUpdate()
    {
        base.FSMFixedUpdate();
        Patrol();
    }

    public override void TakenDamage(int damage)
    {
        base.TakenDamage(damage);
        if (playerTrans.position.x < transform.position.x)
        {
            dir = new Vector2(1, 0);
        }
        else
        {
            dir = new Vector2(-1, 0);
        }
        //将玩家当前所有动量清零
        oriVelocity = Rig.velocity;
        Rig.velocity = Vector2.zero;
        //施加一个力，让玩家飞出去
        Rig.velocity += dir * dashSpeed;
        StartCoroutine(BeingAttack());
    }

    IEnumerator BeingAttack()
    {
        canMove = false;
       
        DOVirtual.Float(DragMaxForce, 0, DragDuration, (x) => Rig.drag = x);      
        yield return new WaitForSeconds(beingAttackTime);
        Rig.drag = 0;
        canMove = true;
        Debug.Log("!");
        Rig.velocity = oriVelocity;
    }

    void Patrol()
    {
        if (canMove)
        {          
            transform.position = Vector2.MoveTowards(transform.position, moveDes[i].position, speed * Time.fixedDeltaTime);
            if (Vector2.Distance(transform.position, moveDes[i].position) < 0.1f)
            {
                if (waitTime > 0)
                {
                    waitTime -= Time.fixedDeltaTime;
                }
                else
                {
                    if (isMovingLeft)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        isMovingLeft = false;
                        i = 1;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        isMovingLeft = true;
                        i = 0;
                    }
                }
            }
        }     
    }
}
