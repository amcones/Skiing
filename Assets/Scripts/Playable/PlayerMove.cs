using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    public Animator animator;

    public Rigidbody2D rb;
    public float horizontalForce;
    public float verticalForce;
    public Vector2 MaxVelocity;
    
    BoxCollider2D playCollider;
    float horizontal;
    float vertical;
    bool isDown;
    // Start is called before the first frame update
    void Start()
    {
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
        rb.AddForce(new Vector2(horizontal * horizontalForce * Time.deltaTime, vertical), ForceMode2D.Impulse);
        rb.velocity = new Vector2(GetClamp(rb.velocity.x, MaxVelocity.x), GetClamp(rb.velocity.y, MaxVelocity.y));
    }

    public void Initialize(float horizontalForce, float verticalForce, Vector2 MaxVelocity)
    {
        this.horizontalForce = horizontalForce;
        this.verticalForce = verticalForce;
        this.MaxVelocity = MaxVelocity;
    }

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
