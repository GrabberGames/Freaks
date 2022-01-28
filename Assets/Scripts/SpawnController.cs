using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class SpawnController
{
    [SerializeField] private int cm_num = 1; // close range
    
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
            obj.gameObject.GetComponent<NavMeshAgent>().Warp(GameManager.Instance.SpawnPoint[randomSpawn].position);
            if (isRage)
            {
                GameObject rageObj = ObjectPooling.instance.GetObject("BlackFreaks");
                //rageObj.transform.position = SpawnPoint[randomSpawn + 1].position;
                rageObj.gameObject.GetComponent<NavMeshAgent>().Warp(GameManager.Instance.SpawnPoint[randomSpawn].position);
            }
            cm_num--;
        }
    }

    public void SetIsRageActivate(bool isRage)
    {
        this.isRage = isRage;
    }
}