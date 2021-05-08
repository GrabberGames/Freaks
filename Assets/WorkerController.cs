using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerController : MonoBehaviour
{
    private NavMeshAgent workerAgent;
    public GameObject worker;
    private RaycastHit hit;     // hit checker
    private string hitColliderName;
    int money;
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
        ObjectMove(workerAgent);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected!");
        if(hit.transform.gameObject.name == "Workshop")
        {
            // ----- workshop enter -----
            worker.SetActive(false);    // ==> cloaking
            // Destroy(worker);         // ==> destroy
            Debug.Log("destroyed!");

            // ----- workshop leave -----
            worker.SetActive(true);     // ==> uncloak
            // Instantiate(worker,new Vector3(123,456,789), Qkuaternion rotation)
            // ==> regenerate
        }
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
            agent = GameObject.Find(hitColliderName).GetComponent<NavMeshAgent>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Ray Set; Mouse Pointer Position

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Debug.Log(hit.transform.gameObject.name);
                agent.SetDestination(hit.point); // Hero Move
            }
        }
    }
}
