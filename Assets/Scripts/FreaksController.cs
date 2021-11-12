using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class FreaksController : MonoBehaviour
{
    public float startingHealth = 100;
    public float currentHealth;
    public float enemyHealth;
    public float MoveSpeed;
    public float timeBetweenAttacks = 0.5f;
    public float attackDamage = 10;

    private GameObject alter;
    private GameObject white_freaks;
    private GameObject black_freaks;
    private GameObject kail;
    private GameObject waron;
    private GameObject near;
    private NavMeshAgent agent;

    private Vector3 alterPosition;
    bool playerInRange;
    float timer;
    Stat _stat = new Stat();
    private bool isStuern = false;
    bool damaged;
    bool isDead;
    public bool isEnemyFound;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _stat = ObjectPooling.instance.Get_Stat("whitefreaks");
        currentHealth = startingHealth;
        enemyHealth = _stat.hp;
        MoveSpeed = agent.speed;
    }

    void Awake()
    {
        waron = GameObject.Find("Waron");
        alter = GameObject.Find("Alter");
        kail = GameObject.Find("Kail");
        white_freaks = GameObject.Find("White Freaks");
        alterPosition = alter.transform.position;
    }

    void Update()
    {
        if (isStuern)
        {
            agent.isStopped = true;
            return;
        }
        else
            agent.isStopped = false;

        //공격 -> 지금은 white_freaks health밖에 없어서 나중에 수정 필요
        // 공격 중 다른 오브젝트에게 공격 -> 타겟팅 변경 O
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth > 0)
        {
            Attack(near);
        }

        //이동
        float dist_alter = Vector3.Distance(alter.transform.position, gameObject.transform.position); // Distance between of alter and black_freaks
        float dist_waron = Vector3.Distance(waron.transform.position, gameObject.transform.position); // Distance between of waron and black_freaks
        float dist_kail = Vector3.Distance(kail.transform.position, gameObject.transform.position); // Distance between of kail and black_freaks
        float dist_white = Vector3.Distance(white_freaks.transform.position, gameObject.transform.position); // Distance between of white_freaks and black_freaks

        //가장 먼저 시야에 들어온 플레이어 오브젝트에게 접근
        if (dist_alter <= 7.5f)
        {
            isEnemyFound = true;
            agent.SetDestination(alter.transform.position);
        }
        else if (dist_waron <= 7.5f)
        {
            isEnemyFound = true;
            agent.SetDestination(waron.transform.position);
        }
        else if (dist_kail <= 7.5f)
        {
            isEnemyFound = true;
            agent.SetDestination(kail.transform.position);
        }
        else if (dist_white <= 7.5f)
        {
            isEnemyFound = true;
            agent.SetDestination(white_freaks.transform.position);
        }
        else
        {
            isEnemyFound = false;
        }

        //적이 근처에 없다면 알터를 향해 진군
        if (agent.enabled && !isEnemyFound)
        {
            agent.SetDestination(alterPosition);
        }
    }

    public void Damaged(float amount)// 영웅으로부터 공격을 받았을 때의 상황
    {
        damaged = true;
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        transform.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    void TakeDamage(GameObject near, float amout)
    {

    }

    void Attack(GameObject near)
    {
        timer = 0f;
        //health가 아직 안되있기에 변경 필요 + 공격받았을 때의 상황이 모두 함수가 안되어있음
        if (currentHealth > 0)
        {
            TakeDamage(near, attackDamage);
        }
    }

    public void ChangeAlterPosition(Vector3 alterPosition)
    {
        this.alterPosition = alterPosition;
    }

    public IEnumerator StartDamage(float damage, Vector3 playerPosition, float delay, float pushBack)//영웅에게 공격받았을 때 영웅에게 튕겨져 나가는 효과를 주도록 움직이는 힘을 가함
    {
        yield return new WaitForSeconds(delay);

        try
        {
            Damaged(damage);

            Vector3 diff = playerPosition - transform.position;
            diff = diff / diff.sqrMagnitude;
            GetComponent<Rigidbody>().AddForce((transform.position - new Vector3(diff.x, diff.y, 0f)) * 50f * pushBack);
        }
        catch (MissingReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == waron || other.gameObject == kail || other.gameObject == white_freaks || other.gameObject == alter)
        {
            playerInRange = true;
            near = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == waron || other.gameObject == kail || other.gameObject == white_freaks || other.gameObject == alter)
        {
            playerInRange = false;
            near = null;
        }
    }
    public IEnumerator MoveSpeedSlow(float value)
    {
        print("movespeed");
        agent.speed = MoveSpeed * value;
        yield return new WaitForSeconds(1.5f);
        agent.speed = MoveSpeed;
    }

    public IEnumerator Stuern(float value)
    {
        print("stuern");
        isStuern = true;
        yield return new WaitForSeconds(value);
        isStuern = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 7.5f);
    }
}