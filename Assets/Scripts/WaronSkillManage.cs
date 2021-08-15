using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaronSkillManage : MonoBehaviour
{
    public GameObject[] SkillColliderList;

    public int UseSkillNumber { get; set; }
    private FreaksController freaksController;
    private WarriorAnims.HeroMovement heroMovement;
    // Start is called before the first frame update
    private void Awake()
    {
        heroMovement = GetComponentInParent<WarriorAnims.HeroMovement>();
    }
    private void Update()
    {
        SkillColliderList[UseSkillNumber].SetActive(true);
    }
    private void AllColliderOff()
    {
        foreach(var all in SkillColliderList)
        {
            all.SetActive(false);
        }
    }
    public bool SkillOnTrigger()
    {
        return true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "BlackFreaks")
        {
            switch (UseSkillNumber)
            {
                case 1:
                    freaksController = collision.transform.GetComponent<FreaksController>();
                    freaksController.MoveSpeedSlow(0.8f);
                    freaksController.Damaged(1f);
                    break;
                case 2:
                    freaksController = collision.transform.GetComponent<FreaksController>();
                    freaksController.StartCoroutine(freaksController.Stuern(2f));
                    break;
                default:
                    break;
            }
        }
    }
}
