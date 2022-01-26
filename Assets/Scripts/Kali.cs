using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Kali : Stat
{
    #region variables
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
    PlayerState _state= PlayerState.Idle;
    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator animator = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Attack:
                    break;
                case PlayerState.Idle:
                    animator.CrossFade("Idle", 0.1f, -1, 0);
                    break;
                case PlayerState.Q:
                    animator.Play("Gun attack3");
                    break;
                case PlayerState.W:
                    animator.Play("TwoGun Attack 05");
                    break;
                case PlayerState.E:
                    animator.Play("Jumbo Back Attack");
                    break;
                case PlayerState.R:
                    animator.Play("Gun Air Attack");
                    break;
                case PlayerState.Moving:
                    animator.CrossFade("Strafe_Run_Front", 0.1f, -1, 0);
                    break;
                case PlayerState.Die:
                    SoundPlay("사망");
                    animator.Play("Dead");
                    break;
            }

        }
    }

    private bool isAction = false;
    private bool canNormalAttack = true;
    private int E_AttackNum = 0;
    private int AttackNum = 0;
    private Vector3 dir;
    private Vector3 look_dir;

    private bool useRootMotion = false;
    private Animator animator;
    private Camera mainCamera;
    private NavMeshAgent agent;

    public KailAni kailAni;

    WaitForSeconds seconds_2s = new WaitForSeconds(2f);

    //skill cooltime variable
    float t_time = 0.0f;
    float q_time = 0.0f;
    float w_time = 0.0f;
    float e_time = 0.0f;
    float r_time = 0.0f;
    bool press = false;
    WaitForSeconds seconds_01s = new WaitForSeconds(0.1f);

    //sound variable
    public AudioSource[] audioSource;
    private bool MovingAudioSoungIsActive = false;

    private GameObject R_Skill;
    public GameObject R_Skill_Prefab;

    //Particle Skill Prefab
    public ParticleSystem _Basic_ps;
    public ParticleSystem _Q_ps;
    public GameObject _W_ps;
    public ParticleSystem _E_ps;

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


    #endregion

    void Activation(string skill)
    {
        switch (skill)
        {
            case "Q":
                q_time = 11.0f;
                break;

            case "W":
                w_time = 15.0f;
                break;

            case "E":
                e_time = 16.0f;
                break;

            case "R":
                r_time = 90.0f;
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
            }
            if (t_time < 0.1f)
            {
                t_time = 0.1f;
                press = false;
            }
            t_time -= 0.1f;
            yield return seconds_01s;
        }
    }

    void Start()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
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
    void ChooseAction()
    {
        if (isAction)
            return;

        //Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (q_time > 0.1f)
                return;
            //useRootMotion = true; 
            State = PlayerState.Q;
            ChangRotate();
            Activation("Q");
            Determination();
        }
        //W

        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (w_time > 0.1f)
                return;
            State = PlayerState.W;
            useRootMotion = true; ChangRotate();
            Atonement();
            Activation("W");
        }
        //E

        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (e_time > 0.1f)
                return;
            State = PlayerState.E;
            useRootMotion = true; ChangRotate();
            Activation("E");
            Evation();
        }
        //R
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (r_time > 0.1f)
                return;
            State = PlayerState.R;
            useRootMotion = true; ChangRotate();
            Activation("R");
            HorizonofMemory();
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
    
    public void SoundPlay(string _name, int idx = 0)
    {
        if (AudioManager.a_instance.Check() == true)
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
        AudioManager.a_instance.Read(_soundname);
    }

    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            GameManager.Instance.PlayerDead();

            ObjectPooling.instance.Set_Stat(gameObject.name, PD, ED, HP, MAX_HP, ATTACK_SPEED, MOVE_SPEED, ATTACK_RANGE, ARMOR);
            State = PlayerState.Die;
        }
    }
    IEnumerator DeadAnimationEnd()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }


    #region Q_Skill
    void Determination()
    {
        SoundPlay("Q", 0);

        agent.ResetPath();
        isAction = true;
    }
    public void Q_Bullet_Spawn()
    {
        ParticleSystem _q = Instantiate(_Q_ps);
        _q.transform.position = _left_arm.transform.position;
        _q.transform.LookAt(look_dir);
        _q.Play();
        StartCoroutine(Q_ParticleOff(_q));

        GameObject go = Instantiate(_bullet);
        go.transform.position = transform.position;
        go.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        go.GetComponent<Kyle_Bullet>().InitSetting(null, "Q", look_dir, 140+PD);
    }
    public void Q_Stop()
    {
        isAction = false;
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
        isAction = true;
    }
    public void W_Stop()
    {
        isAction = false;
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
        isAction = true;
        audioSource[2].Play();
    }
    public void E_Stop()
    {
        useRootMotion = false;
        isAction = false;
        State = PlayerState.Idle;
        E_AttackNum = 0;
    }
    public void E_Animation()
    {
        Bullet_Spawn_NE(E_AttackNum, "E");
        E_AttackNum++;
    }
    #endregion
    #region R_Skill
    void HorizonofMemory()
    {
        SoundPlay("R", 3);
        agent.ResetPath();
        isAction = true;
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
        isAction = false;
        useRootMotion = false;
        State = PlayerState.Idle;
    }
    #endregion

    void Bullet_Spawn_NE(int idx, string shape)
    {
        GameObject go;
        if (idx % 2 == 0)
        {
            go = Instantiate(_bullet);
            go.transform.position = _right_arm.transform.position;
            if(_lockTarget != null)
                go.transform.LookAt(_lockTarget.transform);
        }
        else
        {
            go = Instantiate(_bullet);
            go.transform.position = _left_arm.transform.position;
            if (_lockTarget != null)
                go.transform.LookAt(_lockTarget.transform);
        }
        if(shape == "Normal")
        {
            ParticleSystem _q = Instantiate(_Basic_ps);
            _q.transform.position = _right_arm.transform.position;
            _q.transform.LookAt(_lockTarget.transform);
            _q.Play();
            StartCoroutine(Q_ParticleOff(_q));

            go.GetComponent<Kyle_Bullet>().InitSetting(_lockTarget, "Basic", look_dir, PD);
        }
        if(shape == "E")
        {
            ParticleSystem _q = Instantiate(_E_ps);
            _q.transform.position = _left_arm.transform.position;
            if (_lockTarget != null)
                _q.transform.LookAt(_lockTarget.transform);
            else
                _q.transform.LookAt(look_dir);
            _q.Play();
            StartCoroutine(Q_ParticleOff(_q));

            go.GetComponent<Kyle_Bullet>().InitSetting(null, "E", look_dir, 40 + 0.2f *ED);
        }
    }
    void Basic_Attack()
    {
        if (!canNormalAttack || _lockTarget == null)
            return;


        transform.LookAt(_lockTarget.transform);
        agent.SetDestination(transform.position);

        Bullet_Spawn_NE(AttackNum, "Normal");
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
        canNormalAttack = false;
    }
    public void Normal_Attack_Fun()
    {
        _lockTarget = null;
        canNormalAttack = true;
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

        ChooseAction();

        switch (State)
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
        IEnumerator co = MoveSound();

        if (State == PlayerState.Moving && !MovingAudioSoungIsActive)
        {
            MovingAudioSoungIsActive = true;
            StartCoroutine(co);
        }
        if (State != PlayerState.Moving)
        {
            MovingAudioSoungIsActive = false;
            audioSource[6].Stop();
            StopCoroutine(co);
        }
    }
    IEnumerator MoveSound()
    {
        audioSource[6].Play();
        yield return seconds_2s;
        MovingAudioSoungIsActive = false;
    }
    private void UpdateDie()
    {
        return;
    }
    private void UpdateIdle()
    {
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

            if(distance <= ATTACK_RANGE)
            {
                State = PlayerState.Attack;
                return;
            }
        }
        animator.SetBool("Moving", true);

        look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        if ((dir - transform.position).magnitude < 0.1f)
            State = PlayerState.Idle;

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
                if(_dist > 120f)
                    SoundPlay("장거리 이동");
                else
                    SoundPlay("단거리 이동");
                dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                agent.SetDestination(dir);
            }
        }
    }
}
