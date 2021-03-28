using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestMove : MonoBehaviour
{
    public Animator animator;

    public Rigidbody2D rb;
    public float force;
    public float MaxVelocity;

    float horizontal;
    float vertical;
    // Start is called before the first frame update
    void Start()
    {
        vertical = 1.0f;
        if (rb == null)
            rb = gameObject.GetComponent<Rigidbody2D>();
    }

    //获得左右转向来控制动画的播放
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if(Input.GetAxis("Vertical") < 0)
            vertical = 1.0f + Input.GetAxis("Vertical");
        animator.SetFloat("Horizontal", horizontal);
    }

    //private void FixedUpdate()
    //{
    //    rb.AddForce(new Vector2(horizontal, vertical) * force, ForceMode2D.Impulse);
    //    rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxVelocity);
    //}
}
