using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : MonoBehaviour
{
    private string objectName = null;
    [SerializeField] private int timer = 10;


    MeshRenderer mr;
    Color c;

    private GameObject alter;
    public void Init()
    {
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
                StartCoroutine("Test");
                break;
            case "build_whitetower":             
                objectName = "Whitetower";
                timer = 10;
                /*
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
              
          
                StartCoroutine("BuildingTimer");*/


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

                StartCoroutine("Test");
                // StartCoroutine("BuildingTimer");
                //GoBuild(this.gameObject);



                break;
            case "build_workshop":
                objectName = "Workshop";
                
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.GetComponent<WorkshopController>().SetConnectEssence(BuildingManager.Instance.GetEssenceSpot());


                StartCoroutine(Test());
                break;
            default:
                break;
        }

    }
    public void GoBuild(GameObject building)
    {
        //busyWhiteF++;
        GameObject whiteFreaks = ObjectPooling.Instance.GetObject("WhiteFreaks");
        Vector3 po = new Vector3(transform.position.x + 1, transform.position.y + 2, transform.position.z);
        whiteFreaks.GetComponent<NavMeshAgent>().Warp(po);

        WhiteFreaksController whiteFreaksController = whiteFreaks.GetComponent<WhiteFreaksController>();
        //miningFreaks.Add(whiteFreaks);
        /*
        if (building.GetComponent<SwitchTimer>())
        {
            Vector3 pos = GameObject.Find("SwitchController").GetComponent<SwitchController>().Getpos();
            whiteFreaksController.SetSwitch(pos);
        }
        else if (building.GetComponent<WorkshopController>())
        {
            whiteFreaksController.miningWorkshop = building;
            whiteFreaksController.SetMiningWorkShop();
            //building.GetComponent<WorkshopController>().SetMiningFreeks(whiteFreaks);
        }*/

        whiteFreaksController.miningWorkshop = building;
        whiteFreaksController.SetMiningWorkShop();

    }


    /*
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }*/
    /*
    private void Update()
    {
        if (isTimerON)
        {
            StartCoroutine("BuildingTimer");
            isTimerON = false;
        }

    }*/

    /*
    private void OnTriggerEnter(Collider other) //화이트 프릭스가 도착한 경우
    {

        if (other.gameObject.CompareTag("WhiteFreaks"))
        {
            switch (objectName)
            {
                case "Alter":
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(true);
                    StartCoroutine("BuildingTimer");
                    break;
                case "Whitetower":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                StartCoroutine("BuildingTimer");
                    break;
                case "Workshop": //바로 건설
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
    */


    IEnumerator Test()
    {
        yield return new WaitForSeconds(10f);

        switch (objectName)
        {
            case "Alter":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "Whitetower":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
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


    IEnumerator BuildingTimer()
    {

        yield return new WaitForSeconds(timer);

        switch (objectName)
        {
            case "Alter":
                alter = GameObject.Find("alter").gameObject;
                alter.transform.position = this.transform.position;

                AlterAttack alterAttack = alter.GetComponent<AlterAttack>();
                alterAttack.bulletSpawnNewSetting();

                GameManager.Instance.AlterIsChange.Invoke(this.gameObject);
                Destroy(this.gameObject);
                break;
            case "Whitetower":
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            default:
                break;

        }
    }

    public void ReturnBuildingPool()
    {
        BuildingPooling.ReturnObject(this.gameObject);
    }

}
