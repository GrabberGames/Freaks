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
    public GameObject near;
    private NavMeshAgent agent;

    private Vector3 alterPosition;
    bool playerInRange;
    float timer;
    Stat white_stat = new Stat();
    Stat black_stat = new Stat();
    Stat waron_stat = new Stat();
    Stat kail_stat = new Stat();
    FreaksAttack freaksAttack;
    private bool isStuern = false;
    bool damaged;
    bool isDead;
    bool isEnemyFound = false;
    // Start is called before the first frame update
    void Start()
    {
        freaksAttack = gameObject.GetComponentInChildren<FreaksAttack>();

        agent = GetComponent<NavMeshAgent>();
        white_stat = ObjectPooling.instance.Get_Stat("whitefreaks");
        black_stat = ObjectPooling.instance.Get_Stat("blackfreaks");
        waron_stat = ObjectPooling.instance.Get_Stat("waron");
        kail_stat = ObjectPooling.instance.Get_Stat("kail");

        currentHealth = black_stat.hp;
        enemyHealth = white_stat.hp;
        MoveSpeed = agent.speed;

        alterPosition = alter.transform.position;
        agent.SetDestination(alterPosition);
    }

    void Awake()
    {
        waron = GameObject.Find("Waron");
        alter = GameObject.Find("Alter");
        kail = GameObject.Find("Kail");
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


        // 공격 중 다른 오브젝트에게 공격 -> 타겟팅 변경 O
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange)
        {
            Attack(near);
        }
        isEnemyFound = freaksAttack.isEnemyFound;
        if (isEnemyFound){
            agent.Move(near.transform.position);
        }

    }

    //이동
    public void move(GameObject near)
    {
        agent.SetDestination(near.transform.position);
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

    void TakeDamage(GameObject near, float amount)//공격
    {
        if (near == kail)
        {
            kail_stat.hp -= amount;
        }
        else if (near == waron)
        {
            waron_stat.hp -= amount;
        }
        else if (near == alter)
        {

        }
        else
        {
            white_stat.hp -= amount;
        }
    }

    // 공격
    void Attack(GameObject near)
    {
        timer = 0f;

        if (currentHealth > 0)
        {
            TakeDamage(near, attackDamage);
        }
    }

    public void ChangeAlterPosition(Vector3 alterPosition)
    {
        this.alterPosition = alterPosition;
    }

    //영웅에게 공격받았을 때 영웅에게 튕겨져 나가는 효과를 주도록 움직이는 힘을 가함
    public IEnumerator StartDamage(float damage, Vector3 playerPosition, float delay, float pushBack)
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
    void OnTriggerEnter(Collider other)//두 게임오브젝트의 충돌발생 경우
    {
        if (other.transform.name == "Alter" || other.transform.name=="Waron"|| other.transform.name == "Kail")
        {
            playerInRange = true;
            near = other.gameObject;
            agent.SetDestination(near.transform.position);
        }
    }

    void OnTriggerExit(Collider other)//충돌이 끝난 경우
    {
        if (other.transform.name == "Alter" || other.transform.name == "Waron" || other.transform.name == "Kail")
        {
            playerInRange = false;
            near = null;
            agent.SetDestination(alterPosition);
        }
    }

    public IEnumerator MoveSpeedSlow(float value)
    {
        agent.speed = MoveSpeed * value;
        yield return new WaitForSeconds(1.5f);
        agent.speed = MoveSpeed;
    }

    public IEnumerator Stuern(float value)
    {
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