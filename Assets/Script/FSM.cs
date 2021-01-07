using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{

    protected virtual void Initialize()
    {

    }
    protected virtual void FSMUpdate()
    {

    }
    protected virtual void FSMFixedUpdate()
    {

    }

    void Start()
    {
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
}
