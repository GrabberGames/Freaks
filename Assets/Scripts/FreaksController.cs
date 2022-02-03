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
            switch(state)
            {
                case FreaksState.Attack:
                    canNormalAttack = true;
                    break;
                case FreaksState.Die:

                    break;
                case FreaksState.Moving:
                    IsOnFreaksWay = false;
                    animator.CrossFade("Moving", 0.1f, -1, 0);
                    //Moving���� �ٲ�� Attack->Moving or Stuern->Moving�̹Ƿ� 
                    break;
                case FreaksState.Stuern:
                    break;
            }
        }
    }
    Animator animator;
    NavMeshAgent agent;
    Vector3 alterPosition;
    GameObject target = null;
    bool canNormalAttack = true;
    bool IsOnFreaksWay = true;
    protected override void Init()
    {
        base.Init();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = MOVE_SPEED;
        agent.SetDestination(alterPosition);

        animator = GetComponent<Animator>();

        State = FreaksState.Moving;
    }

    private void Start()
    {
        Init();
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
        agent.SetDestination(transform.position);
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
            IsOnFreaksWay = false;
            float distance = (target.transform.position - transform.position).magnitude;
            agent.areaMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1<< 5;
            //�þ� ������ ������ target�� �ʱ�ȭ����.
            //alterPosition���� destination ������ ����.
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
            //target�� ���� ���� �ȿ� ������ Attack���� ����
            if (distance <= ATTACK_RANGE)
            {
                State = FreaksState.Attack;
                return; 
            }
        }
        else if (IsOnFreaksWay == false)
        {
            if (Physics.Raycast(transform.position, transform.position + Vector3.down * 2f, 100f, LayerMask.GetMask("NoneFreaksWay")))
            {
                print("!");
                agent.areaMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1<< 5;
                IsOnFreaksWay = false;
            }
            else
            {
                print("@");
                agent.areaMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3; 
                IsOnFreaksWay = true;
            }
        }
        //�⺻ ������ ����� ����.
        if (agent.isStopped)
            GoToAlter();
        Vector3 look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        transform.LookAt(look_dir);
        //���� ���� ���� ���� ���� ��� target���� �������ش�.
        LayerMask mask = LayerMask.GetMask("player") | LayerMask.GetMask("whitefreaks");
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
        agent.SetDestination(alterPosition);
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
    public void ChangeAlterPosition(Vector3 alterPosition)
    {
        this.alterPosition = alterPosition;
    }
}