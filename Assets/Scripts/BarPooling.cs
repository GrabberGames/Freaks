using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPooling : MonoBehaviour
{
    public enum bar_name
    {
        enemy_bar,
        ally_bar,
        switch_bar
    }
  
    public static BarPooling instance;

    [SerializeField]
    private int CountLimit = 30;

    [SerializeField]
    private GameObject enemy_bar;
    [SerializeField]
    private GameObject ally_bar;
    [SerializeField]
    private GameObject switch_bar;


    Queue<GameObject> enemy_barQueue = new Queue<GameObject>();
    Queue<GameObject> ally_barQueue = new Queue<GameObject>();
    Queue<GameObject> switch_barQueue = new Queue<GameObject>();




    static BarPooling Instance
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
            GameObject ob_go = GameObject.Find("BarCanvas");

            if (ob_go == null)
            {
                ob_go = new GameObject { name = "BarCanvas" };
                ob_go.AddComponent<BarPooling>();
            }

            instance = ob_go.GetComponent<BarPooling>();
        }
    }

    private void Awake()
    {
        Init();
        Initialize();
    }


    private void Initialize()
    {
        for (int i = 0; i < CountLimit; i++)
        {
            enemy_barQueue.Enqueue(CreateNewObject(bar_name.enemy_bar));
            ally_barQueue.Enqueue(CreateNewObject(bar_name.ally_bar));
        
        }
        for (int i = 0; i < 3; i++)
        {
            switch_barQueue.Enqueue(CreateNewObject(bar_name.switch_bar));
        }
    }

    GameObject newObj;

    private GameObject CreateNewObject(bar_name Obj)
    {
        switch (Obj)
        {
            case bar_name.enemy_bar:
                newObj = Instantiate(enemy_bar, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case bar_name.ally_bar:
                newObj = Instantiate(ally_bar, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;

            case bar_name.switch_bar:
                newObj = Instantiate(switch_bar, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj;
                
            default:
                break;
        }
        return null;
    }



    public GameObject GetObject(bar_name objectName)
    {
       
        switch (objectName)
        {
            case (bar_name.enemy_bar):
                if (Instance.enemy_barQueue.Count <= 0)
                    instance.enemy_barQueue.Enqueue(CreateNewObject(bar_name.enemy_bar));

                newObj = instance.enemy_barQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case (bar_name.ally_bar):
                if (Instance.ally_barQueue.Count <= 0)
                    instance.ally_barQueue.Enqueue(CreateNewObject(bar_name.ally_bar));

                newObj = instance.ally_barQueue.Dequeue();
                newObj.SetActive(true);
                return newObj;

            case (bar_name.switch_bar):
                if (Instance.switch_barQueue.Count <= 0)
                    instance.switch_barQueue.Enqueue(CreateNewObject(bar_name.switch_bar));

                newObj = instance.switch_barQueue.Dequeue();
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
            case ("EnemyHpBar"):
                Instance.enemy_barQueue.Enqueue(obj);
                break;

            case ("AllyHpBar"):
                Instance.ally_barQueue.Enqueue(obj);
                break;

            case ("SwitchBar"):
                Instance.switch_barQueue.Enqueue(obj);
                break;
           
            default:
                break;
        }
    }

}
