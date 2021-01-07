using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FSMStateID 
{ 
    Idle = 0,
    Walk,
    Drop,
    Attack,
    Paragliding,
    Dead
}

public enum Transition
{
    None=0,
    StartDropping,
    AttackPress,
}

public class PlayerFSM : FSM
{
    private List<PlayerFSMState> fsmStates;

    public Transform playerTransform;
    protected float currentSpeed;
    public Rigidbody2D Rig;
    public Animator animator;

    private FSMStateID currentStateID;
    public FSMStateID CurrentStateID
    {
        get
        {
            return currentStateID;
        }
    }

    private PlayerFSMState currentState;
    public PlayerFSMState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public PlayerFSM()
    {
        fsmStates = new List<PlayerFSMState>();
    }

    public void AddFSMState(PlayerFSMState fsmState)
    {
        if (fsmState == null)
        {
            Debug.LogError("FSM ERROR");
            return;
        }

        if (fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return;
        }

        foreach(PlayerFSMState state in fsmStates)
        {
            if (state.ID == fsmState.ID)
            {
                Debug.LogError("FSM ERROR:has already");
                return;
            }
        }

        fsmStates.Add(fsmState);
    }
}
