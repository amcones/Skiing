using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentHUD : MonoBehaviour
{
    protected float maxValue;
    protected float curValue;

    protected Player player;

    public virtual void SetPlayer(Player player)
    {
        this.player = player;
    }

    public virtual void Initialize(float maxValue, float curValue = 0)
    {
        this.maxValue = maxValue;
        this.curValue = curValue;
    }

    public virtual void UpdateHUD() { }
}
