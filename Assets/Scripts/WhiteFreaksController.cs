using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhiteFreaksController : MonoBehaviour
{
    public GameObject miningWorkshop;

    private NavMeshAgent workerAgent;
    private GameObject alter;

    private bool isMining = false;
    private bool hasEssense = false;


    // Start is called before the first frame update
    void Start()
    {
        workerAgent = gameObject.GetComponent<NavMeshAgent>();
        alter = GameObject.Find("Alter");
    }
  
    public void SetMiningWorkShop()
    {
        if (workerAgent == null)
        {
            workerAgent = gameObject.GetComponent<NavMeshAgent>();
        }

        workerAgent.SetDestination(miningWorkshop.transform.position);
        isMining = true;
        print("SetMining");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(isMining)
        {
            string name = collision.transform.name;
            if (name == "Alter")
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
    public void ChangeAlterPosition()
    {
        alter = GameObject.Find("Alter");
    }



}
