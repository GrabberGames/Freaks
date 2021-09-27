using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;

    [SerializeField]
    private int WhiteFreaksCountLimit;                          //화이트 프릭스 인구수 제한
    [SerializeField]
    private GameObject WhiteFreaksObjectPrefab;       //화이트 프릭스 프리팹
    private Queue<GameObject> WhiteFreaksQueue;    //화이트 프릭스 오브젝트 큐

    [SerializeField]
    private int BlackFreaksCountLimit;                          // 블랙 프릭스 인구수 제한
    [SerializeField]
    private GameObject BlackFreaksObjectPrefab;       //블랙 프릭스 프리팹
    private Queue<GameObject> BlackFreaksQueue;    //블랙 프릭스 오브젝트 큐


    private void Awake()
    {
        Instance = this;
        Initialize();
    }
    private void Initialize()                                             //초기 설정
    { 
        for (int i = 0; i < WhiteFreaksCountLimit; i++)     //화이트 프릭스 인구수 제한 만큼 Instantiate로 생성해서 Queue에 Enqueue
        {
            WhiteFreaksQueue.Enqueue(CreateNewObject(WhiteFreaksObjectPrefab)); 
        }
        for (int i = 0; i < BlackFreaksCountLimit; i++)     //블랙 프릭스 인구수 제한 만큼 Instantiate로 생성해서 Queue에 Enqueue
        {
            BlackFreaksQueue.Enqueue(CreateNewObject(BlackFreaksObjectPrefab));
        }
    }
    private GameObject CreateNewObject(GameObject objectName)  //Instatiate로 오브젝트 생성
    {
        var newObj = Instantiate(objectName);   //objectName에 해당하는 Prefab을 Instantiate로 생성 후 return
        newObj.SetActive(false);                         //해당 Object를 SetActive(false)로 설정.
        return newObj; 
    }
    public static GameObject GetObject(string objectName)               //해당 오브젝트를 사용할 스크립트에서 호출하면 됨
    { 
        switch(objectName)
        {
            case ("WhiteFreaks"):
                if(Instance.WhiteFreaksQueue.Count > 0)                           //사용할 수 있는 화이트프릭스가 있으면
                {
                    var obj = Instance.WhiteFreaksQueue.Dequeue();           //Dequeue하여 리턴해줌
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
    public static void ReturnObject(GameObject obj)                     //파괴된 | 사라진 오브젝트 반환 & 사용할 스크립트에서 호출하면 됨.
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
