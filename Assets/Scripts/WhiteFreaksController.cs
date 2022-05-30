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
    public NavMeshAgent navMeshAgent;

    [SerializeField]
    public GameObject alter;

    private Vector3 alterPosition;

    public bool IsBuilding = false;

    private IDisposable arriveStream = default;
    private Action onArriveCallback = default;


    // Start is called before the first frame update
    void Start()
    {
        Init();

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

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
          
        
        /// <-알터 위치가 변경 되었을때 사용되는 함수입니다.->
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
                Debug.Log("도착했음!");
                IsArrive = true;
            }
              
        }

    }
    */
    protected override void Init()
    {
        base.Init();
    }

    private bool IsStopped(Unit unit)
    {
        if (!navMeshAgent.pathPending)
        {
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    return true;
            }
        }
        return false;
    }

    Building buildng;
    void Moving()
    {
        if(IsBuilding == true)
        {
            buildng = targetBuilding.GetComponent<Building>();
            buildng.ChangeBuilding();
            this.gameObject.SetActive(false);
            IsBuilding = false;
            //switch는 이 부분에다가 따로 달자
            //
        }
        else
        {
            WhiteFreaksManager.Instance.ReturnWhiteFreaks(this.gameObject);
        }
    }


    /*
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
*/


    private void ChkNavMesh()
    {
        if (navMeshAgent == null)
        {
     navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        }
    }

    public void SetDestination(GameObject target, bool isBuilding)
    {
        IsBuilding = isBuilding;
        this.targetBuilding = target;
       
        
        ChkNavMesh();
        navMeshAgent.SetDestination(new Vector3(target.transform.position.x + 1, target.transform.position.y + 2, target.transform.position.z));
    }


}