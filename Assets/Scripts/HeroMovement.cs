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
        PlayerState _state = PlayerState.Idle;
        //
        private bool SkillStop = false;
        private bool isAction = false;
 
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
            print(_state);
            switch (_state)
            {
                case PlayerState.Q:
                case PlayerState.W:
                case PlayerState.E:
                case PlayerState.R:
                    transform.LookAt(dir);
                    return;
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
            dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
            if (dir.magnitude < 0.01f)
            {
                animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
                animator.SetBool("Moving", false);
                _state = PlayerState.Idle;
            }

            transform.LookAt(dir);

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
                dir = hit.point;
                dir.y = transform.position.y;
                transform.LookAt(dir);
            }
        }
        void ChooseAction()
        {
            if (isAction)
                return;

            //Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                useRootMotion = true; ChangRotate();
                ThrowingRock();
                _state = PlayerState.Q;
            }
            //W

            else if (Input.GetKeyDown(KeyCode.W))
            {
                useRootMotion = true; ChangRotate();
                LeafOfPride();
                _state = PlayerState.W;
            }
            //E

            else if (Input.GetKeyDown(KeyCode.E))
            {
                useRootMotion = true; ChangRotate();
                BoldRush();
                _state = PlayerState.E;
            }
            //R
            else if (Input.GetKeyDown(KeyCode.R))
            {
                useRootMotion = true; ChangRotate();
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
            animator.SetBool("Attack", false);
            isAction = false;
            waronSkillManage.UseSkillNumber = 0;
            animator.SetBool("Moving", true);
            _state = PlayerState.Idle;
            print("!");
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
        public void LeafOfPride_Stop()
        {
            //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dash-Forward") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.60f);
            waronSkillManage.SkillOnTrigger();
            //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dash-Forward") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f);
            rigid.velocity = Vector3.zero;
            animator.SetBool("Dash", false);
            isAction = false;
            SetAnimatorRootMotion(false);
            waronSkillManage.UseSkillNumber = 0;
            waronSkillManage.AllColliderOff();
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
            _state = PlayerState.Idle;
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