using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerFSMState
{
    public PlayerWalkState()
    {
        stateID = FSMStateID.Walk;
    }

    public override void Reason(Transform transform)
    {

    }
    public override void Act(Rigidbody2D Rig,Transform transform, float currentSpeed)
    {
        Flip(Rig,transform);
        HorizontalMove(Rig, currentSpeed);
        isOnGround=OnGround(transform);
        Debug.Log(isOnGround);
    }

    void HorizontalMove(Rigidbody2D Rig, float currentSpeed)
    {

        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            currentSpeed = Mathf.SmoothDamp(Rig.velocity.x, maxRunSpeed * Time.fixedDeltaTime, ref velocityX, AccelerateTime);
            Rig.velocity = new Vector2(currentSpeed, Rig.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            currentSpeed = Mathf.SmoothDamp(Rig.velocity.x, -1 * maxRunSpeed * Time.fixedDeltaTime, ref velocityX, AccelerateTime);
            Rig.velocity = new Vector2(currentSpeed, Rig.velocity.y);
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(Rig.velocity.x, 0, ref velocityX, DecelerateTime);
            Rig.velocity = new Vector2(currentSpeed, Rig.velocity.y);
        }
    }
}
