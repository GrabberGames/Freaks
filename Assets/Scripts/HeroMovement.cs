using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WarriorAnims
{
    public class HeroMovement : SuperStateMachine
    {
        enum Layers
        {
            Enemy = 7,
        }
        public enum PlayerState
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
        PlayerState _state = PlayerState.Idle;
        public PlayerState PState
        {
            get { return _state; }
            set
            {
                _state = value;
                switch (_state)
                {
                    case PlayerState.Idle:
                        break;
                    case PlayerState.Q:
                        break;
                    case PlayerState.W:
                        break;
                    case PlayerState.E:
                        break;
                    case PlayerState.R:
                        break;
                    case PlayerState.Moving:
                        break;
                    case PlayerState.Die:
                        break;
                }

            }
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
        //
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

        //Waron Basic Attack Target Object
        GameObject _lockTarget;

        //Waron Sound Priority Variable & Far Distance Judge
        private int _priority = 0;
        private float _dist = 0f;

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


        protected override void Init()
        {
            base.Init();

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

        //Dead
        IEnumerator DeadAnimationEnd()
        {
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }
        public override void DeadSignal()
        {
            if (HP <= 0)
            {
                PState = PlayerState.Die;
                Stat _stat = new Stat(PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);
                ObjectPooling.instance.Set_Stat(_stat);
            }
        }
        void Start()
        {
            Init();
        }

        private void Update()
        {
            ChooseAction();
            switch (PState)
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
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("Enemy");
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                {
                    if (hit.collider.gameObject.layer == (int)Layers.Enemy)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;

                    if (_lockTarget != null) //기본 공격 할 대상이 있다.
                    {
                        float distance = (_lockTarget.transform.position - transform.position).magnitude;

                        if (distance <= ATTACK_RANGE)
                        {
                            PState = PlayerState.Attack;
                            return;
                        }
                    }
                    _dist = Vector3.Distance(transform.position, hit.point);
                    if (_dist > 120f)
                        SoundPlay("장거리 이동");
                    else
                        SoundPlay("단거리 이동");



                    dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    agent.SetDestination(dir);
                    animator.SetBool("Moving", true); 
                    velocity = Vector3.MoveTowards(transform.position, dir, agent.speed * Time.deltaTime);
                    animator.SetFloat("Velocity Z", velocity.magnitude);
                    PState = PlayerState.Moving;
                }
            }
        }

        private void UpdateMoving()
        {
            if (_lockTarget != null) //기본 공격 할 대상이 있다.
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;

                if (distance <= ATTACK_RANGE)
                {
                    PState = PlayerState.Attack;
                    return;
                }
            }

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
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("Enemy");
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                {
                    if (hit.collider.gameObject.layer == (int)Layers.Enemy)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;


                    _dist = Vector3.Distance(transform.position, hit.point);
                    if (_dist > 120f)
                        SoundPlay("장거리 이동");
                    else
                        SoundPlay("단거리 이동");


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
                PState = PlayerState.Q;
            }
            //W

            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (w_time > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("W");
                LeafOfPride();
                PState = PlayerState.W;
            }
            //E

            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (e_time > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("E");
                BoldRush();
                PState = PlayerState.E;
            }
            //R
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (r_time > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("R");
                ShockOfLand();
                PState = PlayerState.R;
            }
        }


        #region Q_Skill
        void ThrowingRock()
        {
            SoundPlay("Q", 0);

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
            SoundPlay("W", 1);

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
            SoundPlay("E", 2);

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
            SoundPlay("R", 3);

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
            SetAnimatorRootMotion(false);
        }

        public void SoundPlay(string _name, int idx = 0)
        {
            if (AudioManager.a_instance.Check() == true)
                _priority = 0;
            string _soundname = "";
            switch (_name)
            {
                case "장거리 이동":
                case "단거리 이동":
                    //현재 실행중인 사운드가 없으면 실행
                    if (_priority == 0)
                    {
                        _priority = 3;
                        _soundname = $"{_name} " + UnityEngine.Random.Range(1, 11);
                    }
                    break;

                case "스위치":
                    if (idx != 0)
                    {
                        if (idx == 1)
                            _soundname = "첫번째 ";
                        else if (idx == 2)
                            _soundname = "두번째";
                        else if (idx == 3)
                            _soundname = "세번째";
                        _soundname += $"{_name} " + UnityEngine.Random.Range(1, 3);
                    }
                    break;
                case "Q":
                case "W":
                case "E":
                case "R":
                    //현재 실행중인 사운드가 없거나 우선순위 3순위인게 실행중이면 하이재킹.
                    if (_priority == 0 || _priority > 2)
                    {
                        _priority = 2;
                        if (idx == 3)
                            _soundname = $"{_name}스킬";
                        else
                            _soundname = $"{_name}스킬 " + UnityEngine.Random.Range(1, 3);
                    }
                    break;

                case "사망":
                case "시작":
                case "승리":
                case "부활":
                    //현재 실행중인 사운드가 없거나 우선순위 2, 3순위인게 실행중이면 하이재킹.
                    if (_priority == 0 || _priority > 1)
                    {
                        _priority = 1;
                        _soundname = $"{_name} " + UnityEngine.Random.Range(1, 3);
                    }
                    break;

            }
            //실행하기.
            AudioManager.a_instance.Read(_soundname);
        }
    }

}