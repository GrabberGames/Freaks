using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SpawnController: MonoBehaviour
{
    [SerializeField] private GameObject[] Freakses;     // Minion: close range || Minion_2: long range
    [SerializeField] private GameObject[] SpawnPoint;
    [SerializeField] private int cm_num; // close range
    [SerializeField] private int lm_num; // long range
    
    private int randomSpawn;
    private int wave = 0;
    private bool isRage = false;


    public void FreaksSpawn()
    {
        randomSpawn = Random.Range(0, 3);

        
        while (cm_num > 0)  // close range minion spawn
        {
            var obj = ObjectPooling.instance.GetObject("BlackFreaks");
            obj.transform.position = SpawnPoint[randomSpawn].transform.position;
            //Instantiate(Freakses[0], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);
            if (isRage)
            {
                Instantiate(Freakses[0], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);
                Instantiate(Freakses[0], SpawnPoint[(randomSpawn + 1) % 3].transform.position, Quaternion.identity);
                Instantiate(Freakses[0], SpawnPoint[(randomSpawn + 1) % 3].transform.position, Quaternion.identity);
            }
            cm_num--;
        }

        while (lm_num > 0)  // long range minion spawn
        {
            Instantiate(Freakses[1], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);
            if (isRage)
            {
                Instantiate(Freakses[1], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);
                Instantiate(Freakses[1], SpawnPoint[(randomSpawn + 1) % 3].transform.position, Quaternion.identity);
                Instantiate(Freakses[1], SpawnPoint[(randomSpawn + 1) % 3].transform.position, Quaternion.identity);
            }
            Debug.Log("?");
            lm_num--;
        }
    }

    public void SetIsRageActivate(bool isRage)
    {
        this.isRage = isRage;
    }
}