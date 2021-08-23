using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Kali : MonoBehaviour
{
    private enum AnimationState
    {
        L,
        Q,
        W,
        E,
        R,
        D
    }
    private bool isAction;
    private bool canNormalAttack;
    private int AttackNum;
    private Vector3 TowardVec;
    private Vector3 targetPos;

    private Animator animator;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        rigid = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator Attack()
    {
        if(Input.GetMouseButtonDown(0) && AttackNum == 0)
        {
            animator.SetBool("Attack", true);
            AttackNum++;
        }
        else if(Input.GetMouseButtonDown(0) && AttackNum == 1)
        {
            animator.SetTrigger("Trigger");
            AttackNum++;
        }

        yield return new WaitForSeconds(1f);
    }
    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
    }
    private void CharacterMovement()
    {
        //현재 다른 동작 중이라면 움직임을 제한시킵니다.
        if (isAction)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            agent.velocity = Vector3.zero;
            RaycastHit hit;

            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                agent.SetDestination(targetPos);
                animator.SetBool("Moving", true);
            }
        }
        Move();
    }
    void Move()
    {
        var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;

        if (dir != Vector3.zero)
        {
            TowardVec = dir;
        }
        transform.forward = new Vector3(TowardVec.x, 0, TowardVec.z);

        if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
        {
            animator.SetBool("Moving", false);
        }
    }
}
