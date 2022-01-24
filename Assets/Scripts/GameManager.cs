using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private int _deadCount = 0;
    private int _playTime = 0;
    private int _reviveTime = 0;

    public delegate void TimerEventHandler(int time);

    public event TimerEventHandler TimeChanged;

    [SerializeField]
    GameObject _player;

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

    static GameManager s_instance;

    DamageManager _damage = new DamageManager();

    public static GameManager Instance { get { Init();  return s_instance; } }

    public static DamageManager Damage { get { return Instance._damage; } }
    static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");

            if(go== null)
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
        _reviveTime = Mathf.Min(40, (_deadCount-1) * 5 + _playTime * 5);
        while(_reviveTime > 0)
        {
            _reviveTime--;

            TimeChanged(_reviveTime);

            yield return new WaitForSeconds(1f);
        }

    }
}
