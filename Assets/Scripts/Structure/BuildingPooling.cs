using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPooling : MonoBehaviour
{

    public static BuildingPooling instance;



    [SerializeField]
    private int alterCountLimit = 5;
    [SerializeField]
    private int whitetowerCountLimit = 15;
    [SerializeField]
    private int workshopCountLimit = 15;

    [SerializeField]
    private GameObject build_alter;
    [SerializeField]
    private GameObject build_whitetower;
    [SerializeField]
    private GameObject build_workshop;

    Queue<GameObject> AlterQueue = new Queue<GameObject>();
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
        for (int i = 0; i < alterCountLimit; i++)
            AlterQueue.Enqueue(CreateNewObject("build_alter"));
        for (int i = 0; i < whitetowerCountLimit; i++)
            WhiteTowerQueue.Enqueue(CreateNewObject("build_whitetower"));
        for (int i = 0; i < workshopCountLimit; i++)
            WorkshopQueue.Enqueue(CreateNewObject("build_workshop"));
    }

    GameObject newObj;

    private GameObject CreateNewObject(string Obj)
    {
        switch (Obj)
        {
            case "build_alter":
                newObj = Instantiate(build_alter, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "build_whitetower":
                newObj = Instantiate(build_whitetower, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "build_workshop":
                newObj = Instantiate(build_workshop, gameObject.transform);
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
            case ("build_alter"):
                if(Instance.AlterQueue.Count <= 0)
                    instance.AlterQueue.Enqueue(CreateNewObject("build_alter"));

                newObj = instance.AlterQueue.Dequeue();
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
                

            default:
                return null;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);

        switch (obj.name)
        {
            case ("build_alter"):
                Instance.AlterQueue.Enqueue(obj);
                break;

            case ("build_whittower"):
                Instance.WhiteTowerQueue.Enqueue(obj);
                break;

            case ("build_workshop"):
                Debug.Log("Return workshop count = " + instance.WorkshopQueue.Count);
                Instance.WorkshopQueue.Enqueue(obj);
                break;

            default:
                break;
        }
    }


}
