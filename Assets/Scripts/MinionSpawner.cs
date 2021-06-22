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



public class MinionSpawner : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject[] Minion;     // Minion: close range || Minion_2: long range
    public GameObject[] SpawnPoint;

    public int spawnRate;
    public int waveTimer;
    public int cm_num;
    public int lm_num;

    private Wave wave = Wave.w0;
    private int wave_num = 0;
    private int randomSpawn;


    private void Start()
    {
        randomSpawn = Random.Range(0, SpawnPoint.Length);   // random spawn point pick
        Debug.Log("random: " + randomSpawn);
        Debug.Log("len: " + SpawnPoint.Length);
        StartCoroutine(MinionSpawn());
    }

    IEnumerator Timer() // Enemy Spawn Timer
    {
        while (true)
        {
            if (waveTimer >= 0)
            {
                timerText.text = "TIMER: " + waveTimer;
                waveTimer--;
                yield return new WaitForSeconds(1);
            }
            else
            {
                break;
            }
        }
    }


    IEnumerator MinionSpawn()
    {
        while(true)
        {

            yield return StartCoroutine(Timer());   // after times up

            //yield return new WaitForSeconds(waveTimer);

            if (wave == Wave.w0 && wave_num < System.Enum.GetValues(typeof(Wave)).Length - 1)   // Wave.w0 == break time || 1 <= wave_num <= 10; Wave.Length == 11
            {
                wave_num += 1;
                wave += wave_num;
                Debug.Log("wave: " + wave);

                while (cm_num > 0)
                {
                    Instantiate(Minion[0], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);   // close range minion spawn
                    yield return new WaitForSeconds(spawnRate);     // 1sec wait
                    Debug.Log(SpawnPoint[randomSpawn].name + " Spawned");
                    cm_num--;
                }

                while (lm_num > 0)
                {
                    Instantiate(Minion[1], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);   // long range minion spawn
                    yield return new WaitForSeconds(spawnRate);     // 1sec wait
                    Debug.Log(SpawnPoint[randomSpawn].name + " Spawned");
                    lm_num--;
                }
                wave = Wave.w0;     // break time activate (2 min)
            }
        }
    }
}
