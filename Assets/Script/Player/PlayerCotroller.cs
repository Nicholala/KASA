using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotroller : PlayerFSM
{
    [Header("触地判定")]
    public Vector2 GroundPointOffset;
    public Vector2 GroundJudgeSize;
    public LayerMask GroundLayerMask;
    public bool isOnGround;

    protected override void Initialize()
    {
        ConstructFSM();
    }

    protected override void FSMUpdate()
    {

    }

    protected override void FSMFixedUpdate()
    {
        animator.SetFloat("Horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));
        CurrentState.Reason(playerTransform);
        CurrentState.Act(Rig, playerTransform, currentSpeed);
    }

    protected void ConstructFSM()
    {
        Debug.Log("!!");
        PlayerWalkState Walk = new PlayerWalkState();
        AddFSMState(Walk);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + GroundPointOffset, GroundJudgeSize);
    }
}
