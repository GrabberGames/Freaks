using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingController : MonoBehaviour
{
    //[SerializeField] private Button build;
    //[SerializeField] private GameObject buildList;
    [SerializeField] private GameObject[] Buildings;
    [SerializeField] private ParticleSystem fx_Smoke01; //build FX
    [SerializeField] private GameObject AlterRange;

    private BuildingPreview buildingPreview;

    private WorkshopController workshopController;

    private WhiteFreaksController[] whiteFreaksController;
    private FreaksController[] freaksControllers;

    //private bool isBtnActivate = true;
    //private bool isBtnListActivate = true;
    private bool isPreviewActivate = false;

    private float buildingHeight;

    private int buildnum;
    private bool cantbuild;
    private Vector3 hittedPoint;

    private AlterController alterController;
    private GameObject building;
    
    public Material[] materials;
    public GameObject buttens;


    private enum Building
    {
        Alter,
        Tower,
        Workshop
    };

    private void Start()
    {
        alterController = GameObject.Find("Alter").GetComponent<AlterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name == "Alter")
                {
                    buttens.SetActive(true);
                }
            }
        }

        if(isPreviewActivate)
        {
            ViewPreview();

            if (building.GetComponent<BuildingPreview>().IsBuildable() && Input.GetMouseButtonDown(0))
            {
                // FX Start
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                Physics.Raycast(ray, out hit);
                hittedPoint = hit.point;

                if (buildnum == 0)      // Alter 건설 
                {
                    Destroy(GameObject.Find("Alter"));
                    AlterRange.SetActive(false);
                }
                else if (buildnum == 2) // WorkShop 건설
                {
                    workshopController = building.GetComponentInParent<WorkshopController>();
                    workshopController.Init(building.GetComponent<BuildingPreview>().GetBelowObject());
                    building.GetComponent<BuildingPreview>().GetBelowObject().GetComponent<MeshRenderer>().enabled = false;
                }

                Build();
                Debug.Log("Builded");

                if (Physics.Raycast(ray, out hit))  // smoke FX 
                {
                    Vector3 sPos = hit.point; sPos.y = 0.3f;
                    fx_Smoke01.transform.position = sPos;
                    fx_Smoke01.Play(true);
                }
            }
        }
        
        // FX End
        if (fx_Smoke01.isStopped)
        {
            fx_Smoke01.Stop();
        }
    }


    private void ViewPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 buildPos = building.transform.position;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 _location = hit.point;
            _location.y = 0;
            building.transform.position = _location;
        }
        if (buildnum == 0)
        {
            AlterRange.SetActive(true);
        }

        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buttens.SetActive(false);
            isPreviewActivate = false;
            Destroy(GameObject.Find("Workshop_pre(Clone)"));
        }
        */
    }

    private void Build()
    {
        isPreviewActivate = false;
        buttens.SetActive(false);

        building.GetComponent<Collider>().isTrigger = false;
        building.GetComponent<BuildingPreview>().Destroy();

        alterController.GoBuild(building);
        Destroy(building.GetComponent<BuildingPreview>());

        if(buildnum == 0)
        {
            whiteFreaksController = FindObjectsOfType<WhiteFreaksController>();
            for (int i = 0; i < whiteFreaksController.Length; i++)
                whiteFreaksController[i].ChangeAlterPosition(hittedPoint);

            freaksControllers = FindObjectsOfType<FreaksController>();
            for (int i = 0; i < freaksControllers.Length; i++)
                freaksControllers[i].ChangeAlterPosition(hittedPoint);
        }
    }

    public void ConstructBuilding(int buildingNum)
    {
        Debug.Log("!!");
        buildnum = buildingNum;

        building = Instantiate(Buildings[buildingNum]);
        building.AddComponent<BuildingPreview>();
        building.GetComponent<BuildingPreview>().Init(buildingNum);
        isPreviewActivate = true;
    }
}
