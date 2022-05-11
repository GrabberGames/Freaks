using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhiteFreaksController : Stat
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
        Init();

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        alter = GameManager.Instance.Alter;

        alterController = alter.GetComponent<AlterController>();
        alterPosition = alter.transform.position;

        /// <-알터 위치가 변경 되었을때 사용되는 함수입니다.->
        BuildingManager.Instance.AlterIsChange -= AlterIsChanged;
        BuildingManager.Instance.AlterIsChange += AlterIsChanged;
    }
    protected override void Init()
    {
        base.Init();
    }
    /// <-알터 위치가 변경 되었을때 사용되는 함수입니다.>
    void AlterIsChanged(GameObject go)
    {
        Debug.Log("AlterChanged");
        this.alter = go;
    }
    /// <-알터 위치가 변경 되었을때 사용되는 함수입니다>


    public void SetMiningWorkShop()
    {
        ChkNavMesh();

        navMeshAgent.SetDestination(miningWorkshop.transform.position);
        isMining = true;
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