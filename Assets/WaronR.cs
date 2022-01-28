using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WaronR : MonoBehaviour
{
    public Action FreaksList = null;

    WarriorAnims.HeroMovement hero;
    GameObject go = null;

    float _damage = 0;
    private void Start()
    {
        hero = GetComponentInParent<WarriorAnims.HeroMovement>();
        hero.WaronRHitted += FreaksInRange;
    }
    public void Init(float damage)
    {
        _damage = damage;
    }
    private void OnDisable()
    {
        hero.WaronRHitted -= FreaksInRange;
    }
    void FreaksInRange()
    {
        LayerMask mask = LayerMask.GetMask("blackfreaks");
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 11, mask))
        {
            GameManager.Damage.OnAttacked(_damage, collider.gameObject.GetComponent<Stat>());
        }
    }
}
