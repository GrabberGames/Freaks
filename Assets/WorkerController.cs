using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class WorkerController : MonoBehaviourPunCallbacks, IPunObservable
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

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision Detected!");
        if (hit.transform.gameObject.name == "Workshop" && col.gameObject.name == "Workshop") 
        {
            // ----- workshop enter -----
            worker.SetActive(false);    // ==> cloaking
            // Destroy(worker);         // ==> destroy
            Debug.Log("Workshop hit! destroyed!");

            // ----- workshop leave -----
            worker.SetActive(true);     // ==> uncloak
            // Instantiate(worker,new Vector3(123,456,789), Qkuaternion rotation)
            // ==> regenerate
            workerAgent.destination = alter.position;
            string bject = col.gameObject.name;
            if (col.gameObject.name == "Alter")
            {
                Debug.Log("Alter hit!");
                workerAgent.destination = workshop.position;
            }

            if (col.gameObject.name == "Workshop")
            {
                Debug.Log("Workshop hit!");
                workerAgent.destination = alter.position;
            }
        }
    }
  
    // (-309, 5.5, 115)

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

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(money);
        }
        else
        {
            this.money = (int)stream.ReceiveNext();
        }
    }
    #endregion
}
