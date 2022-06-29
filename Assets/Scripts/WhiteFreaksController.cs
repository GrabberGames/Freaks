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
        alter = GameManager.Instance.Alter;

        alterPosition = alter.transform.position;

        /*
        arriveStream = this.UpdateAsObservable()
            .Select(IsStopped)
            .DistinctUntilChanged()
            .ThrottleFrame(3)
            .Where(stopped => stopped)
            .Subscribe(stopped => onArriveCallback?.Invoke())
            .AddTo(this);
            */

        arriveStream = this.UpdateAsObservable()
          .Select(IsStopped)
          .DistinctUntilChanged()
          .ThrottleFrame(3)
          .Where(stopped => stopped)
          .Subscribe(stopped => Moving())
          .AddTo(this);


        /// <-���� ��ġ�� ���� �Ǿ����� ���Ǵ� �Լ��Դϴ�.->
        //  BuildingManager.Instance.AlterIsChange -= AlterIsChanged;
        //  BuildingManager.Instance.AlterIsChange += AlterIsChanged;
    }
    /*
    bool IsArrive = false;
    private void Update()
    {
        if (IsArrive == false)
        {
            if (navMeshAgent.velocity.sqrMagnitude >=0.2f && navMeshAgent.remainingDistance <= 0.5f)
            {
                Debug.Log("��������!");
                IsArrive = true;
            }
              
        }

    }
    */
    // 죽는거
    public override void DeadSignal()
    {
        //base.DeadSignal();
        IsMoving = false;
 
        if(!(targetBefore.CompareTag("alter"))) //알터로 돌아가는게 아니라면(지으러 가는도중에 쥬금)
        {
            BuildingPooling.instance.ReturnObject(targetBuilding);
            BuildingPooling.instance.ReturnObject(targetBefore);
            if (targetBefore.CompareTag("workshop")) //심지어 워크샵 지으러 가는중이었다면
            {
                BuildingManager.Instance.GetEssenceSpot().SetActive(true); //자원지 다시 보이도록

            }
        }
        WhiteFreaksManager.Instance.SignOfWhiteFreaksDecrease();
        WhiteFreaksManager.Instance.ReturnWhiteFreaks(go);


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
                    WhiteFreaksManager.Instance.SignOfWhiteFreaksDecrease();
                    return true;
                }
            }
        }
        return false;
    }

    Building buildng;
    void Moving()
    {
        IsMoving = true;
        if (IsBuilding == true)
        {
            buildng = targetBuilding.GetComponent<Building>();
            buildng.ChangeBuilding();
            go.SetActive(false);
            IsBuilding = false;
            
            //switch�� �� �κп��ٰ� ���� ����
            //
        }
        else
        {
            WhiteFreaksManager.Instance.ReturnWhiteFreaks(go);
        }
    }

    private void ChkNavMesh()
    {
        if (navMeshAgent == null)
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

    }

    public void SetDestination(GameObject target, bool isBuilding)
    {
        IsMoving = false;
        // go.SetActive(true);
        IsBuilding = isBuilding;
        targetBuilding = target;


        ChkNavMesh();
        navMeshAgent.SetDestination(new Vector3(target.transform.position.x + 1, target.transform.position.y + 2, target.transform.position.z));
    }


}