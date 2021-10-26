using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WarriorAnims
{
    public class HeroMovement : SuperStateMachine, IStatus
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

        private Vector3 targetPos;
        private Vector3 velocity;
        private Vector3 TowardVec;
        private Vector3 UseSkillTowardVector;

        private bool SkillStop = false;
        private bool isAction = false;
        private int nowAnimationState = 0;

        private Stat stat = new Stat();
        protected override void Init()
        {
            stat.tag = "player";
            stat.attack = 10;
            stat.health = 100;
        }

        public float GetHealth()
        {
            return stat.health;
        }

        public float GetPrice()
        {
            return stat.price;
        }
        private void Awake()
        {
            TowardVec = transform.position;
            targetPos = transform.position;

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

            Init();
        }


        private void Update()
        {
            CharacterMovement();
            ChooseCoroutine();
        }


        private void ChooseCoroutine()
        {
            if (isAction)
            {
                return;
            }
                
            if (stat.health <= 0)
            {
                StartCoroutine("Die");
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                waronSkillManage.UseSkillNumber = 1;
                nowAnimationState = (int)AnimationState.Q;
                ThrowingRock();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                waronSkillManage.UseSkillNumber = 2;
                nowAnimationState = (int)AnimationState.W;
                LeafOfPride();
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                waronSkillManage.UseSkillNumber = 3;
                nowAnimationState = (int)AnimationState.E;
                BoldRush();
            }
            else if(Input.GetKeyDown(KeyCode.R))
            {
                waronSkillManage.UseSkillNumber = 4;
                nowAnimationState = (int)AnimationState.R;
                ShockOfLand();
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
            StartCoroutine(ThrowingRock_Stop());
        }
        IEnumerator ThrowingRock_Stop()
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack1") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f);
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack1")) //Q스킬 모션이 끝나면
            animator.SetBool("Attack", false);
            isAction = false;
            nowAnimationState = (int)AnimationState.L;
            waronSkillManage.UseSkillNumber = 0;
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
            StartCoroutine(LeafOfPride_Stop());
        }
        IEnumerator LeafOfPride_Stop()
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dash-Forward") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.60f);
            waronSkillManage.SkillOnTrigger();
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dash-Forward") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f);
            rigid.velocity = Vector3.zero;
            animator.SetBool("Dash", false);
            isAction = false;
            nowAnimationState = (int)AnimationState.L;
            SetAnimatorRootMotion(false);
            waronSkillManage.UseSkillNumber = 0;
            waronSkillManage.AllColliderOff();
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
            StartCoroutine(BoldRush_Stop());
        }
        IEnumerator BoldRush_Stop()
        {
            waronSkillManage.SkillOnTrigger();
            yield return new WaitUntil(() => (animator.GetCurrentAnimatorStateInfo(0).IsName("MoveAttack1") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f) || SkillStop);
            rigid.velocity = Vector3.zero;
            animator.SetBool("Attack", false);
            isAction = false;
            SkillStop = false;
            nowAnimationState = (int)AnimationState.L;
            SetAnimatorRootMotion(false);
            waronSkillManage.UseSkillNumber = 0;
            waronSkillManage.AllColliderOff();
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
            StartCoroutine(ShockOfLand_Stop());
        }
        IEnumerator ShockOfLand_Stop()
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("SpecialAttack1") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f); 
            rigid.velocity = Vector3.zero;
            animator.SetBool("Attack", false);
            isAction = false;
            nowAnimationState = (int)AnimationState.L;
            waronSkillManage.UseSkillNumber = 0;
        }
        #endregion R_Skill


        IEnumerator Die()
        {
            //사망시 현재 위치로 destination을 변경하고
            //Animator의 변수들을 변경해줍니다.
            SetDestination(transform.position);
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            animator.SetBool("Moving", false);

            //isAction Flag를 true로 변경하고 
            //Animator의 변수들을 변경해줍니다.
            isAction = true;
            animator.SetBool("Damaged", true);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 6);

            yield return new WaitForSeconds(2f);
            StartCoroutine("Revive");
        }


        IEnumerator Revive()
        {
            //isAction Flag를 False로 변경하고 
            //Animator의 변수들을 변경해줍니다.
            //캐릭터의 Hp를 400으로 변경해줍니다.
            animator.SetInteger("TriggerNumber", 7);
            animator.SetTrigger("Trigger");
            animator.SetBool("Damaged", false);
            stat.health = 400;
            isAction = false;
            yield return null;
        }


        public void SetAnimatorRootMotion(bool b)
        {
            useRootMotion = b;
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
                    targetPos = new Vector3(hit.point.x , transform.position.y, hit.point.z);
                    SetDestination(targetPos);
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
            
            if(Vector3.Distance(transform.position, targetPos) <= 0.5f)
            {
                animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
                animator.SetBool("Moving", false);
            }
        }


        public void Break()
        {
            SkillStop = true;
            SetAnimatorRootMotion(false);
        }
    }
}