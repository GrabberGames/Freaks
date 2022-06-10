using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPooling : MonoBehaviour
{

    

    public static BuildingPooling instance;

    [SerializeField]
    private int CountLimit = 15;
    [Header("Alter Building")]
    [SerializeField]
    private GameObject alter_before;
    [SerializeField]
    private GameObject alter_building;

    Queue<GameObject> alter_beforeQueue = new Queue<GameObject>();
    Queue<GameObject> alter_buildingQueue = new Queue<GameObject>();


    [Header("WhiteTower Building")]
    [SerializeField]
    private GameObject whitetower_before;
    [SerializeField]
    private GameObject whitetower_building;

    Queue<GameObject> whitetower_beforeQueue = new Queue<GameObject>();
    Queue<GameObject> whitetower_buildingQueue = new Queue<GameObject>();

    [Header("Workshop Building")]
    [SerializeField]
    private GameObject workshop_before;
    Queue<GameObject> workshop_beforeQueue = new Queue<GameObject>();

    [Header("After Building")]
    [SerializeField]
    private GameObject building_after;
    Queue<GameObject> building_afterQueue = new Queue<GameObject>();

    [SerializeField]
    private int buildingCountLimit = 20;
    [SerializeField]
    private int whitetowerCountLimit = 15;
    [SerializeField]
    private int workshopCountLimit = 15;

    [Header("Building")]    
    [SerializeField]
    private GameObject building;
    [SerializeField]
    private GameObject whitetower;
    [SerializeField]
    private GameObject workshop;


    Queue<GameObject> BuildingQueue = new Queue<GameObject>();
    Queue<GameObject> WhiteTowerQueue = new Queue<GameObject>();
    Queue<GameObject> WorkshopQueue = new Queue<GameObject>();



    static BuildingPooling Instance
    {
        get
        {
            Init();
            return instance;
        }
    }
    static void Init()
    {
        if (instance == null)
        {
            GameObject ob_go = GameObject.Find("@BuildingPooling");

            if (ob_go == null)
            {
                ob_go = new GameObject { name = "@BuildingPooling" };
                ob_go.AddComponent<BuildingPooling>();
            }
            DontDestroyOnLoad(ob_go);
            instance = ob_go.GetComponent<BuildingPooling>();
        }
    }

    private void Awake()
    {
        Init();
        Initialize();
    }


    private void Initialize()
    {
        for (int i = 0; i < buildingCountLimit; i++)
            BuildingQueue.Enqueue(CreateNewObject("building"));
        for (int i = 0; i < whitetowerCountLimit; i++)
            WhiteTowerQueue.Enqueue(CreateNewObject("build_whitetower"));
        for (int i = 0; i < workshopCountLimit; i++)
            WorkshopQueue.Enqueue(CreateNewObject("build_workshop"));
        for (int i = 0; i < CountLimit; i++)
        {
            alter_beforeQueue.Enqueue(CreateNewObject("alter_before"));
            alter_buildingQueue.Enqueue(CreateNewObject("alter_building"));
            building_afterQueue.Enqueue(CreateNewObject("building_after"));
            whitetower_beforeQueue.Enqueue(CreateNewObject("whitetower_before"));
            whitetower_buildingQueue.Enqueue(CreateNewObject("whitetower_building"));
            workshop_beforeQueue.Enqueue(CreateNewObject("workshop_before"));



        }

    }

    GameObject newObj;

    private GameObject CreateNewObject(string Obj)
    {
        switch (Obj)
        {
            case "building":
                newObj = Instantiate(building, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "build_whitetower":
                newObj = Instantiate(whitetower, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "build_workshop":
                newObj = Instantiate(workshop, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "alter_before":
                newObj = Instantiate(alter_before, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "alter_building":
                newObj = Instantiate(alter_building, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "building_after":
                newObj = Instantiate(building_after, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "whitetower_before":
                newObj = Instantiate(whitetower_before, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "whitetower_building":
                newObj = Instantiate(whitetower_building, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "workshop_before":
                newObj = Instantiate(workshop_before, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            default:
                break;
        }
        return null;
    }



    public GameObject GetObject(string objectName)
    {
        switch (objectName)
        {
            case ("building"):
                if (Instance.BuildingQueue.Count <= 0)
                    instance.BuildingQueue.Enqueue(CreateNewObject("building"));

                newObj = instance.BuildingQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("build_whitetower"):
                if (Instance.WhiteTowerQueue.Count <= 0)
                    instance.WhiteTowerQueue.Enqueue(CreateNewObject("build_whitetower"));

                newObj = instance.WhiteTowerQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("build_workshop"):
                if (Instance.WorkshopQueue.Count <= 0)
                    instance.WorkshopQueue.Enqueue(CreateNewObject("build_workshop"));

                newObj = instance.WorkshopQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("alter_before"):
                if (Instance.alter_beforeQueue.Count <= 0)
                    instance.alter_beforeQueue.Enqueue(CreateNewObject("alter_before"));

                newObj = instance.alter_beforeQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("alter_building"):
                if (Instance.alter_buildingQueue.Count <= 0)
                    instance.alter_buildingQueue.Enqueue(CreateNewObject("alter_building"));

                newObj = instance.alter_buildingQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("building_after"):
                if (Instance.building_afterQueue.Count <= 0)
                    instance.building_afterQueue.Enqueue(CreateNewObject("building_after"));

                newObj = instance.building_afterQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("whitetower_before"):
                if (Instance.whitetower_beforeQueue.Count <= 0)
                    instance.whitetower_beforeQueue.Enqueue(CreateNewObject("whitetower_before"));

                newObj = instance.whitetower_beforeQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("whitetower_building"):
                if (Instance.whitetower_buildingQueue.Count <= 0)
                    instance.whitetower_buildingQueue.Enqueue(CreateNewObject("whitetower_building"));

                newObj = instance.whitetower_buildingQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("workshop_before"):
                if (Instance.workshop_beforeQueue.Count <= 0)
                    instance.workshop_beforeQueue.Enqueue(CreateNewObject("workshop_before"));

                newObj = instance.workshop_beforeQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            default:
                return null;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);

        switch (obj.name)
        {
            case ("buildings"):
                Instance.BuildingQueue.Enqueue(obj);
                break;

            case ("whitetower"):
                Instance.WhiteTowerQueue.Enqueue(obj);
                break;

            case ("workshop"):
                Instance.WorkshopQueue.Enqueue(obj);
                break;
            case ("alter_before"):
                Instance.alter_beforeQueue.Enqueue(obj);
                break;

            case ("alter_building"):
                Instance.alter_buildingQueue.Enqueue(obj);
                break;

            case ("FX_Construc_End"):
                Instance.building_afterQueue.Enqueue(obj);
                break;
            case ("whitetower_before"):
                Instance.whitetower_beforeQueue.Enqueue(obj);
                break;

            case ("whitetower_building"):
                Instance.whitetower_buildingQueue.Enqueue(obj);
                break;

            case ("workshop_before"):
                Instance.workshop_beforeQueue.Enqueue(obj);
                break;

            default:
                break;
        }
    }


}