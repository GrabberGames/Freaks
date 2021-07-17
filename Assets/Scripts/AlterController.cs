using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterController : MonoBehaviour
{
    [SerializeField]
    private GameObject workerFreaksPref;

    public int essence = 1000;
    [SerializeField]
    private int workerFreaks = 5;
    private int busyWorkerF = 0;

    private List<GameObject> constructingBuilding = new List<GameObject>();
    private List<GameObject> miningFreaks= new List<GameObject>();

    private bool isAlterClicked = false;

    // Update is called once per frame
    void Update()
    {
        newMiningWorkshopChk();
    }

    public bool CanBuild()
    {
        return workerFreaks - busyWorkerF > 0;
    }

    public void GoBuild(GameObject building)
    {
        //busyWorkerF++;
        //GameObject worker = Instantiate(workerFreaksPref);
        //miningFreaks.Add(worker);
        //worker.GetComponent<WhiteFreaksController>().SetMiningWorkShop(building);
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
                    GameObject worker = Instantiate(workerFreaksPref, transform.position, transform.rotation);
                    miningFreaks.Add(worker);
                    worker.GetComponent<WhiteFreaksController>().miningWorkshop = hitGameObject;
                    hitGameObject.GetComponent<WorkshopController>().SetMiningWorker(worker);
                }
            }
        }
    }
}
