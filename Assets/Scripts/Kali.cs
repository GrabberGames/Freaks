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

    private int nowAnimationState = 0;
    private bool useRootMotion = false;
    private Animator animator;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Rigidbody rigid;

    public KailAni kailAni;

    public AudioSource[] audioSource;
    private bool MovingAudioSoungIsActive = false;

    private GameObject R_Skill;
    public GameObject R_Skill_Prefab;
    private void Awake()

    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        rigid = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Start()
    {

    }
    void OnAnimatorMove()
    {
        if (useRootMotion)
        {
            transform.rotation = animator.rootRotation;
            transform.position += animator.deltaPosition;
        }
    }
    public void SetStartPos()
    {
        kailAni.SetStartPosition();
    }
    public void SetEndPos()
    {
        kailAni.SetEndPosition();
    }
    public void SetPos()
    {
        transform.position += kailAni.returnMove();
    }
    void ChooseAction()
    {
        if (isAction)
            return;

        // Normal Attack
        Basic_Attack();

        //Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            useRootMotion = true;
            Determination();
            nowAnimationState = (int)AnimationState.Q;
        }
        //W

        else if (Input.GetKeyDown(KeyCode.W))
        {
            useRootMotion = true;
            Atonement();
            nowAnimationState = (int)AnimationState.W;
        }
        //E

        else if (Input.GetKeyDown(KeyCode.E))
        {
            useRootMotion = true;
            Evation();
            nowAnimationState = (int)AnimationState.E;
        }
        //R
        else if (Input.GetKeyDown(KeyCode.R))
        {
            useRootMotion = true;
            HorizonofMemory();
            nowAnimationState = (int)AnimationState.R;
        }
    }
    #region Q_Skill
    void Determination()
    {
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 1);
        audioSource[0].Play();
        StartCoroutine(Determination_Stop());
    }
    IEnumerator Determination_Stop()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Gun attack3") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f);
        print("*");
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        nowAnimationState = (int)AnimationState.L;
    }
    #endregion
    #region W_Skill
    void Atonement()
    {
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 2);
        audioSource[1].Play();
        StartCoroutine(Atonement_Stop());
    }
    IEnumerator Atonement_Stop()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("TwoGun Attack 05") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f);
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        nowAnimationState = (int)AnimationState.L;
    }
    #endregion
    #region E_Skill
    void Evation()
    {
        agent.ResetPath();
        useRootMotion = true;
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 3);
        audioSource[2].Play();
        StartCoroutine(Evation_Stop());
    }
    IEnumerator Evation_Stop()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Jumbo Back Attack") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f);
        useRootMotion = false;
        isAction = false;
        animator.SetBool("Skill", false);
        nowAnimationState = (int)AnimationState.L;
    }
    #endregion
    #region R_Skill
    void HorizonofMemory()
    {
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 4);
        audioSource[3].Play();
        StartCoroutine(HorizonofMemory_Stop());
    }
    IEnumerator HorizonofMemory_Stop()
    {
        yield return new WaitForSeconds(0.2f);
        audioSource[4].Play();
        R_Skill = Instantiate(R_Skill_Prefab);
        R_Skill.GetComponent<Kail_R>().Trigger(transform.position);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Gun Air Attack") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f);
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        nowAnimationState = (int)AnimationState.L;
    }
    #endregion
    void Basic_Attack()
    {
        if (!canNormalAttack)
            return;
        if(Input.GetMouseButtonDown(0) && canNormalAttack)
        {
            if(AttackNum == 0)
            {
                animator.SetBool("Attack", true);
                animator.SetBool("NormalAttack", true);
                AttackNum = 1;
            }
            else
            {
                animator.SetBool("Attack", true);
                animator.SetBool("NormalAttack", false);
                AttackNum = 0;
            }
            audioSource[5].Play();
            StartCoroutine(CoolTime(canNormalAttackTime, canNormalAttack));
        }
    }
    IEnumerator CoolTime(float time, bool b)
    {
        b = !b;
        yield return new WaitUntil(() => (animator.GetCurrentAnimatorStateInfo(0).IsName("Normal Attack 1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Normal Attack 2")) && !animator.IsInTransition(0));
        animator.SetBool("Attack", false);
        b = !b; 
    }
    void Update()
    {
        CharacterMovement();
        ChooseAction();
        if(animator.GetBool("Moving") && !MovingAudioSoungIsActive)
        {
            MovingAudioSoungIsActive = true;
            StartCoroutine(MoveSound());
        }
        if(animator.GetBool("Moving") == false)
        {
            MovingAudioSoungIsActive = false;
            StopCoroutine(MoveSound());
        }
    }
    IEnumerator MoveSound()
    {
        audioSource[6].Play();
        yield return new WaitForSeconds(2f);
        MovingAudioSoungIsActive = false;

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


        if (Vector3.Distance(transform.position, targetPos) <= 0.5f)
        {
            animator.SetBool("Moving", false);
        }
    }
}
