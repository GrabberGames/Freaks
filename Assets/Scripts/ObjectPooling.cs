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
    private int WhiteFreaksCountLimit = 100;                          //ȭ��Ʈ ������ �α��� ����
    [SerializeField]
    private GameObject WhiteFreaksObject;       //ȭ��Ʈ ������ ������
    private Queue<GameObject> WhiteFreaksQueue = new Queue<GameObject>();    //ȭ��Ʈ ������ ������Ʈ ť

    [SerializeField]
    private int BlackFreaksCountLimit = 100;                          // ���� ������ �α��� ����
    [SerializeField]
    private GameObject BlackFreaksObject;       //���� ������ ������

    private Queue<GameObject> BlackFreaksQueue = new Queue<GameObject>();    //���� ������ ������Ʈ ť


    private void Awake()
    {
        Init();
        _load();
        Initialize();
    }
    private void Initialize()                                             //�ʱ� ����
    {
        for (int i = 0; i < WhiteFreaksCountLimit; i++)     //ȭ��Ʈ ������ �α��� ���� ��ŭ Instantiate�� �����ؼ� Queue�� Enqueue
        {
            WhiteFreaksQueue.Enqueue(CreateNewObject("WhiteFreaks"));
        }
        for (int i = 0; i < BlackFreaksCountLimit; i++)     //���� ������ �α��� ���� ��ŭ Instantiate�� �����ؼ� Queue�� Enqueue
        {
            BlackFreaksQueue.Enqueue(CreateNewObject("BlackFreaks"));
        }
    }
    private GameObject CreateNewObject(string Obj)  //Instatiate�� ������Ʈ ����
    {
        GameObject newObj;
        switch (Obj)
        {
            case "WhiteFreaks":
                newObj = Instantiate(WhiteFreaksObject);
                //newObj.gameObject.transform.SetParent(this.gameObject.transform);
                newObj.SetActive(false);
                return newObj;
            case "BlackFreaks":
                Vector3 spawnPos = new Vector3(0, -20, 200);
                newObj = Instantiate(BlackFreaksObject, spawnPos, Quaternion.identity);

                newObj.SetActive(false);
                return newObj;
            default:
                break;
        }
        return null;
    }
    public GameObject GetObject(string objectName)               //�ش� ������Ʈ�� ����� ��ũ��Ʈ���� ȣ���ϸ� ��
    {
        switch (objectName)
        {
            case ("WhiteFreaks"):
                if (Instance.WhiteFreaksQueue.Count > 0)                           //����� �� �ִ� ȭ��Ʈ�������� ������
                {
                    var obj = Instance.WhiteFreaksQueue.Dequeue();           //Dequeue�Ͽ� ��������
                    obj.transform.position = GameObject.Find("Alter").transform.position;
                    obj.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            case ("BlackFreaks"):
                if (Instance.BlackFreaksQueue.Count > 0)                         //����� �� �ִ� ���� �������� ������
                {
                    var obj = Instance.BlackFreaksQueue.Dequeue();          //Dequeue�Ͽ� ��������
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
    public void ReturnObject(GameObject obj)                     //�ı��� | ����� ������Ʈ ��ȯ & ����� ��ũ��Ʈ���� ȣ���ϸ� ��.
    {
        obj.gameObject.SetActive(false);                                          //�ش� ������Ʈ OFF

        switch (obj.name)
        {
            case ("WhiteFreaks"):
                Instance.WhiteFreaksQueue.Enqueue(obj);                     //ȭ��Ʈ ������ Queue�� Enqueue
                break;

            case ("BlackFreaks"):
                Instance.BlackFreaksQueue.Enqueue(obj);                     //���� ������ Queue�� Enqueue
                break;

            default:
                break;
        }
    }
}
