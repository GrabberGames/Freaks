using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhiteFreaksController : MonoBehaviour
{
    public GameObject miningWorkshop;
    public NavMeshAgent navMeshAgent;

    private AlterController alterController;
    private GameObject alter;

    private bool isMining = false;
    private bool hasEssense = false;
    private bool isFinish = false;

    private Vector3 alterPosition;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        alter = GameObject.Find("Alter");
        alterController = alter.GetComponent<AlterController>();
        alterPosition = alter.transform.position;
    }  


    public void SetMiningWorkShop()
    {
        ChkNavMesh();

        navMeshAgent.SetDestination(miningWorkshop.transform.position);
        isMining = true;
        print("SetMining");
    }


    public void SetSwitch(Vector3 pos)
    {
        ChkNavMesh();

        navMeshAgent.SetDestination(pos);
    }


    public void OnCollisionEnter(Collision collision)
    {
        // 건물 건설 완료 시
        if (isFinish)
        {
            string name = collision.transform.name;

            if (name == "Alter")
            {
                alterController.returnedBusyFreeks();
                Destroy(this.gameObject);
                isMining = false;
            }
        }

        if(isMining)
        {
            string name = collision.transform.name;

            if (name == "Alter")
            {
                navMeshAgent.SetDestination(miningWorkshop.transform.position);

                if (hasEssense)
                {
                    alterController.essence += 10;
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


    public void FinishMining()
    {
        navMeshAgent.SetDestination(alterPosition);
        isFinish = true;
    }
}