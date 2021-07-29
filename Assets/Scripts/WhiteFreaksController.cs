using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhiteFreaksController : MonoBehaviour
{
    public GameObject miningWorkshop;

    private NavMeshAgent navMeshAgent;
    private GameObject alter;

    private bool isMining = false;
    private bool hasEssense = false;

    private Vector3 alterPosition;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        alter = GameObject.Find("Alter");
        alterPosition = alter.transform.position;
    }
  
    public void SetMiningWorkShop()
    {
        ChkNavMesh();

        navMeshAgent.SetDestination(miningWorkshop.transform.position);
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
                navMeshAgent.SetDestination(miningWorkshop.transform.position);
                if (hasEssense)
                {
                    alter.GetComponent<AlterController>().essence += 10;
                    hasEssense = false;
                }
            }
            else if(collision.gameObject == miningWorkshop)
            {
                if(gameObject.activeSelf)
                {
                    navMeshAgent.SetDestination(alterPosition);
                    hasEssense = true;
                }                
            }
        }
    }
    public void ChangeAlterPosition(Vector3 alterPosition)
    {
        this.alterPosition = alterPosition;
    }

    private void ChkNavMesh()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        }
    }

}
