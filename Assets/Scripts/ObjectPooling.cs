using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance = null;

    [SerializeField]
    private int WhiteFreaksCountLimit = 100;                          //화이트 프릭스 인구수 제한
    [SerializeField]
    private GameObject WhiteFreaksObject;       //화이트 프릭스 프리팹
    private Queue<GameObject> WhiteFreaksQueue = new Queue<GameObject>();    //화이트 프릭스 오브젝트 큐

    [SerializeField]
    private int BlackFreaksCountLimit = 100;                          // 블랙 프릭스 인구수 제한
    [SerializeField]
    private GameObject BlackFreaksObject;       //블랙 프릭스 프리팹
    private Queue<GameObject> BlackFreaksQueue = new Queue<GameObject>();    //블랙 프릭스 오브젝트 큐


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(this.gameObject);

        Initialize();
    }
    private void Initialize()                                             //초기 설정
    { 
        for (int i = 0; i < WhiteFreaksCountLimit; i++)     //화이트 프릭스 인구수 제한 만큼 Instantiate로 생성해서 Queue에 Enqueue
        {
            WhiteFreaksQueue.Enqueue(CreateNewObject("WhiteFreaks")); 
        }
        for (int i = 0; i < BlackFreaksCountLimit; i++)     //블랙 프릭스 인구수 제한 만큼 Instantiate로 생성해서 Queue에 Enqueue
        {
            BlackFreaksQueue.Enqueue(CreateNewObject("BlackFreaks"));
        }
    }
    private GameObject CreateNewObject(string Obj)  //Instatiate로 오브젝트 생성
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
                Vector3 spawnPos = new Vector3(0, 0, 0);
                newObj = Instantiate(BlackFreaksObject, spawnPos, Quaternion.identity);
                newObj.SetActive(false);
                return newObj;
            default:
                break;
        }
        return null;
    }
    public GameObject GetObject(string objectName)               //해당 오브젝트를 사용할 스크립트에서 호출하면 됨
    { 
        switch(objectName)
        {
            case ("WhiteFreaks"):
                if(Instance.WhiteFreaksQueue.Count > 0)                           //사용할 수 있는 화이트프릭스가 있으면
                {
                    var obj = Instance.WhiteFreaksQueue.Dequeue();           //Dequeue하여 리턴해줌
                    obj.transform.position = GameObject.Find("Alter").transform.position;
                    obj.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            case ("BlackFreaks"):
                if (Instance.BlackFreaksQueue.Count > 0)                         //사용할 수 있는 블랙 프릭스가 있으면
                {
                    var obj = Instance.BlackFreaksQueue.Dequeue();          //Dequeue하여 리턴해줌
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
    public void ReturnObject(GameObject obj)                     //파괴된 | 사라진 오브젝트 반환 & 사용할 스크립트에서 호출하면 됨.
    { 
        obj.gameObject.SetActive(false);                                          //해당 오브젝트 OFF

        switch (obj.name)
        {
            case ("WhiteFreaks"):
                Instance.WhiteFreaksQueue.Enqueue(obj);                     //화이트 프릭스 Queue에 Enqueue
                break;

            case ("BlackFreaks"):
                Instance.BlackFreaksQueue.Enqueue(obj);                     //블랙 프릭스 Queue에 Enqueue
                break;

            default:
                break;
        }
    }



}
