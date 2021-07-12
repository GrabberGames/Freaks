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


        private bool isFirst = false;
        private bool isMove = false;
        private bool isAction = false;
        public float groundFriction = 50f;

        CharacterStat characterStat = new CharacterStat();

        private void Awake()
        {
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
            //print(isFirst);
        }

        void ChooseCoroutine()
        {
            if (isAction)
                return;
            if(characterStat.Hp <= 0)
            {
                StartCoroutine("Die");
                //print("!");
            }
        }

        IEnumerator Die()
        {
            //print("*");
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
        IEnumerator Moving(bool is_)
        {
            animator.SetBool("Moving", is_);
            yield return null;
        }

        void CharacterMovement()
        {
            if (Input.GetMouseButtonDown(1))
            {
                agent.velocity = Vector3.zero; isMove = true; isFirst = true;
            }
            if (Input.GetMouseButton(0))
            {
                characterStat.Hp = 0;
            }
            else if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                    SetDestination(hit.point);
            }
            
            Move();

        }

        void SetDestination(Vector3 dest)
        {
            agent.SetDestination(dest);
            destination = dest;
            animator.SetBool("Moving", true);
        }
        void Move()
        {
            if (isMove)
            {
                var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
                velocity = Vector3.MoveTowards(transform.position, dir, agent.speed * Time.deltaTime);
                animator.SetFloat("Velocity Z", velocity.magnitude);
                transform.forward = new Vector3(dir.x, 0, dir.z);
                //transform.position += dir.normalized * Time.deltaTime * agent.speed;
            }
            if (agent.velocity.magnitude == 0f)
            {
                if (isFirst)
                {
                    isFirst = false;
                    //print("*");
                    return;
                }
                animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
                animator.SetBool("Moving", false);
                isMove = false;
                //print("!");
            }
        }
    }
}
