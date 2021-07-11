using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WarriorAnims
{
    public class HeroMovement : SuperStateMachine
    {
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

        private Vector3 targetPos;
        private Vector3 velocity;
        private Vector3 TowardVec;
        private Vector3 destination;

        private bool isMove = false;

        public float groundFriction = 50f;


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
                animator.GetComponent<WarriorCharacterAnimatorEvents>().heroMovement = this;
                animator.gameObject.AddComponent<AnimatorParentMove>();
                animator.GetComponent<AnimatorParentMove>().animator = animator;
                animator.GetComponent<AnimatorParentMove>().heroMovement = this;
                animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
                animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            }


            rigid = GetComponent<Rigidbody>();
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
        }
        /*
        // Update is called once per frame
        void Update()
        {
            MoveManage();

            UpdateAnimationSpeed();
        }
        private void UpdateAnimationSpeed()
        {
            SetAnimatorFloat("Animation Speed", animationSpeed);
        }
        void MoveManage()
        {
            //마우스 우클릭 시 캐릭터 이동//
            #region mouse click
            if (Input.GetMouseButtonUp(1))
            {
                velocity = Vector3.zero;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    TowardVec = (targetPos - transform.position).normalized;
                    targetPos = hit.point;
                    agent.SetDestination(targetPos);
                }
            }
            IsRunning(targetPos);
            #endregion
            ///////////////////////////////////
            if(Input.GetButtonDown("Attack"))
            {
                SetAnimatorInt("Action", 1);
                SetAnimatorTrigger("AttackTrigger");
            }
            ///////////////////////////////////

        }

        private void SetAnimatorBool(string name, bool b)
        {
            animator.SetBool(name, b);
        }
        private void SetAnimatorInt(string name, int i)
        {
            animator.SetInteger(name, i);
        }
        private void SetAnimatorFloat(string name, float f)
        {
            animator.SetFloat(name, f);
        }
        private void SetAnimatorTrigger(string name)
        {
            animator.SetTrigger(name);
        }

        private void IsRunning(Vector3 targetPos)
        {
            float dis = Vector3.Distance(transform.position, targetPos);

            if (dis >= 0.3f)
            {
                velocity = Vector3.MoveTowards(velocity, TowardVec * agent.speed, agent.acceleration * Time.deltaTime);
                SetAnimatorBool("Moving", true);
                SetAnimatorFloat("Velocity", velocity.magnitude);
                isRun = true;
            }
            else
            {
                velocity = Vector3.zero;
                SetAnimatorBool("Moving", false);
                SetAnimatorFloat("Velocity", 0);
                isRun = false;
            }
            Turn(targetPos);

            SetAnimatorFloat("Velocity", transform.InverseTransformDirection(velocity).z);
            return;
        }
        private void Turn(Vector3 targetPos)
        {
            Vector3 dir = targetPos - transform.position;
            Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
            Quaternion targetRot = Quaternion.LookRotation(dirXZ);
            rigid.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 55555.0f * Time.deltaTime);
        }
        */
        private void Update()
        {
            CharacterMovement();
            Move();
        }
        void CharacterMovement()
        {
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                    SetDestination(hit.point);
            }
            if (Input.GetMouseButtonDown(1))
            {
                agent.velocity = Vector3.zero;
                /*
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                    SetDestination(hit.point);
                */
            }
        }
        void SetDestination(Vector3 dest)
        {
            agent.SetDestination(dest);
            destination = dest;
            isMove = true;
            animator.SetBool("Moving", true);
        }
        void Move()
        {
            if (isMove)
            {
                if (agent.velocity.magnitude == 0f)
                {
                    isMove = false;
                    animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
                    animator.SetBool("Moving", false);
                    return;
                }
                var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
                velocity = Vector3.MoveTowards(agent.velocity, dir , agent.speed * Time.deltaTime);
                animator.SetFloat("Velocity Z", velocity.magnitude);
                transform.forward = new Vector3(dir.x, 0, dir.z);
                //transform.position += dir.normalized * Time.deltaTime * agent.speed;
            }
        }
    }
}
