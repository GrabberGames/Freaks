using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SpawnController: MonoBehaviour
{
    [SerializeField] private Transform[] SpawnPoint;
    [SerializeField] private int cm_num; // close range
    [SerializeField] private int lm_num; // long range
    
    private int randomSpawn;
    private int wave = 0;
    private bool isRage = false;

    public void FreaksSpawn()
    {
        randomSpawn = Random.Range(0, 3);
        //Debug.Log(randomSpawn);

        while (cm_num > 0)  // close range minion spawn
        {
            GameObject obj = ObjectPooling.instance.GetObject("BlackFreaks");
            obj.transform.position = SpawnPoint[randomSpawn].position;
            //obj.transform.position = new Vector3(-23.1882153f, 0.0966264457f, -6.99029636f);
            //Vector3(-31.6000004,231.300003,101.800003)
            // ³²ÂÊ Vector3(-59.5,0.100000001,-88.6999969) // Vector3(-17,0.0585823059,-177.800003)
            //
            if (isRage)
            {
                GameObject rageObj = ObjectPooling.instance.GetObject("BlackFreaks");
                rageObj.transform.position = SpawnPoint[randomSpawn + 1].position;
            }
            cm_num--;
        }

        while (lm_num > 0)  // long range minion spawn
        {
            var obj = ObjectPooling.instance.GetObject("WhiteFreaks");
            obj.transform.position = SpawnPoint[randomSpawn].position;
            if (isRage)
            {
                var rageObj = ObjectPooling.instance.GetObject("WhiteFreaks");
                rageObj.transform.position = SpawnPoint[randomSpawn + 1].position;
            }
            lm_num--;
        }
    }

    public void SetIsRageActivate(bool isRage)
    {
        this.isRage = isRage;
    }
}