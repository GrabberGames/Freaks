using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPooling : MonoBehaviour
{

    public static BuildingPooling instance;



    [SerializeField]
    private int alterCountLimit = 50;
    [SerializeField]
    private int whitetowerCountLimit = 50;
    [SerializeField]
    private int workshopCountLimit = 50;

    [SerializeField]
    private GameObject Building_alter;
    [SerializeField]
    private GameObject Building_whitetower;
    [SerializeField]
    private GameObject Building_workshop;

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
            AlterQueue.Enqueue(CreateNewObject("Building_alter"));
        for (int i = 0; i < whitetowerCountLimit; i++)
            WhiteTowerQueue.Enqueue(CreateNewObject("Building_whitetower"));
        for (int i = 0; i < workshopCountLimit; i++)
            WorkshopQueue.Enqueue(CreateNewObject("Building_workshop"));
    }


    private GameObject CreateNewObject(string Obj)
    {
        GameObject newObj;
        switch (Obj)
        {
            case "Building_alter":
                newObj = Instantiate(Building_alter, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "Building_whitetower":
                newObj = Instantiate(Building_whitetower, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "Building_workshop":
                newObj = Instantiate(Building_workshop, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;
            default:
                break;
        }
        return null;
    }



    public static GameObject GetObject(string objectName)
    {
        switch (objectName)
        {
            case ("Building_alter"):
                if (Instance.AlterQueue.Count > 0)
                {
                    var obj = instance.AlterQueue.Dequeue();
                    obj.gameObject.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            case ("Building_whitetower"):
                if (Instance.WhiteTowerQueue.Count > 0)
                {
                    var obj = instance.WhiteTowerQueue.Dequeue();
                    obj.gameObject.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            case ("Building_workshop"):
                if (Instance.WorkshopQueue.Count > 0)
                {
                    var obj = instance.WorkshopQueue.Dequeue();
                    obj.gameObject.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            default:
                return null;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);

        switch (obj.name)
        {
            case ("Building_alter"):
                Instance.AlterQueue.Enqueue(obj);
                break;

            case ("Building_whittower"):
                Instance.WhiteTowerQueue.Enqueue(obj);
                break;

            case ("Building_workshop"):
                Instance.WorkshopQueue.Enqueue(obj);
                break;

            default:
                break;
        }
    }


}
