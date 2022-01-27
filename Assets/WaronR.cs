using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WaronR : MonoBehaviour
{
    WarriorAnims.HeroMovement hero;
    GameObject go = null;
    float _damage = 0;
    private void Start()
    {
        hero = GetComponentInParent<WarriorAnims.HeroMovement>();
    }
    public void Init(float damage)
    {
        _damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        hero.WaronRHitted += FreaksInRange;
        go = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        hero.WaronRHitted -= FreaksInRange;
        go = null;
    }
    void FreaksInRange()
    {
        if (go == null)
            return;
        GameManager.Damage.OnAttacked(_damage, go.GetComponent<Stat>());
        print(go.GetComponent<Stat>().HP);
    }
}
