using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private Button build;
    [SerializeField] private GameObject buildList;
    [SerializeField] private GameObject[] Buildings;
    [SerializeField] private ParticleSystem fx_Smoke01; //build FX

    private BuildingPreview buildingPreview;

    private bool isBtnActivate = true;
    private bool isBtnListActivate = true;
    private bool isPreviewActivate = false;

    private float buildingHeight;

    private AlterController alterController;
    private GameObject building;
    
    public Material[] materials;

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
                    SetBuildBtnActivate();
                }
            }
        }

        if(isPreviewActivate)
        {
            viewPreview();
            if (building.GetComponent<BuildingPreview>().IsBuildable() && Input.GetMouseButtonDown(0))
            {
                Build();
                Debug.Log("Builded");

                // FX Start
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 sPos = hit.point; sPos.y = 0.3f;
                    fx_Smoke01.transform.position = sPos;
                    fx_Smoke01.Play(true);
                    Debug.Log("Played");
                }
            }
        }

        // FX End
        if (fx_Smoke01.isStopped)
        {
            fx_Smoke01.Stop();
        }
    }


    private void viewPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 _location = hit.point;
            _location.y = 0;
            building.transform.position = _location;
        }
    }

    private void Build()
    {
        isPreviewActivate = false;
        building.GetComponent<Collider>().isTrigger = false;
        building.GetComponent<BuildingPreview>().Destroy();
        alterController.GoMining(building);
        Destroy(building.GetComponent<BuildingPreview>());
    }

    public void ConstructBuilding(int buildingNum)
    {
        if(buildingNum == (int) Building.Alter)
        {
            Debug.Log("Building Alter is not supported yet.");
            return;
        }

        SetBuildBtnActivate();
        SetBuildListBtnActivate();

        building = Instantiate(Buildings[buildingNum]);
        building.AddComponent<BuildingPreview>();
        building.GetComponent<BuildingPreview>().Init(buildingNum);
        isPreviewActivate = true;
    }

    private void SetBuildBtnActivate()
    {

        build.gameObject.SetActive(isBtnActivate);
        isBtnActivate = !isBtnActivate;
    }

    public void SetBuildListBtnActivate()
    {
        buildList.gameObject.SetActive(isBtnListActivate);
        isBtnListActivate = !isBtnListActivate;
    }
}
