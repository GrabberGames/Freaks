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
                    break;
                case FreaksState.Die:
                    break;
                case FreaksState.Moving:
                    break;
                case FreaksState.Stuern:
                    break;
            }
        }
    }

    NavMeshAgent agent;

    private bool isStuern = false;

    Vector3 alterPosition;
    protected override void Init()
    {
        base.Init();

        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(alterPosition);

    }

    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        switch (state)
        {
            case FreaksState.Attack:

                break;
            case FreaksState.Die:
                break;
            case FreaksState.Moving:
                UpdateMoving();
                break;
            case FreaksState.Stuern:
                break;
        }
    }
    void UpdateMoving()
    {
        LayerMask mask = LayerMask.GetMask("Player") | LayerMask.GetMask("WhiteFreaks");
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 25f, mask))
        {
            print(collider.gameObject.name);
            State = FreaksState.Attack;
            break;
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
        isStuern = true;
        yield return new WaitForSeconds(value);
        isStuern = false;
    }
    public void CallDamaged()
    {

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