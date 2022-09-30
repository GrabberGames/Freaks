using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;

public class WhiteFreaksController : Stat, ITarget
{
    public GameObject targetBuilding;
    public GameObject targetBefore;
    public NavMeshAgent navMeshAgent;
    public GameObject buildingGO;

    [SerializeField]
    private  AudioSource deadSound;

    public GameObject connectEssence;
    [SerializeField]
    public GameObject alter;

    private Vector3 alterPosition;

    public bool IsBuilding = false;
    public bool IsMoving = false;
    private IDisposable arriveStream = default;

    GameObject go;

    [SerializeField] private GameObject circle;
    public void OpenCircle()
    {
        circle.SetActive(true);
    }

    public void CloseCircle()
    {
        circle.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        go = this.gameObject;
        navMeshAgent = GetComponent<NavMeshAgent>();
        IsMoving = false;
        alter = BuildingManager.Instance.Alter;

        alterPosition = alter.transform.position;
        SetHpBar();
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
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    OpenCircle();
                }
                else
                {
                    CloseCircle();
                }
            }
        }
        
        
        
        
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
        StartCoroutine(Dead());

    }
 

    IEnumerator Dead()
    {
        navMeshAgent.Stop();
        deadSound.Play();
        yield return YieldInstructionCache.WaitForSeconds(1.3f); ;

        BarPooling.instance.ReturnObject(hpBar);
        
        
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
            WhiteFreaksManager.Instance.ReturnWhiteFreaks(this.gameObject);
            isFirst = false;
        }

        yield return null;
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
        IsMoving = false;
        if (IsBuilding == true)
        {
           // buildng = targetBuilding.GetComponent<Building>();
            buildingGO.GetComponent<Building>().ChangeBuilding();
            go.SetActive(false);
            hpBar.SetActive(false);
            IsBuilding = false;

            IsArrive = false;
            //switch�� �� �κп��ٰ� ���� ����
            //
        }
        else
        {
            IsArrive = false;
            WhiteFreaksManager.Instance.ReturnWhiteFreaks();
            ObjectPooling.Instance.ReturnObject(this.gameObject);
            BarPooling.instance.ReturnObject(hpBar);
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
    public GameObject hpBar;
    public Vector3 hpBarOffset = new Vector3(0, 5, 0);
    private RectTransform rect;
    private Image hpBarImage;
    private Text hpBarText;
    void SetHpBar()
    {
        hpBar = BarPooling.instance.GetObject(BarPooling.bar_name.ally_bar);
        rect = (RectTransform)hpBar.transform;
        rect.sizeDelta = new Vector2(89, 21);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarImage.rectTransform.sizeDelta = rect.sizeDelta;
        hpBarText = hpBar.GetComponentsInChildren<Text>()[0];
        hpBarText.text = HP.ToString();
        var _hpbar = hpBar.GetComponent<HpBar>();
        _hpbar.target = this.gameObject;
        _hpbar.offset = hpBarOffset;
        _hpbar.what = HpBar.targets.Freaks;
    }

    public override void OnAttackSignal()
    {
        hpBarImage.fillAmount = HP / MAX_HP;
        if(HP>=0)
        hpBarText.text = HP.ToString();
    }



}