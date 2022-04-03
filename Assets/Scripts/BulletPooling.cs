using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    public static BulletPooling instance;



    [SerializeField]
    private int WhiteTowerBulletCountLimit = 100;
    [SerializeField]
    private GameObject WhiteTowerBullet;
    Queue<GameObject> WhiteBulletQueue = new Queue<GameObject>();
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
                ob_go.AddComponent<ObjectPooling>();
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
            WhiteBulletQueue.Enqueue(CreateNewObject("WhiteTowerBullet"));
        
      
    }

    private GameObject CreateNewObject(string Obj)
    {
        GameObject newObj;
        switch (Obj)
        {
            case "WhiteTowerBullet":
                newObj = Instantiate(WhiteTowerBullet, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;/* 
                blackTowerBullet도 추가할까 생각중..
            case "BlackFreaks":
                newObj = Instantiate(BlackFreaksObject, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;*/
            default:
                break;
        }
        return null;
    }

    public  static GameObject GetObject(string objectName)
    {
        switch (objectName) {
            case ("WhiteTowerBullet"):
                if (Instance.WhiteBulletQueue.Count > 0)
                {
                    Debug.Log("여기까지 들어왔나???");
                    var obj = instance.WhiteBulletQueue.Dequeue();
                    obj.transform.SetParent(null);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
                else
                 {
                    Debug.Log("안들어왔나...");
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
            case ("WhiteTowerBullet"):
                Instance.WhiteBulletQueue.Enqueue(obj);
                break;
                /*
            case ("BlackFreaks"):
                Instance.BlackFreaksQueue.Enqueue(obj);
                break;
                */
            default:
                break;
        }
    }






}
