using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    public static BulletPooling instance;



    [SerializeField]
    private int AlterBulletCountLimit = 100;
    [SerializeField]
    private int WhiteTowerBulletCountLimit = 100;
    [SerializeField]
    private int BlackTowerBulletCountLimit = 100;

    [SerializeField]
    private GameObject AlterBullet;
    [SerializeField]
    private GameObject WhiteTowerBullet;
    [SerializeField]
    private GameObject BlackTowerBullet;

    Queue<GameObject> AlterBulletQueue = new Queue<GameObject>();
    Queue<GameObject> WhiteBulletQueue = new Queue<GameObject>();
    Queue<GameObject> BlackBulletQueue = new Queue<GameObject>();
    static BulletPooling Instance
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
            GameObject ob_go = GameObject.Find("@BulletPooling");

            if (ob_go == null)
            {
                ob_go = new GameObject { name = "@BulletPooling" };
                ob_go.AddComponent<BulletPooling>();
            }
            DontDestroyOnLoad(ob_go);
            instance = ob_go.GetComponent<BulletPooling>();
        }
    }

    private void Awake()
    {
        Init();
        Initialize();
    }


    private void Initialize()
    {
        for (int i = 0; i < AlterBulletCountLimit; i++)
            AlterBulletQueue.Enqueue(CreateNewObject("AlterBullet"));
        for (int i = 0; i < WhiteTowerBulletCountLimit; i++)
            WhiteBulletQueue.Enqueue(CreateNewObject("WhiteTowerBullet"));
        for (int i = 0; i < BlackTowerBulletCountLimit; i++)
            BlackBulletQueue.Enqueue(CreateNewObject("BlackTowerBullet"));


    }

    GameObject newObj;
    private GameObject CreateNewObject(string Obj)
    {
        switch (Obj)
        {
            case "AlterBullet":
                newObj = Instantiate(AlterBullet, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "WhiteTowerBullet":
                newObj = Instantiate(WhiteTowerBullet, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case "BlackTowerBullet":
                newObj = Instantiate(BlackTowerBullet, gameObject.transform);
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
            case ("AlterBullet"):
                if (Instance.AlterBulletQueue.Count <= 0)
                    instance.AlterBulletQueue.Enqueue(CreateNewObject("AlterBullet"));

                newObj = instance.AlterBulletQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("WhiteTowerBullet"):
                if (Instance.WhiteBulletQueue.Count <= 0)
                    instance.WhiteBulletQueue.Enqueue(CreateNewObject("WhiteTowerBullet"));

                newObj = instance.WhiteBulletQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case ("BlackTowerBullet"):
                if (Instance.AlterBulletQueue.Count <= 0)
                    instance.BlackBulletQueue.Enqueue(CreateNewObject("BlackTowerBullet"));

                newObj = instance.BlackBulletQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            default:
                return null;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);

        switch (obj.name)
        {
            case ("AlterBullet"):
                Instance.AlterBulletQueue.Enqueue(obj);
                break;

            case ("WhiteTowerBullet"):
                Instance.WhiteBulletQueue.Enqueue(obj);
                break;

            case ("TowerBullet"):
                Instance.BlackBulletQueue.Enqueue(obj);
                break;

            default:
                break;
        }
    }






}