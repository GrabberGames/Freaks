using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Kali : Stat
{
    #region variables
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
    private int E_AttackNum = 0;
    private int AttackNum = 0;
    private Vector3 dir;
    private Vector3 look_dir;
    private Vector3 _mouseHitPosition;

    private bool useRootMotion = false;
    private Animator animator;
    private Camera mainCamera;
    private NavMeshAgent agent;

    public KailAni kailAni;

    //sound variable
    public AudioSource[] audioSource;

    private GameObject R_Skill;
    public GameObject R_Skill_Prefab;

    //Particle Skill Prefab
    public ParticleSystem _Basic_ps;
    public ParticleSystem _Q_ps;
    public GameObject _W_ps;
    public ParticleSystem _E_ps;

    private GameController gameController;

    //Skill Range
    float _wSkillAttackRange = 15;

    //Reference ParticleSystem Born Position
    public GameObject _left_arm;
    public GameObject _right_arm;

    //Kyle Bullet Prefab 
    public GameObject _bullet;

    //Kyle Sound Priority Variable & Far Distance Judge
    private int _priority = 0;
    private float _dist = 0f;

    //Kyle Basic Attack Target Object
    GameObject _lockTarget;

    PlayerState _state = PlayerState.Idle;

    PlayerModel playerModel => GameManager.Instance.models.playerModel;


    public PlayerState State
    {
        get { return _state; }
        set
        {
            if (CheckIsValidToChangePresentPlayerState(_state, value))
                return;

            _state = value;
            SetAnimationAndStartAction();
        }
    }
    private void SetAnimationAndStartAction()
    {
        animator = GetComponent<Animator>();
        switch(State)
        {
            case PlayerState.Idle:
                animator.CrossFade("Idle", 0.1f, -1, 0);
                break;

            case PlayerState.Attack:
                Basic_Attack();
                break;

            case PlayerState.Q:
                useRootMotion = true;
                ChangeRotate();
                Determination();
                animator.Play("Gun attack3");
                StartCoroutine(CoCoolTimer(PlayerState.Q));
                break;
            case PlayerState.W:
                useRootMotion = true;
                ChangeRotate();
                Atonement();
                animator.Play("TwoGun Attack 05");
                StartCoroutine(CoCoolTimer(PlayerState.W));
                break;
            case PlayerState.E:
                useRootMotion = true;
                ChangeRotate();
                Evation();
                animator.Play("Jumbo Back Attack");
                StartCoroutine(CoCoolTimer(PlayerState.E));
                break;
            case PlayerState.R:
                useRootMotion = true;
                ChangeRotate();
                HorizonofMemory();
                animator.Play("Gun Air Attack");
                StartCoroutine(CoCoolTimer(PlayerState.R));
                break;

            case PlayerState.Moving:
                audioSource[6].Play();
                animator.CrossFade("Strafe_Run_Front", 0.1f, -1, 0);
                break;

            case PlayerState.Die:
                SoundPlay("사망");
                animator.Play("Dead");
                break;
        }
    }
    private bool CheckIsValidToChangePresentPlayerState(PlayerState nowState, PlayerState nextState)
    {
        switch (nextState)
        {
            case PlayerState.Attack:
                break;

            case PlayerState.Q:
                if (playerModel.qSkillCoolTime > 0.1f)
                    return true;
                if (!(nowState == PlayerState.Moving || nowState == PlayerState.Idle))
                    return true;
                break;
            case PlayerState.W:
                if (playerModel.wSkillCoolTime > 0.1f)
                    return true;
                if (!(nowState == PlayerState.Moving || nowState == PlayerState.Idle))
                    return true;
                break;
            case PlayerState.E:
                if (playerModel.eSkillCoolTime > 0.1f)
                    return true;
                if (!(nowState == PlayerState.Moving || nowState == PlayerState.Idle))
                    return true;
                break;
            case PlayerState.R:
                if (playerModel.rSkillCoolTime > 0.1f)
                    return true;
                if (!(nowState == PlayerState.Moving || nowState == PlayerState.Idle))
                    return true;
                break;

            case PlayerState.Moving:
                break;
        }

        return false;
    }
    IEnumerator CoCoolTimer(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Q:
                playerModel.qSkillCoolTime = 11;
                var qStart = Time.time;

                while (playerModel.qSkillCoolTime > 0)
                {
                    playerModel.qSkillCoolTime = 11 - (Time.time - qStart);
                    yield return null;
                }
                playerModel.qSkillCoolTime = 0;
                break;

            case PlayerState.W:
                playerModel.wSkillCoolTime = 15;
                var wStart = Time.time;

                while (playerModel.wSkillCoolTime > 0.0f)
                {
                    playerModel.wSkillCoolTime = 15 - (Time.time - wStart);
                    yield return null;
                }
                playerModel.wSkillCoolTime = 0;
                break;
            case PlayerState.E:
                playerModel.eSkillCoolTime = 16;
                var eStart = Time.time;
                while (playerModel.eSkillCoolTime > 0.0f)
                {
                    playerModel.eSkillCoolTime = 16- (Time.time - eStart);
                    yield return null;
                }
                playerModel.eSkillCoolTime = 0;
                break;
            case PlayerState.R :
                playerModel.rSkillCoolTime = 90;
                var rStart = Time.time;
                while (playerModel.rSkillCoolTime > 0.0f)
                {
                    playerModel.rSkillCoolTime = 90 - (Time.time - rStart);
                    yield return null;
                }
                playerModel.rSkillCoolTime = 0;
                break;
        }
    }
    #endregion
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();

        _wSkillAttackRange = Mathf.Sqrt(_wSkillAttackRange);
    }
    void Start()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
        ATTACK_RANGE *= ATTACK_RANGE;
        GameManager.Instance.models.playerModel.playerNowHp = HP;
        GameManager.Instance.models.playerModel.playerMaxHp = MAX_HP; 
        GameManager.Instance.models.playerModel.playerPD = PD;
        GameManager.Instance.models.playerModel.playerED = ED;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void OnAnimatorMove()
    {
        if (useRootMotion)
        {
            transform.rotation = animator.rootRotation;
            transform.position += animator.deltaPosition;
            //dir = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        }
    }
    void ChangeRotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) 
        {
            _mouseHitPosition = hit.point;
            Vector3 dir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
            transform.forward = dir;
        }

    }
    public void SoundPlay(string _name, int idx = 0)
    {
        if (AudioManager.a_Instance.Check() == true)
            _priority = 0;
        string _soundname = "";
        switch(_name)
        {
            case "장거리 이동":
            case "단거리 이동":
                //현재 실행중인 사운드가 없으면 실행
                if (_priority == 0)
                {
                    _priority = 3;
                    {
                        _soundname = $"{_name} " + UnityEngine.Random.Range(1, 11);
                    }
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
                    audioSource[idx].Play();
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
        AudioManager.a_Instance.Read(_soundname);
    }

    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            ObjectPooling.instance.Set_Stat(gameObject.name, PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);

            GameManager.Instance.PlayerDead();
            State = PlayerState.Die;
        }
    }
    public IEnumerator DeadAnimationEnd()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    #region Q_Skill
    void Determination()
    {
        SoundPlay("Q", 0);

        agent.ResetPath();
    }
    public void Q_Bullet_Spawn()
    {
        ParticleSystem _q = Instantiate(_Q_ps);
        _q.transform.position = _left_arm.transform.position;
        _q.transform.rotation = transform.rotation;
        _q.Play();
        StartCoroutine(Q_ParticleOff(_q));

        GameObject go = Instantiate(_bullet);
        go.transform.position = transform.position;
        go.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        go.GetComponent<Kyle_Bullet>().InitSetting(null, "Q", transform.rotation, _mouseHitPosition, 140 +PD);
    }
    public void Q_Stop()
    {
        useRootMotion = false;
        State = PlayerState.Idle;
    }
    IEnumerator Q_ParticleOff(ParticleSystem _q)
    {
        yield return new WaitForSeconds(_q.main.startLifetimeMultiplier);
        if (_q != null)
            Destroy(_q.gameObject);
        else
            yield return null;
    }
    #endregion

    #region W_Skill
    void Atonement()
    {
        SoundPlay("W", 1);
        GameObject _w = Instantiate(_W_ps);
        _w.transform.position = gameObject.transform.position + Vector3.up * 2;
        _w.GetComponent<ParticleSystem>().Play();
        StartCoroutine(W_ParticleOff(_w));
        agent.ResetPath();


        List <FreaksController> _freaks = new List <FreaksController>();
        _freaks = gameController.GetAliveBlackFreaksList();
        for (int i = 0; i < _freaks.Count; i++)
        {
            if (GetInAttackRange(transform.position, _freaks[i].gameObject.transform.position, _wSkillAttackRange))
            {
                GameManager.Damage.OnAttacked(150 + 0.5f * PD, _freaks[i].GetComponent<Stat>());
            }
        }
    }
    public void W_Stop()
    {
        useRootMotion = false;
        State = PlayerState.Idle;
    }
    IEnumerator W_ParticleOff(GameObject _w)
    {
        yield return new WaitForSeconds(_w.GetComponent<ParticleSystem>().main.startLifetimeMultiplier * 1.4f);
        if (_w != null)
            Destroy(_w);
        else
            yield return null;

    }
    #endregion
    #region E_Skill
    void Evation()
    {
        SoundPlay("E", 2);
        agent.ResetPath();
        useRootMotion = true;
        audioSource[2].Play();
        StartCoroutine(BuffDecreaseDamage(0.5f));
    }
    public void E_Stop()
    {
        useRootMotion = false;
        State = PlayerState.Idle;
        E_AttackNum = 0;
    }
    public void E_Animation()
    {
        Bullet_Spawn_NormalAndESkill(E_AttackNum, "E");
        E_AttackNum++;
    }
    #endregion

    #region R_Skill
    void HorizonofMemory()
    {
        SoundPlay("R", 3);
        agent.ResetPath();
    }
    public void R_Sound()
    {
        audioSource[4].Play();
    }
    public void R_Instantiate()
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building");

        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
        {
            R_Skill = Instantiate(R_Skill_Prefab);
            R_Skill.transform.position = new Vector3(hit.point.x, hit.point.y +0.2f, hit.point.z);
            R_Skill.GetComponent<Kail_R>().Trigger();
        }
    }
    public void R_Stop()
    {
        useRootMotion = false;
        State = PlayerState.Idle;
    }
    #endregion

    void Bullet_Spawn_NormalAndESkill(int idx, string shape)
    {
        GameObject go;
        if (idx % 2 == 0)
        {
            go = Instantiate(_bullet);
            go.transform.position = transform.position;
            if(_lockTarget != null)
                go.transform.LookAt(_lockTarget.transform);
        }
        else
        {
            go = Instantiate(_bullet);
            go.transform.position = transform.position;
            if (_lockTarget != null)
                go.transform.LookAt(_lockTarget.transform);
        }
        if(shape == "Normal")
        {
            ParticleSystem _q = Instantiate(_Basic_ps);
            _q.transform.position = transform.position;
            _q.transform.LookAt(transform.forward);
            _q.Play();
            StartCoroutine(Q_ParticleOff(_q));

            go.GetComponent<Kyle_Bullet>().InitSetting(_lockTarget, "Basic", transform.rotation, _mouseHitPosition,  PD);
        }
        if(shape == "E")
        {
            ParticleSystem _q = Instantiate(_E_ps);
            _q.transform.position = transform.position;
            _q.transform.rotation = transform.rotation;
            _q.Play();
            StartCoroutine(Q_ParticleOff(_q));

            go.GetComponent<Kyle_Bullet>().InitSetting(null, "E", transform.rotation, _mouseHitPosition, 40 + 0.2f *ED);
        }
    }
    void Basic_Attack()
    {
        transform.LookAt(_lockTarget.transform);
        agent.SetDestination(transform.position);

        Bullet_Spawn_NormalAndESkill(AttackNum, "Normal");
        animator.SetBool("Moving", false);

        if (AttackNum == 0)
        {
            animator.CrossFade("Normal Attack 1", 0.1f);
            AttackNum = 1;
        }
        else
        {
            animator.CrossFade("Normal Attack 2", 0.1f);
            AttackNum = 0;
        }
        audioSource[5].Play();
    }
    public void Normal_Attack_Fun()
    {
        _lockTarget = null;
        State = PlayerState.Idle;
    }
    void Update()
    {
        if (State == PlayerState.Die)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.PlayerDead();

            ObjectPooling.instance.Set_Stat(gameObject.name, PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);
            State = PlayerState.Die;
        }

         switch (State)
        {
            case PlayerState.Attack:
                break;
            case PlayerState.Q:
            case PlayerState.W:
            case PlayerState.E:
            case PlayerState.R:
                break;
            case PlayerState.Moving:
                UpdateMoving(); MoveToSkillState();
                break;

            case PlayerState.Die:
                UpdateDie();
                break;

            case PlayerState.Idle:
                UpdateIdle(); MoveToSkillState();
                break;
        }
    }
    private void MoveToSkillState()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            State = PlayerState.Q;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            State = PlayerState.W;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            State = PlayerState.E;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            State = PlayerState.R;
        }
    }
    private void UpdateDie()
    {
        return;
    }
    private void UpdateIdle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            agent.velocity = Vector3.zero;
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks");
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
            {
                if (hit.collider.gameObject.layer == (int)Layers.blackfreaks || hit.collider.gameObject.layer == (int)Layers.Enemy)
                    _lockTarget = hit.collider.gameObject;
                else
                    _lockTarget = null;
                if (_lockTarget != null) //기본 공격 할 대상이 있다.
                {
                    float distance = (_lockTarget.transform.position - transform.position).sqrMagnitude;

                    if (distance <= ATTACK_RANGE)
                    {
                        State = PlayerState.Attack;
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
                State = PlayerState.Moving;
            }
        }
    }
    private void UpdateMoving()
    {
        if (_lockTarget != null) //기본 공격 할 대상이 있다.
        {
            float distance = (_lockTarget.transform.position - transform.position).sqrMagnitude;

            if(distance <= ATTACK_RANGE)
            {
                State = PlayerState.Attack;
                return;
            }
        }
        animator.SetBool("Moving", true);

        look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        if ((dir - transform.position).sqrMagnitude < 0.5f)
            State = PlayerState.Idle;

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
                if (hit.collider.gameObject.layer == (int)Layers.blackfreaks || hit.collider.gameObject.layer == (int)Layers.Enemy)
                    _lockTarget = hit.collider.gameObject;
                else
                    _lockTarget = null;

                if (_lockTarget != null) //기본 공격 할 대상이 있다.
                {
                    float distance = (_lockTarget.transform.position - transform.position).sqrMagnitude;

                    if (distance <= ATTACK_RANGE)
                    {
                        State = PlayerState.Attack;
                        return;
                    }
                }

                _dist = Vector3.Distance(transform.position, hit.point);
                if(_dist > 120f)
                    SoundPlay("장거리 이동");
                else
                    SoundPlay("단거리 이동");
                dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                agent.SetDestination(dir);
            }
        }
    }

    IEnumerator BuffDecreaseDamage(float amount)
    {
        DECREASE_DAMAGE = amount;

        yield return new WaitForSeconds(2);

        DECREASE_DAMAGE = 0;
    }
}
