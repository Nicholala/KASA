using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSMState
{
    protected FSMStateID stateID;
    public FSMStateID ID
    {
        get
        {
            return stateID;
        }   
    }
    public virtual void Reason(Transform transform)
    {

    }
    public virtual void Act(Rigidbody2D Rig, Transform transform, float currentSpeed)
    {

    }
    public float velocityX;

    [Header("Move")]
    public float maxRunSpeed=300.0f;
    public float AccelerateTime=0.1f;
    public float DecelerateTime=0.1f;

    [Header("触地判定")]
    public Vector2 GroundPointOffset = new Vector2(0.0f,-0.16f);
    public Vector2 GroundJudgeSize = new Vector2(1.0f,0.5f);
    public LayerMask GroundLayerMask;
    public bool isOnGround;

    protected void Flip(Rigidbody2D Rig, Transform transform)
    {
        if (Mathf.Abs(Rig.velocity.x) > Mathf.Epsilon)
        {
            if (Rig.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Rig.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    protected bool OnGround(Transform transform)
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + GroundPointOffset, GroundJudgeSize, 0, LayerMask.NameToLayer("Ground"));
        if (Coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
