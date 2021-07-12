using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterController : MonoBehaviour
{
    [SerializeField]
    private GameObject workerFreeksPref;

    public int essence = 1000;
    [SerializeField]
    private int workerFreeks = 5;
    private int busyWorkerF = 0;

    private List<GameObject> constructingBuilding = new List<GameObject>();
    private List<GameObject> miningFreeks= new List<GameObject>();

    private bool isAlterClicked = false;

    // Update is called once per frame
    void Update()
    {
        newMiningWorkshopChk();
    }

    public bool CanBuild()
    {
        return workerFreeks - busyWorkerF > 0;
    }

    public void GoBuild(GameObject building)
    {
        //busyWorkerF++;
        //GameObject worker = Instantiate(workerFreeksPref);
        //miningFreeks.Add(worker);
        //worker.GetComponent<WorkerController>().SetMiningWorkShop(building);
    }

    private void newMiningWorkshopChk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitGameObject = hit.transform.gameObject;

                if (hitGameObject == gameObject)
                {
                    isAlterClicked = true;
                }
                else
                {
                    isAlterClicked = false;
                }

            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitGameObject = hit.transform.gameObject;

                if (isAlterClicked && hitGameObject.GetComponent<WorkshopController>() != null)
                {
                    busyWorkerF++;
                    GameObject worker = Instantiate(workerFreeksPref, transform.position, transform.rotation);
                    miningFreeks.Add(worker);
                    worker.GetComponent<WorkerController>().miningWorkshop = hitGameObject;
                    hitGameObject.GetComponent<WorkshopController>().SetMiningWorker(worker);
                }
            }
        }
    }
}
