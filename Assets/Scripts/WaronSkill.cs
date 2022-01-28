using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaronSkill : MonoBehaviour
{
    WarriorAnims.HeroMovement heroMovement;

    private WaronSkillManage waronSkillManage;
    private FreaksController freaksController;
    private int UseSkillNumber;

    private float _damage = 0f;

    private void Start()
    {
        waronSkillManage = GetComponentInParent<WaronSkillManage>();
        heroMovement = GetComponentInParent<WarriorAnims.HeroMovement>();
    }


    public void Renew(float damage)
    {
        UseSkillNumber = waronSkillManage.UseSkillNumber;
        _damage = damage;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "BlackFreaks")
        {
            if (other == null)
                return;
            GameManager.Damage.OnAttacked(_damage, other.GetComponent<Stat>());
            switch (UseSkillNumber)
            {
                case 2:
                    freaksController = other.transform.GetComponent<FreaksController>();
                    freaksController.StartCoroutine(freaksController.MoveSpeedSlow(0.8f));
                    break;
                case 3:
                    heroMovement.Break();
                    freaksController = other.transform.GetComponent<FreaksController>();
                    freaksController.StartCoroutine(freaksController.Stuern(2f));
                    break;
                default:
                    break;
            }
        }
    }
}