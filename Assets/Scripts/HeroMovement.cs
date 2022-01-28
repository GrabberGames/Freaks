using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace WarriorAnims
{
    public class HeroMovement : SuperStateMachine
    {
        public Action WaronRHitted = null;

        #region trash
        [HideInInspector] public SuperCharacterController superCharacterController;
        [HideInInspector] public WarriorMovementController warriorMovementController;
        [HideInInspector] public WarriorInputController warriorInputController;
        [HideInInspector] public WarriorInputSystemController warriorInputSystemController;
        [HideInInspector] public WarriorTiming warriorTiming;
        [HideInInspector] public Animator animator;
        [HideInInspector] public IKHands ikHands;
        [HideInInspector] public bool useRootMotion = false;
        [HideInInspector] public bool waitingOnWeapons = true;
        #endregion

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
                    case PlayerState.Attack:
                        break;
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
        GameObject go = null;
        public GameObject Q_particle;
        public GameObject W_particle;
        public GameObject E_particle;
        public GameObject R_particle;
        //Waron Basic Attack Target Object
        GameObject _lockTarget;

        //Waron Sound Priority Variable & Far Distance Judge
        private int _priority = 0;
        private float _dist = 0f;
        private float _damage = 0f;
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
            if (press == true)
            {
            }
            else
            {
                press = true;
                StartCoroutine(Skill_CoolTime());
            }
        }
        IEnumerator Skill_CoolTime()
        {
            int cnt = 0;
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
                    cnt++;
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
                        else
                        {
                            if (cnt % 10 == 0)
                            {
                                WaronRHitted.Invoke();
                            }
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

        public float getTimer(string type)
        {
            switch (type)
            {
                case "Q":
                    return q_time;
                case "W":
                    return w_time;
                case "E":
                    return e_time;
                case "R":
                    return r_time;
                default:
                    return 0;
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
                ObjectPooling.instance.Set_Stat(gameObject.name, PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);
                PState = PlayerState.Die;
            }
        }
        private void Start()
        {
            Init();
        }

        private void Update()
        {
            ChooseAction();
            switch (PState)
            {
                case PlayerState.Attack:
                    Basic_Attack();
                    break;
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

                case PlayerState.Idle:
                    UpdateIdle();
                    break;
            }
        }
        private bool canNormalAttack = true;
        void Basic_Attack()
        {
            if (!canNormalAttack || _lockTarget == null)
                return;

            transform.LookAt(_lockTarget.transform);
            agent.SetDestination(transform.position);

            animator.SetBool("Moving", false);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetInteger("TriggerNumber", 4);
            animator.SetTrigger("Trigger");

            canNormalAttack = false;
        }
        public void Normal_Attack_Fun()
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Moving", false);
            animator.SetInteger("Action", -1);
            animator.SetInteger("TriggerNumber", 0);
            _lockTarget = null;
            canNormalAttack = true;
            PState = PlayerState.Idle;
        }

        private void UpdateDie()
        {

        }
        private void UpdateIdle()
        {
            agent.velocity = Vector3.zero;
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            animator.SetBool("Moving", true);
            if (Input.GetMouseButtonDown(1))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                agent.velocity = Vector3.zero;
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks");
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                {
                    if (hit.collider.gameObject.layer == (int)Layers.Enemy)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;

                    if (_lockTarget != null) //�⺻ ���� �� ����� �ִ�.
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
            if (_lockTarget != null) //�⺻ ���� �� ����� �ִ�.
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
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                agent.velocity = Vector3.zero;
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks");
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
            StartCoroutine(LeafOfPrideParticle());
            waronSkillManage.SkillOnTrigger(110+0.6f*ED);
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

        IEnumerator LeafOfPrideParticle()
        {
            go = Instantiate(W_particle, this.gameObject.transform);
            ParticleSystem w_ps = go.GetComponent<ParticleSystem>();
            w_ps.Play();
            yield return new WaitForSeconds(w_ps.main.startLifetimeMultiplier);
            Destroy(go);
            go = null;
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
            BoldRushParticleEnd();
            waronSkillManage.SkillOnTrigger(180+0.7f*ED);

            rigid.velocity = Vector3.zero;
            animator.SetBool("Attack", false);
            isAction = false;
            SetAnimatorRootMotion(false);
            waronSkillManage.UseSkillNumber = 0;
            waronSkillManage.AllColliderOff();
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);
            _state = PlayerState.Idle;
        }
        public void BoldRushParticleStart()
        {
            go = Instantiate(E_particle, this.gameObject.transform);
            ParticleSystem e_ps = go.GetComponent<ParticleSystem>();
            e_ps.Play();
        }
        public void BoldRushParticleEnd()
        {
            Destroy(go);
            go = null;
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
            R_particle.GetOrAddComponent<WaronR>().Init(30 + 0.25f * ED);
            StartCoroutine(R_Shader_Value_Change(0.1f));
            _state = PlayerState.Idle;
        }
        IEnumerator R_Shader_Value_Change(float _value)
        {
            yield return new WaitForSeconds(1f);
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
                    if (_priority == 0 || _priority > 2)
                    {
                        _priority = 2;
                        if (idx == 3)
                            _soundname = $"{_name}��ų";
                        else
                            _soundname = $"{_name}��ų " + UnityEngine.Random.Range(1, 3);
                    }
                    break;

                case "사망":
                case "시작":
                case "승리":
                case "부활":
                    if (_priority == 0 || _priority > 1)
                    {
                        _priority = 1;
                        _soundname = $"{_name} " + UnityEngine.Random.Range(1, 3);
                    }
                    break;

            }
            AudioManager.a_instance.Read(_soundname);
        }
    }

}