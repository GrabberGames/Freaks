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
    
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        workerAgent = GetComponent<NavMeshAgent>();
    }
    
    // Update is called once per frame
    void Update()
    {
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


}
