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

                GoBuild(this.gameObject);

                //StartCoroutine("Test");
                // StartCoroutine("BuildingTimer");
            



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
        alter = GameManager.Instance.Alter;
        whiteFreaks = WhiteFreaksManager.Instance.GetWhiteFreaks();
        whiteFreaks.transform.position = alter.transform.position;
        Vector3 po = new Vector3(transform.position.x + 1, transform.position.y + 2, transform.position.z);
        whiteFreaks.GetComponent<WhiteFreaksController>().SetDestination(po);


    }
    private IDisposable arriveStream =
    default;
    private Action onArriveCallback =
      default;



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

                BuildingManager.Instance.AlterIsChange.Invoke(this.gameObject);
                ReturnBuildingPool();
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
