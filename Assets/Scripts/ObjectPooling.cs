using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ObjectPooling : MonoBehaviour
{
    static ObjectPooling instance = null;
    public static ObjectPooling Instance
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
            GameObject ob_go = GameObject.Find("@ObjectPooling");

            if (ob_go == null)
            {
                ob_go = new GameObject { name = "@ObjectPooling" };
                ob_go.AddComponent<ObjectPooling>();
            }
            DontDestroyOnLoad(ob_go);
            instance = ob_go.GetComponent<ObjectPooling>();
        }
    }

    Dictionary<string, Stat> data = new Dictionary<string, Stat>();
    public void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/JsonDotNet/StatusInfo.Json");
        data = JsonConvert.DeserializeObject<Stat>(jdata).Json;
    }
    public Stat Get_Stat(string _objName)
    {
        if (data.Count == 0)
            Load();

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
    public void Set_Stat(string name, float pd = 0, float ed = 0, float hp = 0, float mhp = 0, float ats = 0, float ms = 0, float atr = 0, float ar = 0)
    {
        data[name].PD = pd;
        data[name].ED = ed;
        data[name].HP = hp;
        data[name].MAX_HP = mhp;
        data[name].ATTACK_SPEED = ats;
        data[name].MOVE_SPEED = ms;
        data[name].ATTACK_RANGE = atr;
        data[name].ARMOR = ar;
    }

    [SerializeField]
    private int _whiteFreaksCountLimit = 100;                   
    [SerializeField]
    private GameObject _whiteFreaksObject = null;       
    private Queue<GameObject> _whiteFreaksQueue = new Queue<GameObject>();    

    [SerializeField]
    private int _blackFreaksCountLimit = 100;                       
    [SerializeField]
    private GameObject _blackFreaksObject = null;       
    private Queue<GameObject> _blackFreaksQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleNormalAttackEmitCountLimit = 100;
    [SerializeField]
    private GameObject _kyleNormalAttackEmit = null;
    private Queue<GameObject> _kyleNormalAttackEmitQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleQSkillObjectCountLimit = 100;
    [SerializeField]
    private GameObject _kyleQSkillObject = null;
    private Queue<GameObject> _kyleQSkillObjectQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleWSkillObjectCountLimit = 100;
    [SerializeField]
    private GameObject _kyleWSkillObject = null;
    private Queue<GameObject> _kyleWSkillObjectQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleESkillObjectCountLimit = 100;
    [SerializeField]
    private GameObject _kyleESkillObject = null;
    private Queue<GameObject> _kyleESkillObjectQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleBulletObjectCountLimit = 100;
    [SerializeField]
    private GameObject _kyleBulletObject = null;
    private Queue<GameObject> _kyleBulletObjectQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleNormalBulletEffectCountLimit = 100;
    [SerializeField]
    private GameObject _kyleNormalBulletEffect = null;
    private Queue<GameObject> _kyleNormalBulletEffectQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleSkillEffectCountLimit = 100;
    [SerializeField]
    private GameObject _kyleSkillEffect = null;
    private Queue<GameObject> _kyleSkillEffectQueue = new Queue<GameObject>();

    [SerializeField]
    private int _kyleHitEffectCountLimit = 100;
    [SerializeField]
    private GameObject _kyleHitEffect = null;
    private Queue<GameObject> _kyleHitEffectQueue = new Queue<GameObject>();

    //KyleNormalBulletEffect
    //KyleSkillEffect
    //KyleHit
    private void Awake()
    {
        Init();
        Load();
        Initialize();
    }
    private void Initialize()                                             
    {
        for (int i = 0; i < _whiteFreaksCountLimit; i++)     
        {
            _whiteFreaksQueue.Enqueue(CreateNewObject<GameObject>("WhiteFreaks"));
        }
        for (int i = 0; i < _blackFreaksCountLimit; i++)     
        {
            _blackFreaksQueue.Enqueue(CreateNewObject<GameObject>("BlackFreaks"));
        }        
        for (int i = 0; i < _kyleNormalAttackEmitCountLimit; i++)     
        {
            _kyleNormalAttackEmitQueue.Enqueue(CreateNewObject<GameObject>("KyleNormalEmit"));
        }        
        for (int i = 0; i < _kyleQSkillObjectCountLimit; i++)     
        {
            _kyleQSkillObjectQueue.Enqueue(CreateNewObject<GameObject>("KyleQSkillEmit"));
        }        
        for (int i = 0; i < _kyleWSkillObjectCountLimit; i++)     
        {
            _kyleWSkillObjectQueue.Enqueue(CreateNewObject<GameObject>("KyleWSkillEmit"));
        }        
        for (int i = 0; i < _kyleESkillObjectCountLimit; i++)     
        {
            _kyleESkillObjectQueue.Enqueue(CreateNewObject<GameObject>("KyleESkillEmit"));
        }        
        for (int i = 0; i < _kyleBulletObjectCountLimit; i++)     
        {
            _kyleBulletObjectQueue.Enqueue(CreateNewObject<GameObject>("KyleBullet"));
        }        
        for (int i = 0; i < _kyleNormalBulletEffectCountLimit; i++)     
        {
            _kyleNormalBulletEffectQueue.Enqueue(CreateNewObject<GameObject>("KyleNormalBulletEffect"));
        }        
        for (int i = 0; i < _kyleSkillEffectCountLimit; i++)     
        {
            _kyleSkillEffectQueue.Enqueue(CreateNewObject<GameObject>("KyleSkillEffect"));
        }        
        for (int i = 0; i < _kyleHitEffectCountLimit; i++)     
        {
            _kyleHitEffectQueue.Enqueue(CreateNewObject<GameObject>("KyleHitEffect"));
        }
    }
    private T CreateNewObject<T>(string Obj) where T : UnityEngine.Object
    {
        GameObject newObj;
        switch (Obj)
        {
            case "WhiteFreaks":
                newObj = Instantiate(_whiteFreaksObject, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;
            case "BlackFreaks":
                newObj = Instantiate(_blackFreaksObject, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;
            case "KyleNormalEmit":
                newObj = Instantiate(_kyleNormalAttackEmit, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;
            case "KyleQSkillEmit":
                newObj = Instantiate(_kyleQSkillObject, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;
            case "KyleWSkillEmit":
                newObj = Instantiate(_kyleWSkillObject, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;
            case "KyleESkillEmit":
                newObj = Instantiate(_kyleESkillObject, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;
            case "KyleBullet":
                newObj = Instantiate(_kyleBulletObject, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;            
            case "KyleNormalBulletEffect":
                newObj = Instantiate(_kyleNormalBulletEffect, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;            
            case "KyleSkillEffect":
                newObj = Instantiate(_kyleSkillEffect, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;           
            case "KyleHitEffect":
                newObj = Instantiate(_kyleHitEffect, gameObject.transform);
                newObj.transform.name = newObj.name.Replace("(Clone)", "");
                newObj.SetActive(false);
                return newObj as T;
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
                if (Instance._whiteFreaksQueue.Count > 0)                          
                {
                    var obj = Instance._whiteFreaksQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

            case ("BlackFreaks"):
                if (Instance._blackFreaksQueue.Count > 0)                        
                {
                    var obj = Instance._blackFreaksQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else 
                    return null;

            case ("KyleNormalEmit"):
                if (Instance._kyleNormalAttackEmitQueue.Count > 0)
                {
                    var obj = Instance._kyleNormalAttackEmitQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

            case ("KyleQSkillEmit"):
                if (Instance._kyleQSkillObjectQueue.Count > 0)
                {
                    var obj = Instance._kyleQSkillObjectQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

            case ("KyleWSkillEmit"):
                if (Instance._kyleWSkillObjectQueue.Count > 0)
                {
                    var obj = Instance._kyleWSkillObjectQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

            case ("KyleESkillEmit"):
                if (Instance._kyleESkillObjectQueue.Count > 0)
                {
                    var obj = Instance._kyleESkillObjectQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

            case ("KyleBullet"):
                if (Instance._kyleBulletObjectQueue.Count > 0)
                {
                    var obj = Instance._kyleBulletObjectQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

            case ("KyleNormalBulletEffect"):
                if (Instance._kyleNormalBulletEffectQueue.Count > 0)
                {
                    var obj = Instance._kyleNormalBulletEffectQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

            case ("KyleSkillEffect"):
                if (Instance._kyleSkillEffectQueue.Count > 0)
                {
                    var obj = Instance._kyleSkillEffectQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;
                

            case ("KyleHitEffect"):
                if (Instance._kyleHitEffectQueue.Count > 0)
                {
                    var obj = Instance._kyleHitEffectQueue.Dequeue();
                    obj.transform.name = obj.name.Replace("(Clone)", "");
                    obj.SetActive(true);
                    return obj;
                }
                else
                    return null;

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
                obj.SetActive(false);
                Instance._whiteFreaksQueue.Enqueue(obj);         
                break;

            case ("BlackFreaks"):
                obj.SetActive(false);
                Instance._blackFreaksQueue.Enqueue(obj);
                break;

            case ("KyleNormalEmit"):
                obj.SetActive(false);
                Instance._kyleNormalAttackEmitQueue.Enqueue(obj);
                break;

            case ("KyleQEmit"):
                obj.SetActive(false);
                Instance._kyleQSkillObjectQueue.Enqueue(obj);
                break;

            case ("KyleWEmit"):
                obj.SetActive(false);
                Instance._kyleWSkillObjectQueue.Enqueue(obj);
                break;

            case ("KyleEEmit"):
                obj.SetActive(false);
                Instance._kyleESkillObjectQueue.Enqueue(obj);
                break;

            case ("KyleBullet"):
                obj.SetActive(false);
                Instance._kyleBulletObjectQueue.Enqueue(obj);
                break;            
            
            case ("KyleNormalBulletEffect"):
                obj.SetActive(false);
                Instance._kyleNormalBulletEffectQueue.Enqueue(obj);
                break;
                            
            case ("KyleSkillEffect"):
                obj.SetActive(false);
                Instance._kyleSkillEffectQueue.Enqueue(obj);
                break;
                            
            case ("KyleHitEffect"):
                obj.SetActive(false);
                Instance._kyleHitEffectQueue.Enqueue(obj);
                break;

            default:
                break;
        }
    }
}
