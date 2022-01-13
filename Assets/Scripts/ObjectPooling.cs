using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance = null;
    static ObjectPooling Instance
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
            GameObject ob_go = GameObject.Find("ObjectPooling");

            if (ob_go == null)
            {
                ob_go = new GameObject { name = "ObjectPooling" };
                ob_go.AddComponent<ObjectPooling>();
            }
            DontDestroyOnLoad(ob_go);
            instance = ob_go.GetComponent<ObjectPooling>();
        }
    }

    Dictionary<string, Stat> data = new Dictionary<string, Stat>();
    public void _load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/JsonDotNet/StatusInfo.Json");
        data = JsonConvert.DeserializeObject<Stat>(jdata).Json;
    }
    public Stat Get_Stat(string _objName)
    {
        if (data.Count == 0)
            _load();

        var enumData = data.GetEnumerator();

        while (enumData.MoveNext())
        {
            if (enumData.Current.Key == _objName)
            {
                return enumData.Current.Value;
            }
        }
        return null;
    }

    [SerializeField]
    private int WhiteFreaksCountLimit = 100;                   
    [SerializeField]
    private GameObject WhiteFreaksObject;       
    private Queue<GameObject> WhiteFreaksQueue = new Queue<GameObject>();    

    [SerializeField]
    private int BlackFreaksCountLimit = 100;                       
    [SerializeField]
    private GameObject BlackFreaksObject;       
    private Queue<GameObject> BlackFreaksQueue = new Queue<GameObject>();

    public GameObject Alter = null;

    public void Alter_Setting(GameObject Alter)
    {
        this.Alter = Alter;
    }

    private void Awake()
    {
        Init();
        _load();
        Initialize();
    }
    private void Initialize()                                             
    {
        for (int i = 0; i < WhiteFreaksCountLimit; i++)     
        {
            WhiteFreaksQueue.Enqueue(CreateNewObject("WhiteFreaks"));
        }
        for (int i = 0; i < BlackFreaksCountLimit; i++)     
        {
            BlackFreaksQueue.Enqueue(CreateNewObject("BlackFreaks"));
        }
    }
    private GameObject CreateNewObject(string Obj)  
    {
        GameObject newObj;
        switch (Obj)
        {
            case "WhiteFreaks":
                newObj = Instantiate(WhiteFreaksObject, gameObject.transform);
                newObj.SetActive(false);
                return newObj;
            case "BlackFreaks":
                newObj = Instantiate(BlackFreaksObject, gameObject.transform);
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
            case ("WhiteFreaks"):
                if (Instance.WhiteFreaksQueue.Count > 0)                          
                {
                    var obj = Instance.WhiteFreaksQueue.Dequeue();
                    obj.transform.position = Alter.transform.position;
                    obj.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            case ("BlackFreaks"):
                if (Instance.BlackFreaksQueue.Count > 0)                        
                {
                    var obj = Instance.BlackFreaksQueue.Dequeue();         
                    obj.SetActive(true);
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
    public void ReturnObject(GameObject obj)                    
    {
        obj.gameObject.SetActive(false);                               

        switch (obj.name)
        {
            case ("WhiteFreaks"):
                Instance.WhiteFreaksQueue.Enqueue(obj);         
                break;

            case ("BlackFreaks"):
                Instance.BlackFreaksQueue.Enqueue(obj);         
                break;

            default:
                break;
        }
    }
}
