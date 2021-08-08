using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WarriorAnims
{
    public class HeroMovement : SuperStateMachine
    {
        public Warrior warrior;
        private enum AnimationState
        {
            L,
            Q,
            W,
            E,
            R,
            D
        }
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
        private Vector3 UseSkillTowardVector;

        private bool isAction = false;
        private int nowAnimationState = 0;

        CharacterStat characterStat = new CharacterStat();

        private void Awake()
        {
            TowardVec = transform.position;
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
            warriorTiming = gameObject.AddComponent<WarriorTiming>();
            warriorTiming.heroMovement = this;
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
            CharacterMovement();
            ChooseCoroutine();
            CallAnimationStop();
            //ActionManage();
        }

        void ChooseCoroutine()
        {
            if (isAction)
                return;
            if (characterStat.Hp <= 0)
            {
                StartCoroutine("Die");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                nowAnimationState = (int)AnimationState.Q;
                StartCoroutine(ThrowingRock());
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                nowAnimationState = (int)AnimationState.W;
                StartCoroutine(LeafOfPride());
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                nowAnimationState = (int)AnimationState.E;
                StartCoroutine(BoldRush());
            }
            else if(Input.GetKeyDown(KeyCode.R))
            {
                nowAnimationState = (int)AnimationState.R;
                StartCoroutine(ShockOfLand());
            }
        }
        void CallAnimationStop()
        {
            switch (nowAnimationState)
            {
                case 1:
                    ThrowingRock_Stop();
                    break;
                case 2:
                    LeafOfPride_Stop();
                    break;
                case 3:
                    BoldRush_Stop();
                    break;
                case 4:
                    ShockOfLand_Stop();
                    break;
                default:
                    break;

            }
        }
        void ActionManage()
        {
            if (isAction)
            {
                switch (nowAnimationState)
                {
                    case 2:
                        rigid.MovePosition(transform.position + UseSkillTowardVector * 20 * Time.deltaTime);
                        break;
                }
            }
        }
        ///////////////////////
        #region Q_Skill
        IEnumerator ThrowingRock()
        {
            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetInteger("TriggerNumber", 12);
            animator.SetTrigger("Trigger");
            yield return null;
        }
        void ThrowingRock_Stop()
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack1")) //Q스킬 모션이 끝나면
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animator.SetBool("Attack", false);
                agent.isStopped = false;
                isAction = false;
                nowAnimationState = (int)AnimationState.L;
            }
        }
        #endregion Q_Skill
        ///////////////////////
        ///
        ///////////////////////
        #region W_Skill
        IEnumerator LeafOfPride()
        { 
            agent.ResetPath();
                isAction = true;
                //Lock(true, true, true, 0, warriorTiming.TimingLock(warrior, "dash"));
                animator.SetBool("Moving", false); 
                SetAnimatorRootMotion(true);
                animator.SetBool("Dash", true);
                animator.SetInteger("Action", 1);
                animator.SetTrigger("Trigger");
                animator.SetInteger("TriggerNumber", 3);
            yield return null;
        }
        IEnumerator W_Stop(float time)
        {
            yield return new WaitForSeconds(time);
            rigid.velocity = Vector3.zero;
            animator.SetBool("Dash", false);
            isAction = false;
            nowAnimationState = (int)AnimationState.L;
            SetAnimatorRootMotion(false);
        }
        void LeafOfPride_Stop()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                rigid.velocity = Vector3.zero;
                animator.SetBool("Dash", false);
                isAction = false;
                nowAnimationState = (int)AnimationState.L;
                SetAnimatorRootMotion(false);
            }
        }
        #endregion W_Skill

        ///////////////////////
        ///
        ///////////////////////
        #region E_Skill
        IEnumerator BoldRush()
        {
            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false); SetAnimatorRootMotion(true);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 11);
            yield return null;
        }
        void BoldRush_Stop()
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MoveAttack1")) //Q스킬 모션이 끝나면
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animator.SetBool("Attack", false);
                agent.isStopped = false;
                isAction = false;
                nowAnimationState = (int)AnimationState.L;
                SetAnimatorRootMotion(false);
            }
        }
        #endregion E_Skill

        ///////////////////////
        ///
        ///////////////////////
        #region R_Skill
        IEnumerator ShockOfLand()
        {
            isAction = true;
            animator.SetBool("Moving", false);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 10);
            yield return null;
        }
        void ShockOfLand_Stop()
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SpecialAttack1")) //R스킬 모션이 끝나면

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animator.SetBool("Attack", false);
                agent.isStopped = false;
                isAction = false;
                SetDestination(targetPos);
                Move();
                nowAnimationState = (int)AnimationState.L;
            }
        }
        #endregion R_Skill
        ///////////////////////
        ///
        ///////////////////////
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
            characterStat.Hp = 400;
            isAction = false;
            yield return null;
        }

        public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
        {
            StopCoroutine("_Lock");
            StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
        }
        public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
        {
            if (delayTime > 0) yield return new WaitForSeconds(delayTime);
            if (lockMovement) { LockMove(true); }
            if (lockAction) { LockAction(true); }
            if(timed)
            {
                if(lockTime >0)
                {
                    yield return new WaitForSeconds(lockTime);
                    UnLock(lockMovement, lockAction);
                }
            }

        }
        public void LockMove(bool b)
        {
            if(b)
            {
                animator.SetBool("Moving", false);
                SetAnimatorRootMotion(true);
            }
            else
            {
                SetAnimatorRootMotion(false);
            }
        }
        public void LockAction(bool b)
        {
            //isAction = b;
        }
        public void UnLock(bool movement, bool actions)
        {
            if (movement) LockMove(false);
            //if (actions) isAction = false;
        }

        public void SetAnimatorRootMotion(bool b)
        {
            useRootMotion = b;
        }
        void CharacterMovement()
        {
            //현재 다른 동작 중이라면 움직임을 제한시킵니다.
            if (isAction)
                return;
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
            
            if(Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
                animator.SetBool("Moving", false);
            }
        }
    }
}
