using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WaronR : MonoBehaviour
{
    public Action FreaksList = null;

    WarriorAnims.HeroMovement hero;
    GameController gameController;
    float _damage = 0;
    private void OnEnable()
    {
        gameController = FindObjectOfType<GameController>();
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
        List<FreaksController> _freaks = new List<FreaksController>();
        _freaks = gameController.GetAliveBlackFreaksList();
        for (int i = 0; i < _freaks.Count; i++)
        {
            if ((_freaks[i].gameObject.transform.position - transform.position).magnitude < 15f)
            {
                GameManager.Damage.OnAttacked(150 + 0.5f * _damage, _freaks[i].GetComponent<Stat>());
            }
        }
    }
}
