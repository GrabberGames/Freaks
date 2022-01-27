using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaronSkillManage : MonoBehaviour
{
    public GameObject[] SkillColliderList;

    public int UseSkillNumber { get; set; }


    private void Awake()
    {
        AllColliderOff();
    }


    public void AllColliderOff()
    {
        foreach (var all in SkillColliderList)
        {
            all.GetComponent<Collider>().enabled = false;
        }
    }


    public bool SkillOnTrigger(float damage)
    {
        SkillColliderList[UseSkillNumber].GetComponent<Collider>().enabled = true;
        SkillColliderList[UseSkillNumber].GetComponent<WaronSkill>().Renew(damage);
        return true;
    }
}