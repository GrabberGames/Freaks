using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private Button build;
    [SerializeField]
    private GameObject BuildingList;

    private bool isBtnActivate = true;
    private bool isBuildingListBtnExsit = false;
    private bool isPreviewActivate = false;

    private float buildingHeight;

    private GameObject building;
    [SerializeField]
    private GameObject mbtnPrefab;
    [SerializeField]
    private Sprite[] buildingbtnImgs;
    [SerializeField]
    private GameObject[] Buildings;
    public Material[] materials;

    private enum Building
    {
        Alter,
        Tower,
        Workshop
    };

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name == "Alter_B")
                {
                    SetBuildBtnActive();
                }
            }
        }

        if(isPreviewActivate)
        {
            viewPreview();
            if (building.GetComponent<BuildingPreview>().isBuildable() && Input.GetMouseButtonDown(0))
            {
                Build();
            }
        }
    }

    #region showBuildingListBtn
    public void ShowBuildingList()
    {
        if(!isBuildingListBtnExsit)
        {
            makeBuildingListBtn();
            isBuildingListBtnExsit = true;
        }
        else
        {
            BuildingList.SetActive(true);
        }

    }

    private void makeBuildingListBtn()
    {
        for (int i = 0; i < buildingbtnImgs.Length; i++)
        {
            GameObject btn = Instantiate(mbtnPrefab);

            RectTransform btnpos = btn.GetComponent<RectTransform>();
            btn.transform.position = gameObject.transform.position;

            Image image = btn.GetComponent<Image>();
            Sprite btnsprite = buildingbtnImgs[i];
            image.sprite = btnsprite;

            btnpos.SetParent(BuildingList.transform);
            btnpos.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (30 * i), 30);

            switch (i)
            {
                case (int)Building.Alter:
                    btn.GetComponent<Button>().onClick.AddListener(() => BuildingConstruction((int)Building.Alter));
                    break;

                case (int)Building.Tower:
                    btn.GetComponent<Button>().onClick.AddListener(() => BuildingConstruction((int)Building.Tower));
                    break;

                case (int)Building.Workshop:
                    btn.GetComponent<Button>().onClick.AddListener(() => BuildingConstruction((int)Building.Workshop));
                    break;

                default:
                    break;
            }

        }
    }

    #endregion

    private void viewPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 _location = hit.point;
            _location.y = buildingHeight;
            building.transform.position = _location;
        }
    }

    private void Build()
    {
        isPreviewActivate = false;
        building.GetComponent<Renderer>().material = building.GetComponent<BuildingPreview>().material;
        building.GetComponent<Collider>().isTrigger = false;
        Destroy(building.GetComponent<BuildingPreview>());

    }

    private void BuildingConstruction(int buildingNum)
    {
        if(buildingNum == (int) Building.Alter)
        {
            Debug.Log("Building Alter is not supported yet.");
            return;
        }

        SetBuildBtnActive();

        building = Instantiate(Buildings[buildingNum]);
        buildingHeight = building.GetComponent<Renderer>().bounds.size.y / 2.0f;
        building.AddComponent<BuildingPreview>();
        isPreviewActivate = true;

        BuildingList.SetActive(false);
    }

    private void SetBuildBtnActive()
    {

        build.gameObject.SetActive(isBtnActivate);
        isBtnActivate = !isBtnActivate;
    }
}
