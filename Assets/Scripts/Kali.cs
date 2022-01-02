using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Kali : MonoBehaviour
{
    private enum PlayerState
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
    private bool isAction = false;
    private bool canNormalAttack = true;
    private float canNormalAttackTime = 2f;
    private int AttackNum = 0;
    private Vector3 dir;
    private Vector3 look_dir;

    PlayerState _state = PlayerState.Idle;
    private bool useRootMotion = false;
    private Animator animator;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Rigidbody rigid;

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
    private Stat _stat = new Stat();

    //Particle Skill Prefab
    public ParticleSystem _Q_ps;
    public ParticleSystem _W_ps;
    public ParticleSystem _E_ps;

    //Reference ParticleSystem Born Position
    public GameObject _left_arm;
    public GameObject _right_arm;

    //Kyle Bullet Prefab 
    public GameObject _bullet;

    //Kyle Sound Priority Variable & Far Distance Judge
    private int _priority = 0;
    private float _dist = 0f;
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
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        rigid = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Start()
    {
        _stat = ObjectPooling.instance.Get_Stat("kyle");
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

        // Normal Attack
        Basic_Attack();

        //Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (q_time > 0.1f)
                return;
            //useRootMotion = true; 
            ChangRotate();
            Activation("Q");
            Determination();
            _state = PlayerState.Q;
        }
        //W

        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (w_time > 0.1f)
                return;
            useRootMotion = true; ChangRotate();
            Atonement();
            Activation("W");
            _state = PlayerState.W;
        }
        //E

        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (e_time > 0.1f)
                return;
            useRootMotion = true; ChangRotate();
            Activation("E");
            Evation();
            _state = PlayerState.E;
        }
        //R
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (r_time > 0.1f)
                return;
            useRootMotion = true; ChangRotate();
            Activation("R");
            HorizonofMemory();
            _state = PlayerState.R;
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
                       _soundname = $"{_name} " + Random.Range(1, 11);
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
                        _soundname = $"{_name}스킬 " + Random.Range(1, 3);
                    audioSource[idx].Play();
                }
                break;

            case "시작":
            case "승리":
            case "부활":
            case "스위치":
                //현재 실행중인 사운드가 없거나 우선순위 2, 3순위인게 실행중이면 하이재킹.
                if (_priority == 0 || _priority > 1)
                {
                    _priority = 1;
                    if(idx != 0)
                    {
                        if (idx == 1)
                            _soundname = "첫번째 ";
                        else if (idx == 2)
                            _soundname = "두번째";
                        else if (idx == 3)
                            _soundname = "세번째";
                        _soundname += $"{_name} " + Random.Range(1, 3);
                    }
                    else
                        _soundname = $"{_name} " + Random.Range(1, 3);
                }
                break;


        }
        //실행하기.
        AudioManager.a_instance.Read(_soundname);
    }

    #region Q_Skill
    void Determination()
    {
        SoundPlay("Q", 0);
           ParticleSystem _q = Instantiate(_Q_ps);
        _q.transform.position = _left_arm.transform.position;
        _q.Play();
        StartCoroutine(Q_ParticleOff(_q));

        GameObject go = Instantiate(_bullet);
        go.transform.position = transform.position;
        go.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        go.GetComponent<Kyle_Bullet>().InitSetting(null, "Q", look_dir);

        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 1);
    }
    public void Q_Stop()
    {
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    IEnumerator Q_ParticleOff(ParticleSystem _q)
    {
        yield return new WaitForSeconds(_q.main.startLifetimeMultiplier);
        Destroy(_q);
    }
    #endregion
    #region W_Skill
    void Atonement()
    {
        SoundPlay("W", 1);
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 2);
    }
    public void W_Stop()
    {
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    #endregion
    #region E_Skill
    void Evation()
    {
        SoundPlay("E", 2);
        agent.ResetPath();
        useRootMotion = true;
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 3);
        audioSource[2].Play();
    }
    public void E_Stop()
    {
        useRootMotion = false;
        isAction = false;
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    #endregion
    #region R_Skill
    void HorizonofMemory()
    {
        SoundPlay("R", 3);
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 4);
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
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    #endregion
    void Basic_Attack()
    {
        if (!canNormalAttack)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (AttackNum == 0)
            {
                animator.SetBool("Attack", true);
                animator.SetBool("NormalAttack", true);
                AttackNum = 1;
            }
            else
            {
                animator.SetBool("Attack", true);
                animator.SetBool("NormalAttack", false);
                AttackNum = 0;
            }
            audioSource[5].Play();
            canNormalAttack = false;
        }
    }
    public void Normal_Attack_Fun() { 
        animator.SetBool("Attack", false);
        canNormalAttack = true;
    }
    void Update()
    {
        print(_dist);
        ChooseAction();
        switch (_state)
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
        //CharacterMovement();
        if (animator.GetBool("Moving") && !MovingAudioSoungIsActive)
        {
            MovingAudioSoungIsActive = true;
            StartCoroutine(MoveSound());
        }
        if (animator.GetBool("Moving") == false)
        {
            MovingAudioSoungIsActive = false;

            StopCoroutine(MoveSound());
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

    }
    private void UpdateIdle()
    {
        animator.SetBool("Moving", false);
        if (Input.GetMouseButtonDown(1))
        {
            agent.velocity = Vector3.zero;
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building");
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
            {
                _dist = Vector3.Distance(transform.position, hit.point);
                if (_dist > 120f)
                    SoundPlay("장거리 이동");
                else
                    SoundPlay("단거리 이동");
                dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                agent.SetDestination(dir);
                animator.SetBool("Moving", true);
                _state = PlayerState.Moving;
            }
        }
    }
    private void UpdateMoving()
    {
        look_dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        if ((dir - transform.position).magnitude < 0.1f)
            _state = PlayerState.Idle;

        transform.LookAt(look_dir);

        if (Input.GetMouseButtonDown(1))
        {
            agent.velocity = Vector3.zero;
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building");
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
            {
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
