using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FreaksController : MonoBehaviour
{
    public enum Team
    {
        Blue,
        Red
    }

    private NavMeshAgent agent;
    private GameController gameController;

    private GameObject alter;
    private GameObject enemy;

    private Vector3 alterPosition;

    public bool isEnemyFound;
    public Team myTeam;

    public float attack;    // Variable to be used when adding freaks later
    private float freaksMoveSpeed;
    private float hp;
    private bool isStuern = false;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GameObject.Find("Waron");
        alter = GameObject.Find("Alter");
        alterPosition = alter.transform.position;
        freaksMoveSpeed = agent.speed;
    }


    // Update is called once per frame
    void Update()
    {
        if (isStuern)
        {
            agent.isStopped = true;
            return;
        }
        else
            agent.isStopped = false;
        float dist = Vector3.Distance(enemy.transform.position, gameObject.transform.position); // Distance between of enemy and Freaks

        if (dist <= 7.5f)
        {
            isEnemyFound = true;
            agent.SetDestination(enemy.transform.position);
        }
        else
        {
            isEnemyFound = false;
            agent.SetDestination(alterPosition);
        }
    }


    public void ChangeAlterPosition(Vector3 alterPosition)
    {
        this.alterPosition = alterPosition;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 7.5f);
    }


    public IEnumerator MoveSpeedSlow(float value)
    {
        print("movespeed");
        agent.speed = freaksMoveSpeed * value;
        yield return new WaitForSeconds(1.5f);
        agent.speed = freaksMoveSpeed;
    }
    
    
    public void Damaged(float value)
    {
        print("damaged");
        hp -= value;
    }


    public IEnumerator Stuern(float value)
    {
        print("stuern");
        isStuern = true;
        yield return new WaitForSeconds(value);
        isStuern = false;
    }
}
