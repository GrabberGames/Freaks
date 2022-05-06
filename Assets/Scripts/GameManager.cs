using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] public Transform[] SpawnPoint;

    private int _deadCount = 0;
    private int _reviveTime = 0;

    //영웅 부활 타이머 event
    public delegate void TimerEventHandler(int time);

    /// < 알터 이동 event or 싱글톤 구현해야함 >
    /// 
    /// </알터 이동 event or 싱글톤 구현해야함>

    SpawnController _spawn = new SpawnController();
    public static SpawnController Spawn { get { return Instance._spawn; } }


    [SerializeField]
    GameObject _player;

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
            if (_player == null)
                _player = value;
            else
                return;
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
        //StartCoroutine(PlayerRevive());
    }
    //--------------------------------//
    /*      UI 작업시 구독자 구현 후 주석 해제*/
    //--------------------------------//
    /*
    public IEnumerator PlayerRevive()
    {
        _reviveTime = Mathf.Min(40, (_deadCount++) * 5 + _playTime * 5);
        while(_reviveTime > 0)
        {
            _reviveTime--;

            TimeChanged(_reviveTime);

            yield return new WaitForSeconds(1f);
        }
    }
    */

    public void SetSelectHero(eHeroType value) { _selectHero = value; }
}
