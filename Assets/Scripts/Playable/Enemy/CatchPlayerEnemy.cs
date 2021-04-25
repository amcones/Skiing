using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPlayerEnemy : Enemy
{
    public float disappearTime;
    public float VelocitySize;

    public new Rigidbody2D rigidbody2D;

    public void CatchPlayer()
    {
        rigidbody2D.velocity = (player.transform.position - transform.position).normalized* VelocitySize;
    }

    public override void StartRun()
    {
        StartCoroutine(Catch(disappearTime));
        StartCoroutine(TimingDisappear(disappearTime));
    }

    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }

    private IEnumerator Catch(float time)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(Time.deltaTime);
        while(time > 0.0f)
        {
            CatchPlayer();
            yield return waitForSeconds;
        }
    }

    private IEnumerator TimingDisappear(float time)
    {
        yield return new WaitForSeconds(time);
        Disappear();
    }
}
