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


        private bool isFirst = false;
        private bool isMove = false;
        private bool isAction = false;

        CharacterStat characterStat = new CharacterStat();

        private void Awake()
        {
            targetPos = transform.position;
            SetCharacterStat();
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
        public override void SetCharacterStat()
        {
            characterStat.Hp = 400;
            characterStat.Mp = 100;
            characterStat.MoveSpeed = 10;
            characterStat.AttackSpeed = 1;
            characterStat.Armor = 10;
        }
        private void Update()
        {
            ChooseCoroutine();
            CharacterMovement();
        }

        void ChooseCoroutine()
        {
            if (isAction)
                return;
            if(characterStat.Hp <= 0)
            {
                StartCoroutine("Die");
            }
        }

        IEnumerator Die()
        {
            SetDestination(transform.position);
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            animator.SetBool("Moving", false);

            isAction = true;
            animator.SetBool("Damaged", true);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 6);

            yield return new WaitForSeconds(2f);
            StartCoroutine("Revive");
        }
        IEnumerator Revive()
        {
            animator.SetInteger("TriggerNumber", 7);
            animator.SetTrigger("Trigger");
            animator.SetBool("Damaged", false);
            characterStat.Hp = 400;
            isAction = false;
            yield return null;
        }

        void CharacterMovement()
        {
            if (isAction)
                return;
            if (Input.GetMouseButton(0))
            {
                characterStat.Hp = 0;
            }
            if (Input.GetMouseButtonDown(1))
            {
                agent.velocity = Vector3.zero;
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    targetPos = hit.point;
                    SetDestination(hit.point);
                }
            }
            Move();
        }

        void SetDestination(Vector3 dest)
        {
            agent.SetDestination(dest);
            animator.SetBool("Moving", true);
        }
        void Move()
        {
            var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
            if(dir != Vector3.zero)
            {
                TowardVec = dir;
            }
            velocity = Vector3.MoveTowards(transform.position, dir, agent.speed * Time.deltaTime);
            animator.SetFloat("Velocity Z", velocity.magnitude);
            transform.forward = new Vector3(TowardVec.x, 0, TowardVec.z);
            
            if(Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
                animator.SetBool("Moving", false);
            }
        }
    }
}
