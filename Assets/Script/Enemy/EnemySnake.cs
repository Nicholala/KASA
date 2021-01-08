using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : Enemy
{
    public Transform[] moveDes;
    public float speed;
    public bool isMovingLeft = true;
    public float waitTime;
    private int i = 0;

    protected override void Initialize()
    {
        base.Initialize();
    }
     
    protected override void FSMFixedUpdate()
    {
        base.FSMFixedUpdate();
        Patrol();
    }

    void Patrol()
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
