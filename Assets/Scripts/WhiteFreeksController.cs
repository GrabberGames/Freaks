using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhiteFreeksController : MonoBehaviour 
{
    private NavMeshAgent workerAgent;
    private RaycastHit hit;     // hit checker
    private string hitColliderName;

    public GameObject miningWorkshop;
    private GameObject alter;
    private bool isMining = false;
    private bool hasEssense = false;

    
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        workerAgent = gameObject.GetComponent<NavMeshAgent>();
        alter = GameObject.Find("Alter_B");
        SetMiningWorkShop();
    }
  
    public void SetMiningWorkShop()
    {
        if (workerAgent == null) return;
        workerAgent.SetDestination(miningWorkshop.transform.position);
        isMining = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(isMining)
        {
            string name = collision.transform.name;
            if (name == "Alter_B")
            {
                workerAgent.SetDestination(miningWorkshop.transform.position);
                if (hasEssense)
                {
                    alter.GetComponent<AlterController>().essence += 10;
                    hasEssense = false;
                }
            }
            else if(collision.gameObject == miningWorkshop)
            {
                workerAgent.SetDestination(alter.transform.position);
                hasEssense = true;
            }
        }
    }



}
