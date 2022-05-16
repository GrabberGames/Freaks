using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class StageManager : MonoBehaviour
{
    GameController _gameController;


    [SerializeField] private float _respawnInterval = 1.0f;

    PostProcess _postProcess;

    float _essence;
    bool _isRageMode;

    public float essence { get => _essence; }
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
        _postProcess = GetComponentInChildren<PostProcess>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _beforeSpawnTime = 0;
        StartCoroutine(PlayTimer());
    }

    // Update is called once per frame
    void Update()
    {
        FXmovePlayer();

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
        Debug.Log($"��������{_essence}");
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
    
    #region BlackFreaks ���� ����

    private void RespawnFreaks()
    {
        _beforeSpawnTime = nowPlayTime;
        tempSpawnPointNumber = Random.Range(0, GameManager.Instance.SpawnPoint.Length);

        for (int i = 0; i < GameManager.Instance.SpawnPoint.Length; i++)
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
            Debug.Log($"������ {i} max {spawnBlackfreaksCount}");
            GameObject obj = ObjectPooling.Instance.GetObject("BlackFreaks");
            obj.gameObject.GetComponent<NavMeshAgent>().Warp(GameManager.Instance.SpawnPoint[point].position);

            var script = obj.GetComponent<FreaksController>();
            script.Spawned();
            _gameController.GetAliveBlackFreaksList().Add(script);

            yield return new WaitForSeconds(_respawnInterval);
        }
    }

    #endregion

    #region RageMode ����

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

            int layerMask = (1 << LayerMask.NameToLayer("Building")) + (1 << LayerMask.NameToLayer("Walkable"));

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
}
