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
    private GameObject destination;
    private GameObject enemy;


    public bool isEnemyFound;
    public Team myTeam;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        agent = GetComponent<NavMeshAgent>();
        enemy = GameObject.Find("Brute Warrior");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemyFound)
        {
            Debug.Log("N", gameObject);
            agent.SetDestination(SearchCloseEnemyAlter().transform.position);
        }
        
        if (isEnemyFound)
        {
            Debug.Log("Y", gameObject);
            agent.SetDestination(enemy.transform.position);
        }
    }


    // Find nearest enemy Alter
    GameObject SearchCloseEnemyAlter()
    {
        List<GameObject> enemyAlters;
        if (myTeam == Team.Blue)
        {
            enemyAlters = gameController.redAlters;
        }
        else
        {
            enemyAlters = gameController.blueAlters;
        }

        /*
        if (enemyAlters.Count == 0)
        {
            return null;
        }
        */

        GameObject closeAlter = enemyAlters[0];
        NavMeshPath path = agent.path;

        agent.CalculatePath(closeAlter.transform.position, path);
        
        float min = GetPathLength(path);

        foreach (GameObject i in enemyAlters)
        {
            path = agent.path;
            agent.CalculatePath(closeAlter.transform.position, path);
            float tmp = GetPathLength(path);

            if (min > tmp)
            {
                min = tmp;
                closeAlter = i;
            }

        }
        return closeAlter;
    }

    public static float GetPathLength(NavMeshPath path)
    {
        if (path.corners.Length == 0)
        {
            return 0;
        }

        Vector3 previousCorner = path.corners[0];
        float length = 0.0F;
        int i = 1;

        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            length += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }

        return length;
    }

    
    void OnTriggerEnter(Collider other)
    {
        // Find nearest Enemy Freaks //
        if (other.gameObject.CompareTag("Player"))
        {
            isEnemyFound = true;
            //Debug.Log("Found: " + other.gameObject.name);     // Debug()
        }
        //--//
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isEnemyFound = false;
            //Debug.Log("Lost: " + other.gameObject.name);     // Debug()
        }
    }
    
}
