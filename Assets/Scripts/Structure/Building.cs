using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : MonoBehaviour
{
    private string objectName = null;
    [SerializeField] private int timer = 10;
    
    [Header("Building Sound")]
    [SerializeField]
    private GameObject building_sound;
    [SerializeField]
    private GameObject complete_sound;

    public int build_num;
    //0:Alter, 1:WhiteTower, 2:Workshop

    private GameObject whiteFreaks;

    MeshRenderer mr;
    Color c;

    GameObject go;
    GameObject building_before;
    GameObject building_building;
    GameObject building_complete;
    GameObject building;
    private static GameObject alter;
    public void Init()
    {
        go = this.gameObject;
        alter = GameManager.Instance.Alter;
        switch (build_num)
        {
            case 0:
                objectName = "Alter";
                building_before = BuildingPooling.instance.GetObject("alter_before");
                building_before.SetActive(true);
                building_before.transform.position = transform.position;
                timer = 10;
                
                mr = building_before.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("UI/Lit/Transparent");

                c = new Color(1f, 1f, 1f, 0.1f);
                mr.material.color = c;
                


                GoBuild();
                break;


            case 1:
                objectName = "Whitetower";
                timer = 10;
                building_before = BuildingPooling.instance.GetObject("whitetower_before");

                building_before.SetActive(true);
                building_before.transform.position = transform.position;
                
                mr = building_before.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("UI/Unlit/Transparent");

                c = new Color(1f, 1f, 1f, 0.2f);
                mr.material.color = c;


                mr = building_before.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("UI/Unlit/Transparent");
                mr.material.color = c;

                
                building_before.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                building_before.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                building_before.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(false);


                building_before.transform.GetChild(1).gameObject.SetActive(false);

                building_before.SetActive(true);
                BuildingManager.Instance.GetbuildingRangeList().Add(building_before.transform.GetChild(2).GetComponent< BuildingRange>());
                GoBuild();

                //StartCoroutine("Test");
                // StartCoroutine("BuildingTimer");




                break;
            case 2:
                objectName = "Workshop";
                building_before = BuildingPooling.instance.GetObject("workshop_before");
                building_before.SetActive(true);
              
                building_before.transform.position = new Vector3(transform.position.x, 2.17f, transform.position.z);
                /*
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.GetComponent<WorkshopController>().SetConnectEssence(BuildingManager.Instance.GetEssenceSpot());
                */

                GoBuild();
                break;
            default:
                break;
        }

    }
    GameObject workshop;
    public void GoBuild()
    {

        whiteFreaks = WhiteFreaksManager.Instance.GetWhiteFreaks();
        whiteFreaks.transform.position = alter.transform.position;
        whiteFreaks.GetComponent<WhiteFreaksController>().SetDestination(go, true);

        if (build_num==2)
        {
            workshop = BuildingPooling.instance.GetObject("build_workshop");
            workshop.transform.position = transform.position;
            if(!(go.CompareTag("Switch")))
            workshop.GetComponent<WorkshopController>().SetConnectEssence(BuildingManager.Instance.GetEssenceSpot());
            workshop.GetComponent<WorkshopController>().SetConnetingFreaks(whiteFreaks.GetComponent<WhiteFreaksController>());
            workshop.SetActive(false);
        }
           

    }

    public void GoAlter()
    {

        whiteFreaks.GetComponent<WhiteFreaksController>().SetDestination(alter, false);
    }

    AudioSource SFXBuilding;
    AudioSource SFXBuildingComplete;


    public void ChangeBuilding()
    {


        switch (build_num)
        {
            case 0:
                BuildingPooling.instance.ReturnObject(building_before);
                building_building = BuildingPooling.instance.GetObject("alter_building");
                building_building.transform.position = transform.position;
                building_building.SetActive(true);
                StartCoroutine(CompleteTimer());

                building_building.GetComponent<AudioSource>().Play();
                break;

            case 1:
                BuildingPooling.instance.ReturnObject(building_before);
                building_building = BuildingPooling.instance.GetObject("whitetower_building");
                building_building.transform.position = transform.position;
                building_building.SetActive(true);
                StartCoroutine(CompleteTimer());

                BuildingManager.Instance.GetbuildingRangeList().Add(building_building.transform.GetChild(5).GetComponent<BuildingRange>());


                building_building.GetComponent<AudioSource>().Play();
                break;

            case 2:
                BuildingPooling.instance.ReturnObject(building_before);
                workshop.SetActive(true);
                workshop.transform.position = new Vector3(transform.position.x, 2.17f, transform.position.z);
                workshop.GetComponent<WorkshopState>().SFXworkshopComplete.Play();
                if (BuildingManager.Instance.GetEssenceSpot().CompareTag("Switch"))
                {
                    workshop.GetComponent<WorkshopController>().StartPurify();
                }
                else
                {
                    workshop.GetComponent<WorkshopController>().StartDigging();
                }
                BuildingPooling.instance.ReturnObject(this.gameObject);
                break;
            default:
                break;
        }


    }
    GameObject whiteTower;
    /// <summary>
    /// �ǹ��� ��� �ϼ��Ǿ����� ����Ǵ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    /// <returns></returns>
    IEnumerator CompleteTimer()
    {

        yield return YieldInstructionCache.WaitForSeconds(timer);

        switch (build_num)
        {
            case 0:
                WhiteFreaksManager.Instance.ReturnWhiteFreaks(whiteFreaks);

                StartCoroutine(ChangeAlter());


                break;
            case 1:
                building_building.GetComponent<AudioSource>().Stop();
                whiteTower = BuildingPooling.instance.GetObject("build_whitetower");
                whiteTower.transform.position = transform.position;
                whiteTower.SetActive(true);

                BuildingPooling.instance.ReturnObject(building_building);
                building_complete = BuildingPooling.instance.GetObject("building_after");
                building_complete.transform.position = transform.position;
                building_complete.SetActive(true);
                
                whiteTower.GetComponent<WhiteTowerAttack>().SFXWhiteTowerComplete.Play();
                yield return YieldInstructionCache.WaitForSeconds(0.7f);
                whiteFreaks.SetActive(true);
                GoAlter();
                BuildingPooling.instance.ReturnObject(building_complete);
                BuildingManager.Instance.GetbuildingRangeList().Add(whiteTower.transform.GetChild(2).GetComponent<BuildingRange>());
                BuildingPooling.instance.ReturnObject(this.gameObject);
                break;
            default:
                break;

        }


    }

    public void ReturnBuildingPool()
    {
        BuildingPooling.instance.ReturnObject(go);
    }



    GameObject alterComplete;
    IEnumerator ChangeAlter()
    {
        building_building.GetComponent<AudioSource>().Stop();
     

        yield return YieldInstructionCache.WaitForSeconds(0.02f);
        building_building.transform.GetChild(7).GetComponent<AudioSource>().Play();
        alter.transform.GetChild(4).gameObject.SetActive(true);

        building_complete = BuildingPooling.instance.GetObject("building_after");
        alterComplete = BuildingPooling.instance.GetObject("building_after");
        alterComplete.transform.position = alter.transform.position;
        alterComplete.SetActive(true);

        building_complete.transform.position = transform.position;
        yield return YieldInstructionCache.WaitForSeconds(0.8f);

        BuildingPooling.instance.ReturnObject(building_building);


        alter.transform.position = transform.position;
        alter.transform.GetChild(4).gameObject.SetActive(false);

        AlterAttack alterAttack = alter.GetComponent<AlterAttack>();
        alterAttack.bulletSpawnNewSetting();

        BuildingManager.Instance.AlterIsChange.Invoke(alter);


        ReturnBuildingPool();
        BuildingPooling.instance.ReturnObject(building_complete);
        BuildingPooling.instance.ReturnObject(alterComplete);
        BuildingPooling.instance.ReturnObject(this.gameObject);
    }


}