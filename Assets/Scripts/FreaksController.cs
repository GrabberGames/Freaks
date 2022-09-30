using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;
public class FreaksController : Stat, ITarget
{    
    [SerializeField] private GameObject circle;
    public void OpenCircle()
    {
        circle.SetActive(true);
    }

    public void CloseCircle()
    {
        circle.SetActive(false);
    }

    
    
    public enum FreaksState
    {
        Attack,
        Die,
        Moving,
        Stuern,
        Slow,
    }

    FreaksState state = FreaksState.Moving;
    public FreaksState State
    {
        get { return state; }
        set
        {
            state = value;
            switch(state)
            {
                case FreaksState.Attack:
                    canNormalAttack = true;
                    break;
                case FreaksState.Die:

                    break;
                case FreaksState.Moving:
                    animator.CrossFade("Moving", 0.1f, -1, 0);
                    //Moving���� �ٲ�� Attack->Moving or Stuern->Moving�̹Ƿ� 
                    break;
                case FreaksState.Stuern:
                    agent.isStopped = true;
                    stuernEffect.SetActive(true);
                    break;
                case FreaksState.Slow:
                    slowEffect.SetActive(true);
                    break;
            }
        }
    }
    Animator animator;
    NavMeshAgent agent;
    GameObject alter;
    GameObject target = null;

    [SerializeField]
    GameObject stuernEffect;
    [SerializeField]
    GameObject slowEffect;

    bool canNormalAttack = true;

    const int GetEssenceByKilling = 10;


    float d = 625f;

    GameController _gameController;
    protected override void Init()
    {
        base.Init();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = MOVE_SPEED;

        alter = GameManager.Instance.Alter;

        agent.SetDestination(alter.transform.position);

        animator = GetComponent<Animator>();

        State = FreaksState.Moving;
       
    }

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        Init();

        /// <-���� ��ġ�� ���� �Ǿ����� ���Ǵ� �Լ��Դϴ�.->
        BuildingManager.Instance.AlterIsChange -= AlterIsChanged;
        BuildingManager.Instance.AlterIsChange += AlterIsChanged;
    }
    public void Spawned() 
    {
        Init();
        SetHpBar();
    }
    void AlterIsChanged(GameObject go)
    {
        this.alter = go;
        GoToAlter();
    }
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
                    Debug.Log("OpenCircle");
                    OpenCircle();
                }
                else
                {
                    Debug.Log("CloseCircle");
                    CloseCircle();
                }
            }
        }
        
        switch (state)
        {
            case FreaksState.Attack:
                UpdateAttack();
                break;
            case FreaksState.Die:
                break;
            case FreaksState.Moving :
                UpdateMoving();
                break;
            case FreaksState.Stuern :
                break;
        }
    }
   
    void UpdateAttack()
    {
        if (canNormalAttack == false || target == null)
        {
            return;
        }
        transform.LookAt(target.transform);
        Animator animator = GetComponent<Animator>();
        animator.CrossFade("Attack", 0.1f);
        //agent.Stop();
        GameManager.Damage.OnAttacked(PD, target.GetComponent<Stat>());
        canNormalAttack = false;
    }
    public void CanNormalAttackChange()
    {
        State = FreaksState.Moving;
    }
    void UpdateMoving()
    {
        canNormalAttack = true;
        if (target != null) //�⺻ ���� �� ����� �ִ�.
        {
            if(target.GetComponent<Stat>().HP <= 0)
            {
                target = null;
                State = FreaksState.Moving;
                return;
            }
            
            float distance = (target.transform.position - transform.position).sqrMagnitude;
            agent.areaMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1<< 5;
            //�þ� ������ ������ target�� �ʱ�ȭ����.
            //alterPosition���� destination ������ ����.
            if (distance > 625)
            {
                GoToAlter();
                target = null;
            }
            else if (distance <= 625 && distance > ATTACK_RANGE * ATTACK_RANGE)
            {
                Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                agent.SetDestination(dir);
            }
            //target�� ���� ���� �ȿ� ������ Attack���� ����
            if (distance <= ATTACK_RANGE * ATTACK_RANGE)
            {
                State = FreaksState.Attack;
                return; 
            }
        }

        if(target == null)
        {
            if(agent.areaMask == (1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5))
            {
                LayerMask Lmask = LayerMask.GetMask("NoneFreaksWay");
                if (Physics.Raycast(transform.position, Vector3.up, 5000f, Lmask))
                {
                    agent.areaMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5;
                    agent.SetDestination(alter.transform.position);
                }
                else
                {
                    agent.areaMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3;
                    agent.SetDestination(alter.transform.position);
                }
            }
        }

        //�⺻ ������ ����� ����.
        if (agent.isStopped)
            GoToAlter();

        Vector3 look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        transform.LookAt(look_dir);
        //���� ���� ���� ���� ���� ��� target���� �������ش�.
        LayerMask mask = LayerMask.GetMask("player") | LayerMask.GetMask("whitefreaks") | LayerMask.GetMask("Alter") | LayerMask.GetMask("Friendly");
        if (target != null)
            return;

        if (a)
            d = 2200;

        foreach (Collider collider in Physics.OverlapSphere(transform.position, d, mask))
        {
            if (collider.GetComponent<Stat>().HP > 0)
            {
                target = collider.gameObject;
                Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                agent.SetDestination(dir);
                return;
            }
        }
        
        if (a)
            d = 625f;
    }

    void GoToAlter()
    {
        if(gameObject.activeInHierarchy)
            agent.SetDestination(alter.transform.position);
    }

    public IEnumerator MoveSpeedSlow(float value)
    {
        State = FreaksState.Slow;
        agent.speed = MOVE_SPEED * value;
        yield return new WaitForSeconds(1.5f);
        agent.speed = MOVE_SPEED;
        slowEffect.SetActive(false);
        State = FreaksState.Moving;
    }

    public IEnumerator Stuern(float value)
    {
        State = FreaksState.Stuern;
        yield return new WaitForSeconds(value);
        agent.isStopped = false;
        stuernEffect.SetActive(false);
        State = FreaksState.Moving;
    }

    public override void DeadSignal()
    {
        base.DeadSignal();
        StageManager.Instance.AddEssence(GetEssenceByKilling);
        _gameController.SignOfFreaksDead();
        ObjectPooling.Instance.ReturnObject(gameObject);
        BarPooling.instance.ReturnObject(hpBar);
    }



    public GameObject hpBar;
    public Vector3 hpBarOffset = new Vector3(0, 5, 0);
    private RectTransform rect;
    private Image hpBarImage;
    private Text hpBarText;
    void SetHpBar()
    {
        hpBar = BarPooling.instance.GetObject(BarPooling.bar_name.enemy_bar);
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
    public bool a = false;
    public override void OnAttackSignal()
    {
        a = true;
        hpBarImage.fillAmount = HP / MAX_HP;
        if (HP >= 0)
            hpBarText.text = HP.ToString();
    }
   

}