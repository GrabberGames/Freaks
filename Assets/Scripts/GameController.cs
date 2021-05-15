using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    private RaycastHit hit;
    private string hitColliderName;

    public int money = 50;

    // Team Separation
    public List<GameObject> blueAlters = new List<GameObject>();
    public List<GameObject> redAlters = new List<GameObject>();

    // UI
    public GameObject gameOver;


    // Start is called before the first frame update
    private void Start()
    {
        //pc = GetComponent<NavMeshAgent>();
        //gameOver = GameObject.Find("GameOver");
    }

    private void Update()
    {

    }


    public void ObjectMove(NavMeshAgent agent)
    {
        if (Input.GetMouseButtonDown(0))    // Get Hero's name
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.transform.gameObject.name);
                hitColliderName = hit.transform.gameObject.name;
            }
        }

        if (Input.GetMouseButtonDown(1))    // Right Mouse Click && Hero Clicked
        {
            Debug.Log(hitColliderName);
            agent = GameObject.Find(hitColliderName).GetComponent<NavMeshAgent>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Ray Set; Mouse Pointer Position

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point); // Hero Move
            }
        }
    }

}
