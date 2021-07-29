using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum Team
{
    Blue,
    Red
}
public class FreaksController : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameController gameController;

    private GameObject alter;
    private GameObject enemy;

    private Vector3 alterPosition;

    public bool isEnemyFound;
    public Team myTeam;
     

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GameObject.Find("Waron");
        alter = GameObject.Find("Alter");
        alterPosition = alter.transform.position;
    }


    // Update is called once per frame
    void Update()
    {
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
}
