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
            blackfreaks = 6,
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

        //skill vfx prefabs 
        public GameObject leftArm;
        GameObject rock = null;
        GameObject go = null;
        public GameObject Q_particle;
        public GameObject W_particle;
        public GameObject E_particle;
        public GameObject R_particle;
        //Waron Basic Attack Target Object
        GameObject _lockTarget;

        private Vector3 _mouseHitPosition;

        float qTime = .0f;
        float wTime = .0f;
        float eTime = .0f;
        float rTime = .0f;

        int cnt = 0;
        bool canPlayMoveSound = true;

        IEnumerator RRR;
        PlayerModel PlayerModel => GameManager.Instance.models.playerModel;

        //Waron Sound Priority Variable & Far Distance Judge
        private int _priority = 0;
        private float _dist = 0f;
        void Activation(string skill)
        {
            switch (skill)
            {
                case "Q":
                    PlayerModel.QSkillCoolTime = 6.0f;
                    qTime = Time.time;
                    break;

                case "W":
                    PlayerModel.WSkillCoolTime = 12.0f;
                    wTime = Time.time;
                    break;

                case "E":
                    PlayerModel.ESkillCoolTime = 14.0f;
                    eTime = Time.time;
                    break;

                case "R":
                    PlayerModel.RSkillCoolTime = 120.0f;
                    rTime = Time.time;
                    break;
            }
        }
        protected override void Init()
        {
            base.Init();

            animator = GetComponentInChildren<Animator>();

            if (animator == null)
            {

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

            GameManager.Instance.models.playerModel.PlayerNowHp = HP;
            GameManager.Instance.models.playerModel.PlayerMaxHp = MAX_HP;
            GameManager.Instance.models.playerModel.PlayerPD = PD;
            GameManager.Instance.models.playerModel.PlayerED = ED;
            GameManager.Instance.models.playerModel.PlayerMoveSpeed = MOVE_SPEED;
            GameManager.Instance.models.playerModel.PlayerAttackSpeed = ATTACK_SPEED;
            GameManager.Instance.models.playerModel.PlayerArmor = ARMOR;

            agent.speed = MOVE_SPEED;

            PlayerModel.QSkillMaxCoolTime = 6;
            PlayerModel.WSkillMaxCoolTime = 12;
            PlayerModel.ESkillMaxCoolTime = 14;
            PlayerModel.RSkillMaxCoolTime = 120;

            GameManager.Damage.ChangedHP -= HpToModel;
            GameManager.Damage.ChangedHP += HpToModel;
            GameManager.Instance.models.playerModel.StatChanged -= HpUp;
            GameManager.Instance.models.playerModel.StatChanged += HpUp;
        }
        public void HpUp(StatusType statusType)
        {
            HP = PlayerModel.PlayerNowHp;
            MAX_HP = PlayerModel.PlayerMaxHp;
        }
        public void HpToModel()
        {
            PlayerModel.PlayerNowHp = HP;
            PlayerModel.PlayerMaxHp = MAX_HP;
        }
        public override void DeadSignal()
        {
            if (HP <= 0)
            {
                animator.SetBool("Moving", false);
                animator.SetBool("Damaged", true);
                animator.SetInteger("TriggerNumber", 6);
                animator.SetTrigger("Trigger");

                CameraController.Instance.ReleaseFixCamera();
                ObjectPooling.Instance.Set_Stat(gameObject.name, PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);
                GameManager.Instance.PlayerDead();
                PState = PlayerState.Die;
            }
        }
        public IEnumerator DeadAnimationEnd()
        {
            yield return new WaitForSeconds(1f);
            agent.ResetPath();
            agent.isStopped = true;
            agent.Warp(new Vector3(-9999, -9999, -9999));
        }
        void ResetCanPlayMoveSound()
        {
            canPlayMoveSound = true;
        }
        public override void ReviveSignal()
        {
            animator.SetBool("Damaged", false);
            animator.SetBool("Moving", true);
            animator.SetFloat("Velocity Z", Vector3.zero.magnitude);

            PState = PlayerState.Idle;
            agent.isStopped = false;
            agent.Warp(BuildingManager.Instance.Alter.transform.position);


            HP = GameManager.Instance.models.playerModel.PlayerMaxHp;
            GameManager.Instance.models.playerModel.PlayerNowHp = MAX_HP;
            GameManager.Instance.models.playerModel.PlayerMaxHp = MAX_HP;
        }

        private void Awake()
        {
            Init();
        }
        public override void SetModel()
        {
            base.SetModel();

            PlayerModel.PlayerNowHp = HP;
            PlayerModel.PlayerMaxHp = MAX_HP;
            PlayerModel.PlayerPD = PD;
            PlayerModel.PlayerED = ED;
            PlayerModel.PlayerMoveSpeed = MOVE_SPEED;
            PlayerModel.PlayerAttackSpeed = ATTACK_SPEED;
            PlayerModel.PlayerArmor = ARMOR;
        }
        public override void SetStat()
        {
            HP = PlayerModel.PlayerNowHp;
            MAX_HP = PlayerModel.PlayerMaxHp;
            PD = PlayerModel.PlayerPD;
            ED = PlayerModel.PlayerED;
            MOVE_SPEED = PlayerModel.PlayerMoveSpeed;
            ATTACK_SPEED = PlayerModel.PlayerAttackSpeed;
            ARMOR = PlayerModel.PlayerArmor;
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
            CoolTimer();
        }
        void CoolTimer()
        {
            if (PlayerModel.QSkillCoolTime > 0)
            {
                Debug.Log("WARRON Q");
                PlayerModel.QSkillCoolTime = Mathf.Clamp(6 - (Time.time - qTime), 0, 6);
            }
            if (PlayerModel.WSkillCoolTime > 0)
            {
                Debug.Log("WARRON W");
                PlayerModel.WSkillCoolTime = Mathf.Clamp(12 - (Time.time - wTime), 0, 12);
            }
            if (PlayerModel.ESkillCoolTime > 0)
            {
                Debug.Log("WARRON E");
                PlayerModel.ESkillCoolTime = Mathf.Clamp(14 - (Time.time - eTime), 0, 14);
            }
            if (PlayerModel.RSkillCoolTime > 0)
            {
                Debug.Log("WARRON R");
                PlayerModel.RSkillCoolTime = Mathf.Clamp(120 - (Time.time - rTime), 0, 120);
                if (R_particle.activeInHierarchy)
                {
                    if (PlayerModel.RSkillCoolTime < 96.0f && PlayerModel.RSkillCoolTime > 95.9f)
                    {
                        StartCoroutine(R_Shader_Value_Change(0.4f));
                    }
                    if (PlayerModel.RSkillCoolTime < 95.0f)
                    {
                        R_particle.SetActive(false);
                        StopCoroutine(RRR);
                    }
                }
            }
        }
        IEnumerator RR()
        {
            while (true)
            {
                Debug.Log("R Hit!");
                WaronRHitted?.Invoke();
                yield return new WaitForSeconds(1f);
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
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Ground");
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                {
                    if (hit.collider.gameObject.layer == (int)Layers.Enemy || hit.collider.gameObject.layer == (int)Layers.blackfreaks)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;

                    if (_lockTarget != null)
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
            if (_lockTarget != null) 
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
                LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Ground");
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                {
                    if (hit.collider.gameObject.layer == (int)Layers.Enemy || hit.collider.gameObject.layer == (int)Layers.blackfreaks)
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
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
            {
                _mouseHitPosition = hit.point;
                Vector3 dir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
                transform.forward = dir;
            }
        }
        void ChooseAction()
        {
            if (isAction)
                return;

            //Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (PlayerModel.QSkillCoolTime > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("Q");
                ChangRotate();
                ThrowingRock();
                PState = PlayerState.Q;
            }
            //W

            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (PlayerModel.WSkillCoolTime > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("W");
                LeafOfPride();
                PState = PlayerState.W;
            }
            //E

            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (PlayerModel.ESkillCoolTime > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("E");
                BoldRush();
                PState = PlayerState.E;
            }
            //R
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (PlayerModel.RSkillCoolTime > 0.1f)
                    return;
                useRootMotion = true; ChangRotate();
                Activation("R");
                ShockOfLand();
                PState = PlayerState.R;
                RRR = RR();
                StartCoroutine(RRR);
            }
        }


        #region Q_Skill
        Vector3 qPos;
        void ThrowingRock()
        {
            SoundPlay("Q", 0);
            ChangRotate();

            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetInteger("TriggerNumber", 12);
            animator.SetTrigger("Trigger");

            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Ground");

            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
            {
                qPos = hit.point;
            }
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
        public void ThrowingRockSpawn()
        {
            rock = Instantiate(Q_particle);
            rock.transform.position = leftArm.transform.position;
            rock.GetComponent<Waron_Q>().Init(qPos, 10, leftArm);
        }
        public void ThrowingRockThrow()
        {
            rock.GetComponent<Waron_Q>().b = false;
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
            go = Instantiate(W_particle);
            go.transform.position = transform.position;
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
            StartCoroutine(HUDManager.Instance.PlayUltEffect());
            SoundPlay("R", 3);

            agent.ResetPath();
            isAction = true;
            animator.SetBool("Moving", false);
            animator.SetBool("Attack", true);
            animator.SetInteger("Action", 1);
            animator.SetTrigger("Trigger");
            animator.SetInteger("TriggerNumber", 10);
            SetAnimatorRootMotion(true);

            StartCoroutine(BuffMoveSpeed(1.2F));
            StartCoroutine(BuffHp(1.2F));

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
            SetAnimatorRootMotion(false);
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
            if (AudioManager.Instance.Check() == true)
                _priority = 0;
            string _soundname = "";
            switch (_name)
            {
                case "장거리 이동":
                case "단거리 이동":
                    if (_priority == 0 && PlayerState.Die != PState)
                    {
                        if (canPlayMoveSound == false)
                            return;
                        _priority = 3;
                        _soundname = $"{_name} " + UnityEngine.Random.Range(1, 11);
                        canPlayMoveSound = false;

                        Invoke("ResetCanPlayMoveSound", 8f);
                        AudioManager.Instance.Read("Warron", _soundname);
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
                        AudioManager.Instance.Read("Warron", _soundname);
                    }
                    break;
                case "Q":
                case "W":
                case "E":
                case "R":
                    if ((_priority == 0 || _priority > 2) && PlayerState.Die != PState)
                    {
                        _priority = 2;
                        if (idx == 3)
                            _soundname = $"{_name}스킬";
                        else
                            _soundname = $"{_name}스킬 " + UnityEngine.Random.Range(1, 3);
                        AudioManager.Instance.Read("Warron", _soundname);
                    }
                    break;

                case "사망":
                case "시작":
                case "승리":
                case "부활":
                    if (_priority == 0 || _priority > 1)
                    {
                        _priority = 1;
                        _soundname = $"{_name} " + UnityEngine.Random.Range(1, 4);
                        AudioManager.Instance.Read("Warron", _soundname);
                    }
                    break;

            }
        }


        IEnumerator BuffMoveSpeed(float amount)
        {
            float ms = MOVE_SPEED;
            MOVE_SPEED *= amount;
            yield return new WaitForSeconds(25f);
            MOVE_SPEED = ms;
        }
        IEnumerator BuffHp(float amount)
        {
            float hp = HP;
            HP *= amount;
            yield return new WaitForSeconds(25f);
            HP = hp;
        }
    }
}