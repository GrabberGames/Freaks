using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] public Transform[] SpawnPoint;

    //영웅 부활 타이머 event
    public delegate void TimerEventHandler(int time);

    /// < 알터 이동 event or 싱글톤 구현해야함 >
    /// 
    /// </알터 이동 event or 싱글톤 구현해야함>


    [SerializeField]
    GameObject _player;

    [SerializeField]
    Stat _playerStat;

    private eHeroType _selectHero = eHeroType.Kyle;
    public eHeroType selectHero { get => _selectHero; }


    public GameObject Player
    {
        get { 
            if(_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            return _player; 
        }
        set {
            _player = value;
         }
    }

    [SerializeField]
     GameObject _alter;

    public GameObject Alter
    {
        get
        {
            if(_alter == null)
            {
                _alter = GameObject.Find("alter");
               // _alter = GameObject.FindGameObjectWithTag("alter");
            }
            return _alter;
        }
        set
        {     
                _alter = value;
                AlterIsChange.Invoke(_alter);
       
                return;                   
        }
    }
    public Action<GameObject> AlterIsChange = null;

    public int _deadCount = 0;

    WaitForSeconds _oneS = new WaitForSeconds(1f);
       
    static GameManager s_instance;

    DamageManager _damage = new DamageManager();

    public static GameManager Instance { get { Init();  return s_instance; } }

    public static DamageManager Damage { get { return Instance._damage; } }

    Models _models = new Models();

    public Models models { get { return Instance._models; } }

    static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");

            if(go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();
        }
    }
    public void PlayerDead()
    {
        StartCoroutine(PlayerRevive());
    }
    public IEnumerator PlayerRevive()
    {
        float _reviveTime = Mathf.Min(40, (_deadCount++) * 5 + (StageManager.Instance.nowPlayTime % 60) * 5);
        HUDManager.Instance.ActiveReviveTimer(true);

        while (_reviveTime > 0)
        {
            _reviveTime--;

            HUDManager.Instance.SetReviveTimer(_reviveTime);

            yield return _oneS;
        }
        HUDManager.Instance.ActiveReviveTimer(false);

        if (_playerStat == null)
        {
            _playerStat = Player.GetComponent<Stat>();
        }

        _playerStat.ReviveSignal();
    }

    public void SetSelectHero(eHeroType value) 
    {
        _selectHero = value;
    }


    public void DefeatShow()
    {
        GameResult.Instance.ActiveDeUI();
    }
    public void VictoryShow()
    {
        GameResult.Instance.ActiveVicUI();
    }
}
