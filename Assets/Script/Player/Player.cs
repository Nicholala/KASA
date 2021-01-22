using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : FSM
{
    public enum PlayerFSMState
    {
        Idle,
        Run,
        Drop,
        Jump,
        Attack,
        Glide,
        Dash,
        Dead
    }

    [Header("State")]
    public PlayerFSMState currentState;
    public PlayerFSMState lastState;

    [Header("Move")]
    public float maxRunSpeed;
    public float moveDir;
    public float currentSpeedX;
    public float currentSpeedY;
    public float AccelerateTime;
    public float DecelerateTime;

    [Header("跳跃")]
    public float JumpStartY;
    public float JumpSpeed;
    public float FallMultiplier;
    public float JumpMultiplier;
    public float LowJumpMultiplier;
    public int JumpTimes;
    public bool isJumping;
    public bool CanJump = true;
    public float JumpStartTime;
    public bool isJumpingButtonReleased = true;

    [Header("攻击")]
    public PolygonCollider2D HitBox;
    public float attackStartTime;
    public float attackTime;
    public bool isAttackButtonReleased = true;

    [Header("滑翔")]
    public float glideSpeedY;


    [Header("冲刺")]
    public float dashTime;
    public float dashCoolDown;
    public float dashSpeed;
    public bool isDashReleased = true;
    private bool isDashing;  
    private float dashTimeLeft;
    private float lastDash = -10f;
    private float beforeDashSpeedX;

    [Header("重力调整")]
    public float gravity;
    public float dropAccelerateTime;
    public float maxDropSpeed;
    public bool CanUsingGravityAdjuster = true;

    [Header("触地判定")]
    public Vector2 GroundPointOffset;
    public Vector2 GroundJudgeSize;
    public LayerMask GroundLayerMask;
    public bool isOnGround;

    [Header("Component")]
    public Rigidbody2D Rig;
    float velocityX;
    public Animator animator;

    protected override void Initialize()
    {
        currentState = PlayerFSMState.Run;
        beforeDashSpeedX = 0;
        gravity = Rig.gravityScale;
    }

    protected override void FSMFixedUpdate()
    {
        animator.SetFloat("Horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));
        Flip();
        isOnGround = OnGround();
        if (Rig.velocity.x < -0.1f)
        {
            moveDir = -1.0f;
        }
        if (Rig.velocity.x > 0.1f)
        {
            moveDir = 1.0f;
        }

        if (Input.GetAxis("Jump") == 0)
        {
            isJumpingButtonReleased = true;
        }
        if (Input.GetAxis("Attack") == 0)
        {
            isAttackButtonReleased = true;
        }
        if (Input.GetAxis("Dash") == 0)
        {
            isDashReleased = true;
        }

        switch (currentState)
        {
            case PlayerFSMState.Run:
                RunState();
                break;
            case PlayerFSMState.Drop:
                DropState();
                break;
            case PlayerFSMState.Jump:
                JumpState();
                break;
            case PlayerFSMState.Glide:
                GlideState();
                break;
            case PlayerFSMState.Attack:
                AttackState();
                break;
            case PlayerFSMState.Dash:
                DashState();
                break;
        }
    }

    private void Flip()
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
    #region 公用方法
    private void HorizontalMove()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            currentSpeedX = Mathf.SmoothDamp(Rig.velocity.x, maxRunSpeed * Time.fixedDeltaTime, ref velocityX, AccelerateTime);
            Rig.velocity = new Vector2(currentSpeedX, Rig.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            currentSpeedX = Mathf.SmoothDamp(Rig.velocity.x, -1 * maxRunSpeed * Time.fixedDeltaTime, ref velocityX, AccelerateTime);
            Rig.velocity = new Vector2(currentSpeedX, Rig.velocity.y);
        }
        else
        {
            currentSpeedX = Mathf.SmoothDamp(Rig.velocity.x, 0, ref velocityX, DecelerateTime);
            Rig.velocity = new Vector2(currentSpeedX, Rig.velocity.y);
        }
    }

    bool OnGround()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + GroundPointOffset, GroundJudgeSize, 0, GroundLayerMask);
        if (Coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + GroundPointOffset, GroundJudgeSize);
    }

    #endregion

    #region Run
    private void RunState()
    {
        CanJump = true;
        HorizontalMove();
        if (!isOnGround)
        {
            currentState = PlayerFSMState.Drop;
            Debug.Log("StateChange:Drop");
        }
        if (Input.GetAxis("Jump") == 1)
        {
            JumpStartY = transform.position.y;
            currentState = PlayerFSMState.Jump;
            Debug.Log("StateChange:Jump");
        }
        if (Input.GetAxis("Attack") == 1 && isAttackButtonReleased)
        {
            lastState = currentState;
            currentState = PlayerFSMState.Attack;
            animator.SetTrigger("Attack");
            isAttackButtonReleased = false;
        }
        if (Input.GetAxis("Dash") == 1 && isDashReleased)
        {
            Debug.Log("WantToDash");
            isDashReleased = false;
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //执行dash
                ReadyToDash();
                lastState = currentState;
                currentState = PlayerFSMState.Dash;
                isDashReleased = false;
            }
        }
    }

    #endregion

    private void DropState()
    {
        HorizontalMove();
        if (Rig.velocity.y > maxDropSpeed)
        {
            Rig.velocity += Vector2.up * Physics2D.gravity.y * Time.fixedDeltaTime * (FallMultiplier) * Time.fixedDeltaTime;
        }
        else
        {
            Rig.velocity = new Vector2(currentSpeedX, maxDropSpeed);
        }
        if (isOnGround)
        {
            currentState = PlayerFSMState.Run;
            Debug.Log("StateChange:Run");
        }
        if (Input.GetAxisRaw("Glide") == 1)
        {
            currentState = PlayerFSMState.Glide;
        }
        if (Input.GetAxis("Attack") == 1 && isAttackButtonReleased)
        {
            lastState = currentState;
            currentState = PlayerFSMState.Attack;
            animator.SetTrigger("Attack");
            isAttackButtonReleased = false;
        }
        if (Input.GetAxis("Dash") == 1 && isDashReleased)
        {
            Debug.Log("WantToDash");
            isDashReleased = false;
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //执行dash
                ReadyToDash();
                lastState = currentState;
                currentState = PlayerFSMState.Dash;
                isDashReleased = false;
            }
        }
    }

    #region 跳跃
    private void JumpState()
    {
        HorizontalMove();
        //Jump
        if (CanJump)
        {
            if (Input.GetAxis("Jump") == 1 && !isJumping && isJumpingButtonReleased)
            {
                //JumpAudio.Play();
                isJumpingButtonReleased = false;
                JumpStartTime = Time.time;
                Rig.velocity = new Vector2(Rig.velocity.x, JumpSpeed * Time.fixedDeltaTime);
                isJumping = true;
                JumpTimes--;
            }
            if (CanUsingGravityAdjuster)
            {
                if (Rig.velocity.y < 0)
                {
                    Rig.velocity += Vector2.up * Physics2D.gravity.y * Time.fixedDeltaTime * (FallMultiplier) * Time.fixedDeltaTime;
                }
                else if (Rig.velocity.y > 0 && Input.GetAxis("Jump") != 1)
                {
                    Rig.velocity += Vector2.up * Physics2D.gravity.y * Time.fixedDeltaTime * (LowJumpMultiplier) * Time.fixedDeltaTime;
                }
                else
                {
                    Rig.velocity += Vector2.up * Physics2D.gravity.y * Time.fixedDeltaTime * (JumpMultiplier) * Time.fixedDeltaTime;
                }
            }
        }
        if (isOnGround && Time.time - JumpStartTime > 0.2f)
        {
            JumpTimes = 1;
            isJumping = false;
            currentState = PlayerFSMState.Run;
            Debug.Log("StateChange:Run");
        }
        if (Input.GetAxisRaw("Glide") == 1)
        {          
            currentState = PlayerFSMState.Glide;
        }
        if (transform.position.y < (JumpStartY - 0.1f))
        {
            currentState = PlayerFSMState.Drop;
        }
        if (Input.GetAxis("Attack") == 1  && isAttackButtonReleased)
        {
            lastState = currentState;
            currentState = PlayerFSMState.Attack;
            animator.SetTrigger("Attack");
            isAttackButtonReleased = false;
        }
        if (Input.GetAxis("Dash") == 1 && isDashReleased)
        {
            Debug.Log("WantToDash");
            isDashReleased = false;
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //执行dash
                ReadyToDash();
                lastState = currentState;
                currentState = PlayerFSMState.Dash;
                isDashReleased = false;
            }
        }
    }
    #endregion

    #region 滑翔
    private void GlideState()
    {
        HorizontalMove();
        if (Input.GetAxisRaw("Glide") == 1)
        {
            Rig.gravityScale = 0;
            Rig.velocity = new Vector2(currentSpeedX, glideSpeedY);
        }
        if (Input.GetAxisRaw("Glide") != 1)
        {
            Rig.gravityScale = gravity;
            currentState = PlayerFSMState.Drop;
        }
        if (isOnGround)
        {
            Rig.gravityScale = gravity;
            currentState = PlayerFSMState.Run;           
        }
    }
    #endregion

    #region 攻击
    private void AttackState()
    {
        HorizontalMove();
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(attackStartTime);
        HitBox.enabled = true;
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(attackStartTime);
        HitBox.enabled = false;
        currentState = lastState;
    }
    #endregion

    private void DashState()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                Rig.velocity = new Vector2(dashSpeed * moveDir, 0);
                dashTimeLeft -= Time.fixedDeltaTime;
                ObjectPool.instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                Rig.velocity = new Vector2(beforeDashSpeedX, 0);
                Rig.gravityScale = gravity;
                currentState = lastState;
            }
        }
    }

    private void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        beforeDashSpeedX = currentSpeedX;
        Rig.gravityScale = 0;
    }
}