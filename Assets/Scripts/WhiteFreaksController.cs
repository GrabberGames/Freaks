using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;

public class WhiteFreaksController : Stat
{
    public GameObject targetBuilding;
    public GameObject targetBefore;
    public NavMeshAgent navMeshAgent;
    public GameObject buildingGO;


    public GameObject connectEssence;
    [SerializeField]
    public GameObject alter;

    private Vector3 alterPosition;

    public bool IsBuilding = false;
    public bool IsMoving = false;
    private IDisposable arriveStream = default;

    GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        go = this.gameObject;
        navMeshAgent = GetComponent<NavMeshAgent>();
        IsMoving = false;
        alter = BuildingManager.Instance.Alter;

        alterPosition = alter.transform.position;
    }
      /*
        arriveStream = this.UpdateAsObservable()
          .Select(IsStopped)
          .DistinctUntilChanged()
          .ThrottleFrame(2)
          .Where(stopped => stopped)
          .Subscribe(stopped => Arrive())
          .AddTo(this);


    
    }*/
    
    bool IsArrive = false;
    private void Update()
    {
        if (IsArrive == false)
        {

            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        IsMoving = false;
                        WhiteFreaksManager.Instance.SignOfWhiteFreaksDecrease();
                        //this.gameObject.SetActive(false);


                        IsArrive = true;
                        Arrive();

                    }
                }


            }
/*
                if (navMeshAgent.velocity.sqrMagnitude >=0.2f && navMeshAgent.remainingDistance <= 0.5f)
            {
               
                IsArrive = true;
                Arrive();
            }*/
              
        }

    }
    
    bool isFirst = true;
    // 죽는거
    public override void DeadSignal()
    {
        //base.DeadSignal();

        if (isFirst == true)
        {
            IsMoving = false;

            if (targetBuilding.CompareTag("Friendly") || targetBuilding.CompareTag("workshop")) //알터로 돌아가는게 아니라면(지으러 가는도중에 쥬금)
            {
                BuildingPooling.instance.ReturnObject(targetBuilding);
               // BuildingPooling.instance.ReturnObject(targetBefore);
                BuildingPooling.instance.ReturnObject(buildingGO);
                buildingGO.GetComponent<Building>().StopAllCoroutines();
                if (targetBuilding.CompareTag("workshop")) //심지어 워크샵 지으러 가는중이었다면
                {
                    //BuildingManager.Instance.GetEssenceSpot().SetActive(true); //자원지 다시 보이도록
                    connectEssence.SetActive(true);
                }
            }
            Debug.Log("여기까지왔니 그러면 없어져야할텐데..");
            WhiteFreaksManager.Instance.ReturnWhiteFreaks(this.gameObject);
            isFirst = false;
        }

    }


    protected override void Init()
    {
        base.Init();
    }

    private bool IsStopped(Unit unit)
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    IsMoving = false;
                    WhiteFreaksManager.Instance.SignOfWhiteFreaksDecrease();
                    //this.gameObject.SetActive(false);
                    return true;
                }
            }
        }
        return false;
    }

   // Building buildng;
    void Arrive()
    {
        Debug.Log("도착했다");
        IsMoving = false;
        if (IsBuilding == true)
        {
            Debug.Log("지으러왔다");
           // buildng = targetBuilding.GetComponent<Building>();
            buildingGO.GetComponent<Building>().ChangeBuilding();
            go.SetActive(false);
            IsBuilding = false;

            IsArrive = false;
            //switch�� �� �κп��ٰ� ���� ����
            //
        }
        else
        {
            IsArrive = false;
            Debug.Log("지으러 온거아니다");
            WhiteFreaksManager.Instance.ReturnWhiteFreaks();
            ObjectPooling.Instance.ReturnObject(this.gameObject);
        }
    }

    private void ChkNavMesh()
    {
        if (navMeshAgent == null)
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

    }

    public void SetDestination(GameObject target, bool isBuilding)
    {
        IsMoving = true;
        // go.SetActive(true);
        IsBuilding = isBuilding;
        targetBuilding = target;


        ChkNavMesh();
        navMeshAgent.SetDestination(new Vector3(target.transform.position.x + 1, target.transform.position.y + 2, target.transform.position.z));
    }


}