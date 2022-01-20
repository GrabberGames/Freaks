using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public void OnAttacked(float value, Stat defender)
    { 
        int damage = (int)Mathf.Max(0, value - defender.ARMOR);
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
