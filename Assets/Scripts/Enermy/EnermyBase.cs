using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyBase : MonoBehaviour
{
    public float Health { get; private set; }




    public virtual void Attack()
    {

    }

    public void IncreaseHealth(float value)
    {
        if (value > 0)
            Health += value;
        else
            Debug.LogWarning("增加的血量为负数");
    }
    public void DecreaseHealth(float value)
    {
        if (value > 0)
            Health -= value;
        else
            Debug.LogWarning("减少的血量为负数");
    }
}
