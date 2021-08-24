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
    private bool isAction = false;
    private bool canNormalAttack = true;
    private float canNormalAttackTime = 2f;
    private int AttackNum = 0;
    private Vector3 TowardVec;
    private Vector3 targetPos;

    //public Avatar[] avatars;
    //public Avatar avatar;
    private Animator animator;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

        //avatar = GetComponentInChildren<Avatar>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        rigid = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //avatar = avatars[0];
    }
    void ChooseAction()
    {
        // Normal Attack
        Basic_Attack();

        //Q

        //W

        //E
        
        //R
    }
    void Basic_Attack()
    {
        print(!canNormalAttack + " " + AttackNum);
        if (!canNormalAttack)
            return;
        if(Input.GetMouseButtonDown(0) && AttackNum == 0 && canNormalAttack)
        {
            //avatar = avatars[1];
            AttackNum++;
            StartCoroutine(CoolTime(canNormalAttackTime, canNormalAttack, "Attack", "Trigger"));
        }
        if(Input.GetMouseButtonDown(0) && AttackNum == 2 && canNormalAttack)
        {
            canNormalAttack = false;
            animator.SetTrigger("Trigger");
            AttackNum++;
            StopCoroutine (CoolTime(canNormalAttackTime, canNormalAttack, "Attack", "Trigger"));

            StartCoroutine(CoolTime(canNormalAttackTime, canNormalAttack, "Attack", "Trigger"));
        }
    }
    IEnumerator CoolTime(float time, bool b, string str, string tri)
    {
        yield return new WaitForSeconds(time);
        b = !b;
        animator.SetBool(str, !b);
        animator.SetTrigger(tri);
    }
    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
        ChooseAction();
        print(animator.GetBool("Moving"));
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
