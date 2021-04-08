using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    public Transform Player;
    [SerializeField] private float startDistance;
    [SerializeField] private float speed;

    public LayerMask checkLayer;
    public float checkDis;

    private bool isBarried = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Player.position + new Vector3(0, startDistance, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Following();
    }

    private void Following()
    {
        float dis = Vector3.Distance(transform.position, Player.position);
        float xdis = transform.position.x - Player.position.x;
        if (!CheckBarrier() || dis < checkDis)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }
        else
        {
            if (xdis >= 0)
                transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            else
                transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
        }

    }
    
    private bool CheckBarrier()
    {
        RaycastHit2D hit2D;
        hit2D = Physics2D.Raycast(transform.position, Vector3.down, checkDis, checkLayer);
        Debug.DrawRay(transform.position, Vector3.down);
        if (hit2D)
            return true;
        return false;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Barries")
    //        isBarried = true;
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Barries")
    //        isBarried = false;
    //}
}
