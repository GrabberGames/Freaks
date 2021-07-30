using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlterController : Building
{
    public GameObject whiteFreaksPref;

    public int essence = 1000;
    [SerializeField] 
    private int whiteFreaks = 5;
    private int busyWhiteF = 0;

    private List<GameObject> miningFreaks = new List<GameObject>();

    private bool isAlterClicked = false;

    // Update is called once per frame
    void Update()
    {
        newMiningWorkshopChk();
    }

    public bool CanBuild()
    {
        return whiteFreaks - busyWhiteF > 0;
    }

    public void GoBuild(GameObject building)
    {
        busyWhiteF++;
        GameObject whiteFreaks = Instantiate(whiteFreaksPref, transform.position, transform.rotation);
        miningFreaks.Add(whiteFreaks);
        whiteFreaks.GetComponent<WhiteFreaksController>().miningWorkshop = building;

        if(building.GetComponent<WorkshopController>())
        {
            whiteFreaks.GetComponent<WhiteFreaksController>().SetMiningWorkShop();
            building.GetComponent<WorkshopController>().SetMiningFreeks(whiteFreaks);
        }
        
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
                    GoBuild(hitGameObject);
                }
            }
        }
    }

    public override void SetMaterial(bool isRed)
    {
        return;
    }

    public override void SetOpacity(bool isTransparent)
    {
        return;
    }
}
