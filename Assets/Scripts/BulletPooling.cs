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
        for (int i = 0; i < WhiteTowerBulletCountLimit; i++)
            AlterBulletQueue.Enqueue(CreateNewObject("AlterBullet"));
        for (int i = 0; i < WhiteTowerBulletCountLimit; i++)
            WhiteBulletQueue.Enqueue(CreateNewObject("WhiteTowerBullet"));
        for (int i = 0; i < WhiteTowerBulletCountLimit; i++)
            BlackBulletQueue.Enqueue(CreateNewObject("BlackTowerBullet"));


    }

    private GameObject CreateNewObject(string Obj)
    {
        GameObject newObj;
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



    public  static GameObject GetObject(string objectName)
    {
        switch (objectName) {
            case ("AlterBullet"):
                if (Instance.AlterBulletQueue.Count > 0)
                {
                    var obj = instance.AlterBulletQueue.Dequeue();
                    obj.transform.SetParent(null);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            case ("WhiteTowerBullet"):
                if (Instance.WhiteBulletQueue.Count > 0)
                {
                    var obj = instance.WhiteBulletQueue.Dequeue();
                    obj.transform.SetParent(null);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
                else
                 {
                    return null;
                 }

            case ("BlackTowerBullet"):
                if (Instance.AlterBulletQueue.Count > 0)
                {
                    var obj = instance.BlackBulletQueue.Dequeue();
                    obj.transform.SetParent(null);
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
            case ("AlterBullet"):
                Instance.AlterBulletQueue.Enqueue(obj);
                break;

            case ("WhiteTowerBullet"):
                Instance.WhiteBulletQueue.Enqueue(obj);
                break;
            
            case ("BlackTowerBullet"):
                Instance.BlackBulletQueue.Enqueue(obj);
                break;
            
            default:
                break;
        }
    }






}
