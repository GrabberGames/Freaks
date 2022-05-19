using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class FreaksController : Stat
{
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
                    //Moving으로 바뀌면 Attack->Moving or Stuern->Moving이므로 
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

        /// <-알터 위치가 변경 되었을때 사용되는 함수입니다.->
        BuildingManager.Instance.AlterIsChange -= AlterIsChanged;
        BuildingManager.Instance.AlterIsChange += AlterIsChanged;
    }
    public void Spawned() 
    {
        Init(); 
    }
    void AlterIsChanged(GameObject go)
    {
        this.alter = go;
        GoToAlter();
    }
    private void Update()
    {
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
        if (target != null) //기본 공격 할 대상이 있다.
        {
            float distance = (target.transform.position - transform.position).magnitude;
            agent.areaMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1<< 5;
            //시야 밖으로 나가면 target을 초기화해줌.
            //alterPosition으로 destination 설정도 해줌.
            if (distance > 25)
            {
                GoToAlter();
                target = null;
            }
            else if (distance <= 25 && distance > ATTACK_RANGE)
            {
                Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                agent.SetDestination(dir);
            }
            //target이 공격 범위 안에 들어오면 Attack으로 변경
            if (distance <= ATTACK_RANGE)
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
        //기본 공격할 대상이 없다.
        if (agent.isStopped)
            GoToAlter();
        Vector3 look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        transform.LookAt(look_dir);
        //일정 범위 내에 적이 들어올 경우 target으로 설정해준다.
        LayerMask mask = LayerMask.GetMask("player") | LayerMask.GetMask("whitefreaks") | LayerMask.GetMask("Alter");
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 25f, mask))
        {
            target = collider.gameObject;
            Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            agent.SetDestination(dir);
            return;
        }
    }
    void GoToAlter()
    {
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
    }
}