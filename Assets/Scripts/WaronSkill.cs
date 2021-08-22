using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaronSkill : MonoBehaviour
{
    WarriorAnims.HeroMovement heroMovement;

    private WaronSkillManage waronSkillManage;
    private FreaksController freaksController;
    
    private int UseSkillNumber;


    private void Awake()
    {
        waronSkillManage = GetComponentInParent<WaronSkillManage>();
        heroMovement = GetComponentInParent<WarriorAnims.HeroMovement>();
    }


    public void Renew()
    {
        UseSkillNumber = waronSkillManage.UseSkillNumber;
    }


    private void OnTriggerEnter(Collider other)
    {
        print("*");
        if (other.transform.tag == "BlackFreaks")
        {
            switch (UseSkillNumber)
            {
                case 2:
                    freaksController = other.transform.GetComponent<FreaksController>();
                    freaksController.StartCoroutine(freaksController.MoveSpeedSlow(0.8f));
                    freaksController.Damaged(1f);
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