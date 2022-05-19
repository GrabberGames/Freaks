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

    private GameController gameController;

    //Reference ParticleSystem Born Position
    public GameObject _left_arm;
    public GameObject _right_arm;

    //Kyle Bullet Prefab 

    //Kyle Sound Priority Variable & Far Distance Judge
    private int _priority = 0;
    private float _dist = 0f;

    //Kyle Basic Attack Target Object
    GameObject _lockTarget;

    PlayerState _state = PlayerState.Idle;

    PlayerModel playerModel => GameManager.Instance.models.playerModel;

    float qTime = .0f;
    float wTime = .0f;
    float eTime = .0f;
    float rTime = .0f;
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
        switch (State)
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
                playerModel.qSkillCoolTime = 11;
                qTime = Time.time;
                break;
            case PlayerState.W:
                useRootMotion = true;
                ChangeRotate();
                Atonement();
                animator.Play("TwoGun Attack 05");
                playerModel.wSkillCoolTime = 15;
                wTime = Time.time;
                break;
            case PlayerState.E:
                useRootMotion = true;
                ChangeRotate();
                Evation();
                animator.Play("Jumbo Back Attack");
                playerModel.eSkillCoolTime = 16;
                eTime = Time.time;
                break;
            case PlayerState.R:
                useRootMotion = true;
                ChangeRotate();
                HorizonofMemory();
                animator.Play("Gun Air Attack");
                playerModel.rSkillCoolTime = 90;
                rTime = Time.time;
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
    void CoolTimer()
    {
        if (playerModel.qSkillCoolTime > 0)
        {
            playerModel.qSkillCoolTime = Mathf.Clamp(11 - (Time.time - qTime), 0, 11);
        }
        if (playerModel.wSkillCoolTime > 0)
        {
            playerModel.wSkillCoolTime = Mathf.Clamp(15 - (Time.time - wTime), 0, 15);
        }
        if (playerModel.eSkillCoolTime > 0)
        {
            playerModel.eSkillCoolTime = Mathf.Clamp(16 - (Time.time - eTime), 0, 16);
        }
        if (playerModel.rSkillCoolTime > 0)
        {
            playerModel.rSkillCoolTime = Mathf.Clamp(90 - (Time.time - rTime), 0, 90);
        }
    }
    #endregion
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }
    void Start()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
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
        LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            Debug.Log(hit.transform.name);
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
            ObjectPooling.Instance.Set_Stat(gameObject.name, PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);

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
        GameObject _q = ObjectPooling.Instance.GetObject("KyleQSkillEmit");
        _q.transform.position = _left_arm.transform.position;
        _q.transform.rotation = transform.rotation;
        var particleSystem = _q.GetComponent<ParticleSystem>();
        particleSystem.Play();
        StartCoroutine(Q_ParticleOff(particleSystem));

        GameObject go = ObjectPooling.Instance.GetObject("KyleBullet");
        go.transform.position = transform.position;
        go.GetComponent<Kyle_Bullet>().InitSetting(null, Bullet.Q, transform.rotation, _mouseHitPosition, 140 + PD);
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
            ObjectPooling.Instance.ReturnObject(_q.gameObject);
        else
            yield return null;
    }
    #endregion

    #region W_Skill
    void Atonement()
    {
        SoundPlay("W", 1);
        GameObject _w = ObjectPooling.Instance.GetObject("KyleWSkillEmit");
        _w.transform.position = gameObject.transform.position + Vector3.up * 2;
        _w.GetComponent<ParticleSystem>().Play();
        StartCoroutine(W_ParticleOff(_w));
        agent.ResetPath();


        List<FreaksController> _freaks = new List<FreaksController>();
        _freaks = gameController.GetAliveBlackFreaksList();
        for (int i = 0; i < _freaks.Count; i++)
        {
            if ((_freaks[i].gameObject.transform.position - transform.position).magnitude < 15f)
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
        Bullet_Spawn_NormalAndESkill(Bullet.E);
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
        LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Ground");

        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
        {
            R_Skill = Instantiate(R_Skill_Prefab);
            R_Skill.transform.position = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
            R_Skill.GetComponent<Kail_R>().Trigger();
        }
    }
    public void R_Stop()
    {
        useRootMotion = false;
        State = PlayerState.Idle;
    }
    #endregion

    void Bullet_Spawn_NormalAndESkill(Bullet shape)
    {
        GameObject go;
        if (shape.Equals(Bullet.Basic))
        {
            go = ObjectPooling.Instance.GetObject("KyleBullet");
            go.transform.position = transform.position;

            if (_lockTarget != null)
                go.transform.LookAt(_lockTarget.transform);

            GameObject _q = ObjectPooling.Instance.GetObject("KyleNormalEmit");
            _q.transform.position = transform.position;
            _q.transform.LookAt(transform.forward);
            var particleSystem = _q.GetComponent<ParticleSystem>();
            particleSystem.Play();
            StartCoroutine(Q_ParticleOff(particleSystem));

            go.GetComponent<Kyle_Bullet>().InitSetting(_lockTarget, Bullet.Basic, transform.rotation, _mouseHitPosition, PD);
        }
        if (shape.Equals(Bullet.E))
        {
            GameObject _q = ObjectPooling.Instance.GetObject("KyleESkillEmit");
            _q.transform.position = transform.position;
            _q.transform.rotation = transform.rotation;
            var particleSystem = _q.GetComponent<ParticleSystem>();
            particleSystem.Play();
            StartCoroutine(Q_ParticleOff(particleSystem));

            go = ObjectPooling.Instance.GetObject("KyleBullet");
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.identity;
            go.GetComponent<Kyle_Bullet>().InitSetting(null, Bullet.E, transform.rotation, _mouseHitPosition, 40 + 0.2f * ED);
        }
    }
    void Basic_Attack()
    {
        transform.LookAt(transform.forward);
        agent.SetDestination(transform.position);

        Bullet_Spawn_NormalAndESkill(Bullet.Basic);
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

            ObjectPooling.Instance.Set_Stat(gameObject.name, PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);
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
        CoolTimer();
    }
    public override void SetModel()
    {
        base.SetModel();

        playerModel.playerNowHp = HP;
        playerModel.playerMaxHp = MAX_HP;
        playerModel.playerPD = PD;
        playerModel.playerED = ED;
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
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Walkable") | LayerMask.GetMask("Ground");
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
            {
                if (hit.collider.gameObject.layer == (int)Layers.blackfreaks || hit.collider.gameObject.layer == (int)Layers.Enemy)
                    _lockTarget = hit.collider.gameObject;
                else
                    _lockTarget = null;
                if (_lockTarget != null) //기본 공격 할 대상이 있다.
                {
                    float distance = (_lockTarget.transform.position - transform.position).magnitude;

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
            float distance = (_lockTarget.transform.position - transform.position).magnitude;

            if (distance <= ATTACK_RANGE)
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
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building") | LayerMask.GetMask("blackfreaks") | LayerMask.GetMask("Ground");

            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
            {
                if (hit.collider.gameObject.layer == (int)Layers.blackfreaks || hit.collider.gameObject.layer == (int)Layers.Enemy)
                    _lockTarget = hit.collider.gameObject;
                else
                    _lockTarget = null;

                if (_lockTarget != null) //기본 공격 할 대상이 있다.
                {
                    float distance = (_lockTarget.transform.position - transform.position).magnitude;

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