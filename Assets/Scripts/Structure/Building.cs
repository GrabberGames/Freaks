using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : MonoBehaviour
{
    private string objectName = null;
    [SerializeField] private int timer = 10;
    private GameObject whiteFreaks;

    MeshRenderer mr;
    Color c;

    GameObject go;
     private static GameObject alter;
    public void Init()
    {
        go = this.gameObject;
        alter = GameManager.Instance.Alter;
        switch (go.name)
        {
            case "build_alter":
                objectName = "Alter";
                timer = 10;



                mr = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("UI/Lit/Transparent");

                c = new Color(1f, 1f, 1f, 0.1f);
                mr.material.color = c;


                transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                // transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(false);

                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                GoBuild();
                break;





            case "build_whitetower":             
                objectName = "Whitetower";
                timer = 10;
         

                mr = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("UI/Unlit/Transparent");

                c = new Color(1f, 1f, 1f, 0.2f);
                mr.material.color = c;


                mr = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("UI/Unlit/Transparent");
                mr.material.color = c;

   
                transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(false);

                transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(5).gameObject.SetActive(false);
                GoBuild();

                //StartCoroutine("Test");
                // StartCoroutine("BuildingTimer");
            



                break;
            case "build_workshop":
                objectName = "Workshop";
                
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.GetComponent<WorkshopController>().SetConnectEssence(BuildingManager.Instance.GetEssenceSpot());


                GoBuild();
                break;
            default:
                break;
        }

    }

    public void GoBuild()
    {

        whiteFreaks = WhiteFreaksManager.Instance.GetWhiteFreaks();
        whiteFreaks.transform.position = alter.transform.position;
       whiteFreaks.GetComponent<WhiteFreaksController>().SetDestination(go,true);

        if (objectName.CompareTo("Workshop") ==0)
            transform.GetChild(1).gameObject.GetComponent<WorkshopController>().SetConnetingFreaks(whiteFreaks.GetComponent<WhiteFreaksController>());

    }

    public void GoAlter()
    {

        whiteFreaks.GetComponent<WhiteFreaksController>().SetDestination(alter,false);
    }

    AudioSource SFXBuilding;
    AudioSource SFXBuildingComplete;
    public void ChangeBuilding()
    {


        switch (objectName)
        {
            case "Alter":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                StartCoroutine(CompleteTimer());

                SFXBuilding = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
                SFXBuildingComplete = alter.gameObject.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.GetComponent<AudioSource>();
                SFXBuilding.Play();
                break;
            case "Whitetower":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                StartCoroutine(CompleteTimer());

                SFXBuilding = transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
                SFXBuildingComplete = transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
                SFXBuilding.Play();

                break;
            case "Workshop":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                SFXBuilding = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
                SFXBuilding.Play();

                transform.GetChild(1).gameObject.GetComponent<WorkshopController>().StartDigging();
                break;
            default:
                break;
        }
       

    }

    /// <summary>
    /// 건물이 모두 완성되었을때 실행되는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator CompleteTimer()
    {

        yield return YieldInstructionCache.WaitForSeconds(timer);

        switch (objectName)
        {
            case "Alter":
                WhiteFreaksManager.Instance.ReturnWhiteFreaks(whiteFreaks);

                StartCoroutine(ChangeAlter());


                break;
            case "Whitetower":
                transform.GetChild(5).gameObject.SetActive(true);

                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);

                SFXBuilding.Stop();
                SFXBuildingComplete.Play();

                whiteFreaks.SetActive(true);
                GoAlter();

                break;
            default:
                break;

        }

   
    }

    public void ReturnBuildingPool()
    {
       // Debug.Log("Return BUilding Name = " + go.name);
        BuildingPooling.instance.ReturnObject(go);
        Debug.Log("Return BUilding Name = " + go.name);
    }


 

    IEnumerator ChangeAlter()
    {
        SFXBuilding.Stop();
        yield return YieldInstructionCache.WaitForSeconds(0.02f);
        SFXBuildingComplete.Play();
        alter.transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(0.7f);


        alter.transform.position = this.transform.position;
        alter.transform.GetChild(4).gameObject.SetActive(false);

        AlterAttack alterAttack = alter.GetComponent<AlterAttack>();
        alterAttack.bulletSpawnNewSetting();

        BuildingManager.Instance.AlterIsChange.Invoke(alter);


        ReturnBuildingPool();
    }


}
