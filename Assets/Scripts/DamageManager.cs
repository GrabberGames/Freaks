using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager
{
    public void OnAttacked(float value, Stat defender)
    { 
        float damage = Mathf.Max(0, value - defender.ARMOR) * (1 - defender.DECREASE_DAMAGE);
        defender.HP -= damage;

        if(defender.HP <= 0)
        {
            defender.HP = 0;
            OnDead(defender);
        }
    }
    public void OnDead(Stat defender)
    {
        Debug.Log("Dead!" + defender.gameObject);

        defender.DeadSignal();
    }
}
