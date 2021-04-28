using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    public enum MoveState
    {
        Skiing,
        TouchBarrier,
        SpeedUp,
    }

    public Animator animator;

    public Rigidbody2D rb;
    public BoxCollider2D playCollider;
    public float horizontalForce;
    public float decreaseRate;
    public float verticalForce;
    public Vector2 MaxVelocity;
    public float waitSpeedUpTime;
    public Vector2 speedUpVelocity;
    public UnityEvent addSpeedUpEvent;

    float horizontal;
    float vertical;
    bool isSpeedUpDown;

    float speedUpTime;
    MoveState moveState;
    float preHorizontalForce;


    // Start is called before the first frame update
    void Start()
    {
        preHorizontalForce = horizontalForce;
        speedUpTime = 0;
        moveState = MoveState.Skiing;
        if (playCollider == null)
            playCollider = gameObject.GetComponent<BoxCollider2D>();
        if (rb == null)
            rb = gameObject.GetComponent<Rigidbody2D>();
    }

    //获得左右转向来控制动画的播放
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Mathf.Clamp(-(verticalForce * Time.deltaTime), -MaxVelocity.y, 0);
        if (moveState == MoveState.TouchBarrier)
            vertical /= decreaseRate;
        if(!isSpeedUpDown)
            isSpeedUpDown = Input.GetKeyDown(KeyCode.Space);
        animator.SetFloat("Horizontal", horizontal);
        AddLoadSpeedUpTime();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(GetClamp(rb.velocity.x + Time.deltaTime * horizontalForce * horizontal, MaxVelocity.x), 
            GetClamp(rb.velocity.y + vertical * (MaxVelocity.y - rb.velocity.y) / MaxVelocity.y, MaxVelocity.y));
        if(isSpeedUpDown)
        {
            SpeedUp();
        }
        if(moveState == MoveState.SpeedUp)
        {
            rb.velocity += speedUpVelocity;
        }
    }

    public void StopMove()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    public float GetSpeedUpTime()
    {
        return speedUpTime;
    }

    public void AddLoadSpeedUpTime()
    {
        addSpeedUpEvent.Invoke();
        if (moveState == MoveState.SpeedUp)
            return;
        speedUpTime += Time.deltaTime;
        if (speedUpTime >= waitSpeedUpTime)
            speedUpTime = waitSpeedUpTime;
    }

    public void SpeedUp()
    {
        if(speedUpTime == waitSpeedUpTime)
        {
            StartCoroutine(WaitSpeedUp(waitSpeedUpTime));
        }
    }

    private IEnumerator WaitSpeedUp(float time)
    {
        if (moveState == MoveState.SpeedUp)
            yield break;

        moveState = MoveState.SpeedUp;
        yield return new WaitForSeconds(time);
        if (moveState == MoveState.SpeedUp)
            moveState = MoveState.Skiing;
    }

    public void TouchBarrier()
    {
        rb.velocity = Vector2.zero;
        horizontalForce /= decreaseRate;
        playCollider.enabled = false;
        moveState = MoveState.TouchBarrier;
    }

    public void ResetState()
    {
        playCollider.enabled = true;
        horizontalForce = preHorizontalForce;
        moveState = MoveState.Skiing;
    }

    public void Initialize(float horizontalForce, float verticalForce, Vector2 MaxVelocity)
    {
        this.horizontalForce = horizontalForce;
        this.verticalForce = verticalForce;
        this.MaxVelocity = MaxVelocity;
    }

    /// <summary>
    /// 提供一个限制value范围在[-target, taget]的方法
    /// </summary>
    float GetClamp(float value, float target)
    {
        if(value > 0 && value > target)
        {
            return target;
        }

        if(value < 0 && value < -target)
        {
            return -target;
        }

        return value;
    }
}
