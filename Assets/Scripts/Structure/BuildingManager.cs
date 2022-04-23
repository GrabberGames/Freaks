using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    [Header("Building Setting")]
    [SerializeField] private GameObject build_alter;
    [SerializeField] private GameObject build_whitetower;
    [SerializeField] private GameObject build_workshop;

    [Header("Alter")]
    [SerializeField] private AlterController altercontroller;
    [SerializeField] private GameObject alter;

    private List<WhiteTowerAttack> whiteTowerList = new List<WhiteTowerAttack>();

    private GameObject go;


    private static BuildingManager mInstance;
    public static BuildingManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<BuildingManager>();
            }
            return mInstance;
        }
    }
    // test용도
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetBuildPosition("Alter", new Vector3(0,0,0));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetBuildPosition("Whitetower", new Vector3(0, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetBuildPosition("Workshop", new Vector3(0, 0, 0));
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
           BuildingRangeON(true);
        }

    }
    */
    private void Start()
    {
        build_alter.SetActive(false);
        build_whitetower.SetActive(false);
        build_workshop.SetActive(false);
    }

    public void SetBuildPosition(eBuilding buildingType, Vector3 pos)
    {
        switch (buildingType)
        {
            case eBuilding.Alter:
                go = Instantiate(build_alter, pos, transform.rotation);
                go.SetActive(true);
                go.GetComponent<Building11>().Init();

                break;
            case eBuilding.WhiteTower:
                go = Instantiate(build_whitetower, pos, transform.rotation);
                whiteTowerList.Add(go.transform.GetChild(1).gameObject.GetComponent<WhiteTowerAttack>());
                go.SetActive(true);
                go.GetComponent<Building11>().Init();

                break;
            case eBuilding.Workshop:
                go = Instantiate(build_workshop, pos, transform.rotation);
                go.SetActive(true);
                go.GetComponent<Building11>().Init();

                break;
            default:
                break;
        }
    }

    public void BuildingRangeON(bool check) //건물들 rangeON
    {
        InterfaceRange interfaceRange;
     
            interfaceRange = altercontroller.GetComponent<InterfaceRange>();
            interfaceRange.BuildingRangeON(check);

        for (int i = 0; i < whiteTowerList.Count; i++)
        {
            if (whiteTowerList[i] != null)
            {
                interfaceRange = whiteTowerList[i].GetComponent<InterfaceRange>();
                interfaceRange.BuildingRangeON(check);
            }
            else
            {
                whiteTowerList.RemoveAt(i); //whitetower 소멸되었을 경우
            }
        }
    }

    public  Vector3 GetAlterPosition() //Alter 위치 전달
    {
        return alter.gameObject.transform.position;
    }

    public  float GetAlterBuildRadius() //알터 건설가능범위 radius 전달
    {
        float radius = (alter.gameObject.transform.GetChild(2).gameObject.transform.localScale.x) / 2;
        return radius;
    }

    public  float GetAlterNoBuildRadius() //알터 건설 불가능범위 radius 전달
    {
        float radius = ((alter.gameObject.transform.GetChild(2).gameObject.transform.localScale.x) * (float)0.3) / 2;
        return radius;
    }


}
