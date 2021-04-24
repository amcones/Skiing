using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    public Animator animator;

    public Rigidbody2D rb;
    public BoxCollider2D playCollider;
    public float horizontalForce;
    public float decreaseRate;
    public float verticalForce;
    public Vector2 MaxVelocity;
    
    float horizontal;
    float vertical;
    bool isDown;

    float preHorizontalForce;

    // Start is called before the first frame update
    void Start()
    {
        preHorizontalForce = horizontalForce;
        if (playCollider == null)
            playCollider = gameObject.GetComponent<BoxCollider2D>();
        if (rb == null)
            rb = gameObject.GetComponent<Rigidbody2D>();
    }

    //获得左右转向来控制动画的播放
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Mathf.Clamp(-(vertical + verticalForce * Time.deltaTime), -MaxVelocity.y, 0);
        isDown = Input.GetKeyDown(KeyCode.DownArrow);
        animator.SetFloat("Horizontal", horizontal);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(GetClamp(rb.velocity.x + Time.deltaTime * horizontalForce * horizontal, MaxVelocity.x), 
            GetClamp(rb.velocity.y + vertical * (MaxVelocity.y - rb.velocity.y) / MaxVelocity.y, MaxVelocity.y));
    }

    public void StopMove()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    public void TouchBarrier()
    {
        rb.velocity = Vector2.zero;
        horizontalForce /= decreaseRate;
        playCollider.enabled = false;
    }

    public void ResetState()
    {
        playCollider.enabled = true;
        horizontalForce = preHorizontalForce;
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
