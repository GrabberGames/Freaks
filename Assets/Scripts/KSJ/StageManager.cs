using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private float _respawnInterval;
    float _beforeFreakRespawnTime;

    PostProcess _postProcess;
    float _startTime;

    float _nowTime;
    float _essence;
    bool _isRageMode;

    public float nowTime { get => _nowTime; }
    public float essence { get => _essence; }
    public bool isRagemode { get => _isRageMode; }


    private static StageManager mInstance;
    public static StageManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<StageManager>();
            }
            return mInstance;
        }
    }

    private void Awake()
    {
        _postProcess = GetComponentInChildren<PostProcess>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _nowTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _nowTime = _nowTime + Time.deltaTime;
        //Debug.Log(_nowTime);

        if(_nowTime >= (_beforeFreakRespawnTime + _respawnInterval))
        {
            //프릭스 리스폰 요청
        }


        if (Input.GetKeyDown(KeyCode.M))
        {
            ActiveRageMode(!isRagemode);
        }
    }

    public bool ChkEssence (float value)
    {
        return _essence >= value;
    }

    public bool UseEssence(float value)
    {
        if(_essence >= value)
        {
            _essence -= value;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RespawnFreaks()
    {
        //프릭스 리스폰 요청
        _beforeFreakRespawnTime = nowTime;        
    }

    public void AddEssence(int value)
    {
        _essence += value;
    }

    public void ActiveRageMode(bool value)
    { 
        if(!isRagemode.Equals(value))
        {
            _isRageMode = value;
            _postProcess.ActiveRageMode(isRagemode);
        }
    }
}
