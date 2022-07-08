using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class StageManager : MonoBehaviour
{
    GameController _gameController;

    [SerializeField]
    GameObject[] PlayerObject;

    [SerializeField] private float _respawnInterval = 1.0f;

    PostProcessManager _postProcess;

    int _essence;
    bool _isRageMode;

    public int essence { get => _essence; }
    public bool isRagemode { get => _isRageMode; }
    
    // FX
    [SerializeField] private ParticleSystem fx_Move;
    
    private int _nowPlayTime;
    private int _beforeSpawnTime;
    private int _spawnIntervalTime = 120;
    public int nowPlayTime { get => _nowPlayTime; }
    public int beforeSpawnTime { get => _beforeSpawnTime; }
    public int spawnIntervalTime { get => _spawnIntervalTime; }

    private int spawnBlackfreaksCount = 3;
    //private int spawnedBlackfreaksCount;

    private int tempSpawnPointNumber;

    private float createCompleteTime;
    private float createStartTime;
    private float createTime;

    private int _createFreaksCount;
    public int createFreaksCount { get => _createFreaksCount; }

    IEnumerator coCreateWhiteFreaks;

    private int switchCount = 0;
    private int maxSwitchCount = 3;

    public Action createCompleteUIAction;

    [SerializeField] private Transform[] SpawnPoint;

    [SerializeField] private ShopUI shopUI;
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
        _gameController = FindObjectOfType<GameController>();
        _postProcess = GetComponentInChildren<PostProcessManager>();        
        var Player = PlayerObject[(int)GameManager.Instance.selectHero-1];
        GameManager.Instance.Player = Player;
        Player.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        _beforeSpawnTime = 0;
        _essence = 0;
        AddEssence(200);

        StartCoroutine(PlayTimer());
    }

    // Update is called once per frame
    void Update()
    {
        FXmovePlayer();

        if (Input.GetKeyDown(KeyCode.Z))
            shopUI.ActiveShopUI(true);

        if (Input.GetKeyDown(KeyCode.N))
            RespawnFreaks();

        if (Input.GetKeyDown(KeyCode.M))
            ActiveRageMode(!isRagemode);

        if (Input.GetKeyDown(KeyCode.P))
            AddEssence(100);
    }

    public bool ChkEssence (int value)
    {
        return _essence >= value;
    }

    public bool UseEssence(int value)
    {
        if(_essence >= value)
        {
            _essence -= value;
            HUDManager.Instance.EssenceVisualization(_essence);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddEssence(int value)
    {
        _essence += value;
        HUDManager.Instance.EssenceVisualization(_essence);
    }



    IEnumerator PlayTimer()
    {
        while (true)
        {
            _nowPlayTime = (int)Time.time;

            if (nowPlayTime >= beforeSpawnTime + spawnIntervalTime)
            {
                RespawnFreaks();
            }

            yield return null;
        }
    }
    
    #region BlackFreaks 스폰 관련

    private void RespawnFreaks()
    {
        _beforeSpawnTime = nowPlayTime;
        tempSpawnPointNumber = UnityEngine.Random.Range(0, SpawnPoint.Length);

        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            if (_isRageMode.Equals(i.Equals(tempSpawnPointNumber)))
                continue;

            StartCoroutine(CoFreaksSpawn(i));
        }
    }

    IEnumerator CoFreaksSpawn(int point)
    {
        for (int i = 0; i < spawnBlackfreaksCount; i++)
        {
            Debug.Log($"스폰중 {i} max {spawnBlackfreaksCount}");
            GameObject obj = ObjectPooling.Instance.GetObject("BlackFreaks");
            obj.gameObject.GetComponent<NavMeshAgent>().Warp(SpawnPoint[point].position);

            var script = obj.GetComponent<FreaksController>();
            script.Spawned();
            _gameController.GetAliveBlackFreaksList().Add(script);

            yield return new WaitForSeconds(_respawnInterval);
        }
    }

    #endregion

    #region WhiteFreaksCreate
    public bool ChkCreateWhiteFreaks()
    {
        return coCreateWhiteFreaks == null;
    }

    public void CreateWhiteFreaks(float createTime)
    {
        _createFreaksCount++;
        createCompleteUIAction.Invoke();

        if (coCreateWhiteFreaks == null)
        {
            coCreateWhiteFreaks = CoWhiteFreaksSpawn();

            this.createTime = createTime;
            createStartTime = Time.time;
            createCompleteTime = createStartTime + this.createTime;

            StartCoroutine(coCreateWhiteFreaks);
        }
    }


    IEnumerator CoWhiteFreaksSpawn()
    {
        while (true)
        {
            yield return null;
            if (Time.time >= createCompleteTime)
            {
                WhiteFreaksManager.Instance.increaseFreaks(1);
                _createFreaksCount--;

                createCompleteUIAction.Invoke();

                if(createFreaksCount <= 0)
                {
                    coCreateWhiteFreaks = null;
                    break;
                }
                else
                {
                    createStartTime = createCompleteTime;
                    createCompleteTime = createStartTime + this.createTime;
                }
            }
        }
    }

    public float GetCreateWhiteFreaksProgress()
    {
        if (ChkCreateWhiteFreaks())
            return 0.0f;
        else
            return (Time.time - createStartTime) / createTime;
    }

    public void SetCreateCompleteUIAction(Action action)
    {
        if(createCompleteUIAction != null)
        {
            createCompleteUIAction = null;
        }

        createCompleteUIAction = action;
    }

    #endregion

    #region RageMode 관련

    public void ActiveRageMode(bool value)
    {
        if (!isRagemode.Equals(value))
        {
            _isRageMode = value;
            _postProcess.ActiveRageMode(isRagemode);
        }
    }
    
    #endregion

    // FX Play on Mouse Click pos.
    private void FXmovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Vector3 mPos;

            int layerMask = (1 << LayerMask.NameToLayer("Building")) + (1 << LayerMask.NameToLayer("Walkable")) + (1 << LayerMask.NameToLayer("Ground"));

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {
                mPos = hit.point; mPos.y = 0.3f;
                fx_Move.transform.position = mPos;
                fx_Move.Play(true);
            }
        }

        if (fx_Move.isStopped)
        {
            fx_Move.Stop();
        }
    }
    // switch count up => 레이지 모드
    public void switchCountUp()
    {
        
        switchCount++;

        if(switchCount ==maxSwitchCount)
        {
            ActiveRageMode(true);
        }
    }

}
