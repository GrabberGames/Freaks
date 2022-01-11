using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WarriorAnims
{
    public class HeroMovement : SuperStateMachine
    {
        private enum PlayerState
        {
            Attack,
            Die,
            Moving,
            Idle,
            Q,
            W,
            E,
            R,
        }

        public Warrior warrior;

        [HideInInspector] public SuperCharacterController superCharacterController;
        [HideInInspector] public WarriorMovementController warriorMovementController;
        [HideInInspector] public WarriorInputController warriorInputController;
        [HideInInspector] public WarriorInputSystemController warriorInputSystemController;
        [HideInInspector] public WarriorTiming warriorTiming;
        [HideInInspector] public Animator animator;
        [HideInInspector] public IKHands ikHands;
        [HideInInspector] public bool useRootMotion = false;
        [HideInInspector] public bool waitingOnWeapons = true;

        private Rigidbody rigid;
        private Camera mainCamera;
        private NavMeshAgent agent;
        private WaronSkillManage waronSkillManage;

        //
        private Vector3 velocity;
        private Vector3 dir;
        private Vector3 look_dir;
        PlayerState _state = PlayerState.Idle;
        //
        private bool SkillStop = false;
        private bool isAction = false;

        //skill cooltime variable
        float t_time = 0.0f;
        float q_time = 0.0f;
        float w_time = 0.0f;
        float e_time = 0.0f;
        float r_time = 0.0f;
        bool press = false;
        WaitForSeconds seconds = new WaitForSeconds(0.1f);

        //skill vfx prefabs 
        public GameObject R_particle;

        void Activation(string skill)
        {
            switch (skill)
            {
                case "Q":
                    q_time = 6.0f;
                    break;

                case "W":
                    w_time = 12.0f;
                    break;

                case "E":
                    e_time = 14.0f;
                    break;

                case "R":
                    r_time = 120.0f;
                    break;
            }
            t_time = Mathf.Max(q_time, w_time, e_time, r_time, t_time);
            //이미 실행 중이라면
            if (press == true)
            {
            }
            //코루틴 처음 시작하면
            else
            {
                press = true;
                StartCoroutine(Skill_CoolTime());
            }
        }
        IEnumerator Skill_CoolTime()
        {
            while (t_time > 0)
            {
                if (q_time > 0.1f)
                {
                    q_time -= 0.1f;
                }
                if (w_time > 0.1f)
                {
                    w_time -= 0.1f;
                }
                if (e_time > 0.1f)
                {
                    e_time -= 0.1f;
                }
                if (r_time > 0.1f)
                {
                    r_time -= 0.1f;
                    if(R_particle.activeInHierarchy)
                    {
                        if(r_time < 96.0f && r_time > 95.9f)
                        {
                            StartCoroutine(R_Shader_Value_Change(0.4f));
                        }
                        if(r_time < 95.0f)
                        {
                            R_particle.SetActive(false);
                        }
                    }
                }
                if (t_time < 0.1f)
                {
                    t_time = 0.1f;
                    press = false;
                }
                t_time -= 0.1f;
                yield return seconds;
            }
        }

        private void Awake()
        {

            animator = GetComponentInChildren<Animator>();

            if (animator == null)
            {
                Debug.LogError("ERROR: There is no Animator component for character.");
                Debug.Break();
            }
            else
            {
                animator.gameObject.AddComponent<WarriorCharacterAnimatorEvents>();
                animator.gameObject.AddComponent<AnimatorParentMove>();

                animator.GetComponent<WarriorCharacterAnimatorEvents>().heroMovement = this;
                animator.GetComponent<AnimatorParentMove>().animator = animator;
                animator.GetComponent<AnimatorParentMove>().heroMovement = this;

                animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
                animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            }

            agent = GetComponent<NavMeshAgent>();
            rigid = GetComponent<Rigidbody>();
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            warriorTiming = gameObject.AddComponent<WarriorTiming>();
            warriorTiming.heroMovement = this;
            agent.updateRotation = false;
            waronSkillManage = GetComponentInChildren<WaronSkillManage>();

        }

        private void Update()
        {
            ChooseAction();
            switch (_state)
            {
                case PlayerState.Q:
                case PlayerState.W:
                case PlayerState.E:
                case PlayerState.R:
                    break;
                case PlayerState.Moving:
                    UpdateMoving();
                    break;

                case PlayerState.Die:
                    UpdateDie();
                    break;

                case PlayerState.Attack:
                    break;

                case PlayerState.Idle:
                    UpdateIdle();
                    break;

            }
        }

        private void UpdateDie()
        {

        }
        private void UpdateIdle()
        {
            agent.velocity = Vector3.zero;
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            animator.SetBool("Moving", false);
            if (Input.GetMouseButtonDown(1))
            {
                agent.velocity = Vector3.zero;
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building");
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                {
                    dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    agent.SetDestination(dir);
                    animator.SetBool("Moving", true); 
                    velocity = Vector3.MoveTowards(transform.position, dir, agent.speed * Time.deltaTime);
                    animator.SetFloat("Velocity Z", velocity.magnitude);
                    _state = PlayerState.Moving;
                }
            }
        }

        private void UpdateMoving()
        {
            velocity = Vector3.MoveTowards(transform.position, dir, agent.speed * Time.deltaTime);
            look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
            if ((dir-transform.position).magnitude < 0.1f)
            {
                animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
                animator.SetBool("Moving", false);
                _state = PlayerState.Idle;
            }

            transform.LookAt(look_dir);

            if (Input.GetMouseButtonDown(1))
            {
                agent.velocity = Vector3.zero;
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building");
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                {
                    dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    agent.SetDestination(dir);
                }
            }
        }
        void ChangRotate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                look_dir = hit.point;
                look_dir.y = transform.position.y;
                transform.LookAt(look_dir);
            }
        }
        void ChooseAction()
        {
            if (isAction)
                return;

            //Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (q_time > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("Q");
                ThrowingRock();
                _state = PlayerState.Q;
            }
            //W

            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (w_time > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("W");
                LeafOfPride();
                _state = PlayerState.W;
            }
            //E

            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (e_time > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("E");
                BoldRush();
                _state = PlayerState.E;
            }
            //R
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (r_time > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("R");
                ShockOfLand();
                _state = PlayerState.R;
            }
        }


        #region Q_Skill
        void ThrowingRock()
        {
            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetInteger("TriggerNumber", 12);
            animator.SetTrigger("Trigger");
        }
        public void ThrowingRock_Stop()
        {
            rigid.velocity = Vector3.zero;
            animator.SetBool("Attack", false);
            isAction = false;
            SetAnimatorRootMotion(false);
            waronSkillManage.UseSkillNumber = 0;
            animator.SetBool("Moving", true);
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            _state = PlayerState.Idle;
        }
        #endregion Q_Skill


        #region W_Skill
        void LeafOfPride()
        { 
            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false); 
            SetAnimatorRootMotion(true);
            animator.SetBool("Dash", true);
            animator.SetInteger("Action", 1);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 3);
        }
        public void LeafOfPride_Stop_1()
        {
            waronSkillManage.SkillOnTrigger();
        }
        public void LeafOfPride_Stop_2()
        {
            rigid.velocity = Vector3.zero;
            animator.SetBool("Dash", false);
            isAction = false;
            SetAnimatorRootMotion(false);
            waronSkillManage.UseSkillNumber = 0;
            waronSkillManage.AllColliderOff();
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            _state = PlayerState.Idle;
        }
        #endregion W_Skill


        #region E_Skill
        void BoldRush()
        {
            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false); 
            SetAnimatorRootMotion(true);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 11); 
        }
        public void BoldRush_Stop()
        {
            waronSkillManage.SkillOnTrigger();
            rigid.velocity = Vector3.zero;
            animator.SetBool("Attack", false);
            isAction = false;
            SkillStop = false;
            SetAnimatorRootMotion(false);
            waronSkillManage.UseSkillNumber = 0;
            waronSkillManage.AllColliderOff();
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            _state = PlayerState.Idle;
        }
        #endregion E_Skill


        #region R_Skill
        void ShockOfLand()
        {
            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 10);
        }
        public void ShockOfLand_Stop()
        {
            rigid.velocity = Vector3.zero;
            animator.SetBool("Attack", false);
            isAction = false;
            waronSkillManage.UseSkillNumber = 0;
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            R_particle.SetActive(true);
            StartCoroutine(R_Shader_Value_Change(0.1f));
            _state = PlayerState.Idle;
        }
        IEnumerator R_Shader_Value_Change(float _value)
        {
            yield return new WaitForSeconds(1f);
            //Debug.Log(R_particle.transform.GetChild(0).transform.GetChild(0) + " " + gameObject);
            //print(R_particle.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.GetFloat("Mask Clip Value"));

            R_particle.transform.GetChild(0).GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_Cutoff", _value);
        }
        #endregion R_Skill


        public void SetAnimatorRootMotion(bool b)
        {
            useRootMotion = b;
        }

        public void Break()
        {
            SkillStop = true;
            SetAnimatorRootMotion(false);
        }
    }
}