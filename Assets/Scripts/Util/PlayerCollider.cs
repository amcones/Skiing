using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour, IGetOtherCollider
{
    private Collider2D otherCollider;

    void OnTriggerEnter2D(Collider2D collider)
    {
        otherCollider = collider;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        otherCollider = collider;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        otherCollider = collision.collider;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        otherCollider = collision.collider;
    }

    public Collider2D GetOtherCollider 
    { 
        get 
        {
            Collider2D collider = otherCollider;
            otherCollider = null;
            return collider;
        } 
    }
}
