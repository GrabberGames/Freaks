using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Wave
{
    w0,
    w1, w2, w3, w4, w5,
    w6, w7, w8, w9, w10
}



public class SpawnController: MonoBehaviour
{
    //public TextMeshProUGUI timerText;
    public GameObject[] Minion;     // Minion: close range || Minion_2: long range
    public GameObject[] SpawnPoint;

    public int spawnRate;
    public int waveTimer;   // min
    public int cm_num;
    public int lm_num;

    private int wave_num = 0;
    private int randomSpawn;
    private int _min;
    // private int _sec; // Comment out unused variables
    private Wave wave = Wave.w0;


    private void Start()
    {
        _min = (waveTimer / 60);
        // _sec = 0; // Comment out unused variables
        randomSpawn = Random.Range(0, SpawnPoint.Length);   // random spawn position set   
        //StartCoroutine(MinionSpawn());
    }

    /*
    IEnumerator SpawnTimer() // Enemy Spawn Timer
    {
        while (true)
        {
            if (_min >= 0)
            {
                timerText.text = string.Format("{0:D1}:{1:D2}", _min, _sec);

                if (_sec == 0)
                {
                    if (_min == 0 && _sec == 0)
                    {
                        _min = 0;
                        _sec = 0;
                        break;
                    }
                    else
                    {
                        _min -= 1;
                        _sec = 59;
                    }
                }
                _sec--;
                yield return new WaitForSeconds(1);
            }
        }
    }
    */


    IEnumerator MinionSpawn()
    {
        while (true)
        {
            //yield return StartCoroutine(SpawnTimer());
            //timer Reset
            _min = (waveTimer / 60);
            // _sec = 0; // Comment out unused variables

            if (wave == Wave.w0 && wave_num < System.Enum.GetValues(typeof(Wave)).Length - 1)   // Wave.w0 == break time || 1 <= wave_num <= 10; Wave.Length == 11
            {
                wave_num += 1;
                wave += wave_num;

                while (cm_num > 0)
                {
                    Instantiate(Minion[0], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);   // close range minion spawn
                    yield return new WaitForSeconds(spawnRate);     // 1sec wait
                    cm_num--;
                }

                while (lm_num > 0)
                {
                    Instantiate(Minion[1], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);   // long range minion spawn
                    yield return new WaitForSeconds(spawnRate);     // 1sec wait
                    lm_num--;
                }
                wave = Wave.w0;     // break time activate (2 min)
            }
        }
    }
}
