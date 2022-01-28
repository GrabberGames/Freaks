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
    }

    FreaksState state = FreaksState.Moving;
    public FreaksState State
    {
        get { return state; }
        set
        {
            state = value;
            Animator animator = GetComponentInChildren<Animator>();
            switch(state)
            {
                case FreaksState.Attack:
                    animator.Play("Attack");
                    break;
                case FreaksState.Die:

                    break;
                case FreaksState.Moving:
                    animator.CrossFade("Moving", 0.1f, -1, 0);
                    //Moving으로 바뀌면 Attack->Moving or Stuern->Moving이므로 
                    //alterPosition으로 destination setting.
                    agent.SetDestination(alterPosition);
                    break;
                case FreaksState.Stuern:
                    break;
            }
        }
    }

    NavMeshAgent agent;
    Vector3 alterPosition;
    GameObject target = null;
    bool canNormalAttack = true;
    protected override void Init()
    {
        base.Init();

        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(alterPosition);
    }

    private void Start()
    {
        Init();
        State = FreaksState.Moving;
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
            State = FreaksState.Moving;
            return;
        }

        GameManager.Damage.OnAttacked(PD, target.GetComponent<Stat>());
        canNormalAttack = false;
    }
    public void CanNormalAttackChange()
    {
        canNormalAttack = true;
        State = FreaksState.Moving;
    }
    void UpdateMoving()
    {
        print(alterPosition);
        if (target != null) //기본 공격 할 대상이 있다.
        {
            float distance = (target.transform.position - transform.position).magnitude;
            //시야 밖으로 나가면 target을 초기화해줌.
            //alterPosition으로 destination 설정도 해줌.
            if(distance > 25)
            {
                agent.SetDestination(alterPosition);
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
        //기본 공격할 대상이 없다.
        Vector3 look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        transform.LookAt(look_dir);

        //일정 범위 내에 적이 들어올 경우 target으로 설정해준다.
        LayerMask mask = LayerMask.GetMask("player") | LayerMask.GetMask("whitefreaks");
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 25f, mask))
        {
            target = collider.gameObject;
            Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            agent.SetDestination(dir);
            return;
        }
    }
    public IEnumerator MoveSpeedSlow(float value)
    {
        agent.speed = MOVE_SPEED * value;
        yield return new WaitForSeconds(1.5f);
        agent.speed = MOVE_SPEED;
    }

    public IEnumerator Stuern(float value)
    {
        bool isStuern = false;
        isStuern = true;
        yield return new WaitForSeconds(value);
        isStuern = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 7.5f);
    }
    public void ChangeAlterPosition(Vector3 alterPosition)
    {
        this.alterPosition = alterPosition;
    }
}