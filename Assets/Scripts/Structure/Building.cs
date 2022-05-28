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

     private GameObject alter;
    public void Init()
    {
        alter = GameManager.Instance.Alter;
        switch (this.gameObject.name)
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
                GoBuild();
                break;





            case "build_whitetower":             
                objectName = "Whitetower";
                timer = 10;
         

                mr = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("UI/Unlit/Transparent");

                c = new Color(1f, 1f, 1f, 0.5f);
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
       whiteFreaks.GetComponent<WhiteFreaksController>().SetDestination(this.gameObject,true);

   
    }

    public void GoAlter()
    {

        whiteFreaks.GetComponent<WhiteFreaksController>().SetDestination(alter,false);
    }


    public void ChangeBuilding()
    {
        switch (objectName)
        {
            case "Alter":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                StartCoroutine(CompleteTimer());
                break;
            case "Whitetower":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                StartCoroutine(CompleteTimer());
                break;
            case "Workshop":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);

                transform.GetChild(1).gameObject.GetComponent<WorkshopController>().StartDigging();
                break;
            default:
                break;
        }
       

    }
    



    IEnumerator CompleteTimer()
    {

        yield return YieldInstructionCache.WaitForSeconds(timer);

        switch (objectName)
        {
            case "Alter":
          
                alter.transform.position = this.transform.position;

                AlterAttack alterAttack = alter.GetComponent<AlterAttack>();
                alterAttack.bulletSpawnNewSetting();

                BuildingManager.Instance.AlterIsChange.Invoke(this.gameObject);
                WhiteFreaksManager.Instance.ReturnWhiteFreaks(whiteFreaks);
                ReturnBuildingPool();
                break;
            case "Whitetower":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            default:
                break;

        }

        whiteFreaks.SetActive(true);
        GoAlter();
    }

    public void ReturnBuildingPool()
    {
        BuildingPooling.ReturnObject(this.gameObject);
    }


}
