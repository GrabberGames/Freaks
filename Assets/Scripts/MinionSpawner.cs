using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wave
{
    w0,
    w1, w2, w3, w4, w5,
    w6, w7, w8, w9, w10
}

public enum SpawnPoint
{
    SpawnPoint_Black_N, SpawnPoint_Black_S, SpawnPoint_Black_E
}


public class MinionSpawner : MonoBehaviour
{
    public GameObject[] Minion;     // Minion: close range || Minion_2: long range
    public int spawnRate = 1;
    public int waveTimer = 2;
    public int cm_num = 3;
    public int lm_num = 2;

    private SpawnPoint spawnPoint = 0;
    private Wave wave = Wave.w0;
    private int wave_num = 0;
    private int randomSelect;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MinionSpawn());
    }


    IEnumerator MinionSpawn()
    {
        while(true)
        {
            randomSelect = Random.Range(0, 3);  // enum SpawnPoint (0, 1, 2)
            spawnPoint += randomSelect;


            yield return new WaitForSeconds(waveTimer * 60);    // sec -> mins


            if (gameObject.name == spawnPoint.ToString())   // if, random selected point
            {
                if (wave == Wave.w0 && wave_num < System.Enum.GetValues(typeof(Wave)).Length - 1)   // Wave.w0 == break time || 1 <= wave_num <= 10; Wave.Length == 11
                {
                    wave_num += 1;
                    wave += wave_num;
                    Debug.Log("wave: " + wave);

                    while (cm_num > 0)
                    {
                        Instantiate(Minion[0], this.transform.position, Quaternion.identity);   // close range minion spawn
                        yield return new WaitForSeconds(spawnRate);     // 1sec wait
                        Debug.Log(gameObject.name + " Spawned");
                        cm_num--;
                    }

                    while (lm_num > 0)
                    {
                        Instantiate(Minion[1], this.transform.position, Quaternion.identity);   // long range minion spawn
                        yield return new WaitForSeconds(spawnRate);     // 1sec wait
                        Debug.Log(gameObject.name + " Spawned");
                        lm_num--;
                    }
                    wave = Wave.w0;     // break time activate (2 min)
                }
            }
            else
            {
                break;
            }
        }
    }
}
