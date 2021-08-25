using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SpawnController: MonoBehaviour
{
    public enum Wave
    {
        w0,
        w1, w2, w3, w4, w5,
        w6, w7, w8, w9, w10
    }


    //public TextMeshProUGUI timerText;
    [SerializeField] private GameObject[] Freakses;     // Minion: close range || Minion_2: long range
    [SerializeField] private GameObject[] SpawnPoint;
    [SerializeField] private int cm_num; // close range
    [SerializeField] private int lm_num; // long range
    
    private int wave_num = 0;
    private int randomSpawn;

    public Wave wave = Wave.w0;


    public IEnumerator FreaksSpawn()
    {
        Debug.Log("!!!!");
        //return null;

        
        while (true)
        {
            if (wave == Wave.w0 && wave_num < System.Enum.GetValues(typeof(Wave)).Length - 1)   // Wave.w0 == break time || 1 <= wave_num <= 10; Wave.Length == 11
            {
                //randomSpawn = Random.Range(0, SpawnPoint.Length);   // random spawn position set  
                Debug.Log("&");
                wave_num += 1;
                wave += wave_num;

                /*
                while (cm_num > 0)
                {
                    Instantiate(Freakses[0], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);   // close range minion spawn
                    Debug.Log("!");
                    yield return new WaitForSeconds(1);     // 1sec wait
                    cm_num--;
                }

                while (lm_num > 0)
                {
                    Instantiate(Freakses[1], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);   // long range minion spawn
                    Debug.Log("?");
                    yield return new WaitForSeconds(1);     // 1sec wait
                    lm_num--;
                }
                */
                //wave = Wave.w0;     // break time activate (2 min)
            }
        }
        
    }
}