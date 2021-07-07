using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerController : MonoBehaviour 
{
    private NavMeshAgent workerAgent;
    public GameObject worker;
    public Transform alter;
    public Transform workshop;
    private RaycastHit hit;     // hit checker
    private string hitColliderName;
    
    [SerializeField]
    private int money = 500;
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
    
        workerAgent = GetComponent<NavMeshAgent>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    money -= 100;
        //}

        ObjectMove(workerAgent);
    }

    IEnumerator SetRendererActiveTrue()
    {
        // ----- delay 1 sec -----
        yield return new WaitForSeconds(1f);

        // ----- workshop leave -----
        worker.GetComponent<Renderer>().enabled = true;
        workerAgent.destination = alter.position;
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision Detected!");
        if (hit.transform.gameObject.name == "Workshop" && col.gameObject.name == "Workshop")
        {
            // ----- workshop enter -----
            worker.GetComponent<Renderer>().enabled = false;
            Debug.Log("Workshop hit! destroyed!");

            StartCoroutine("SetRendererActiveTrue");

        }

        // ----- workshop regression -----
        if (hit.transform.gameObject.name == "Workshop" && col.gameObject.name == "Alter")
        {
            workerAgent.destination = workshop.position;
        }
    }
  
    // (-309, 5.5, 115)

    public void ObjectMove(NavMeshAgent agent)
    {
        //if (Input.GetMouseButtonDown(0))    // Get Hero's name
        //{
        //    print("!!!");
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //    {
        //        Debug.Log(hit.transform.gameObject.name);
        //        hitColliderName = hit.transform.gameObject.name;
        //    }
        //}

        //if (Input.GetMouseButtonDown(0))    // Right Mouse Click && Hero Clicked
        //{
        //    agent = GameObject.Find(hitColliderName).GetComponent<NavMeshAgent>();
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Ray Set; Mouse Pointer Position

        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //    {
        //        // Debug.Log(hit.transform.gameObject.name);
        //        agent.SetDestination(hit.point); // Hero Move
        //    }
        //}
    }

}
