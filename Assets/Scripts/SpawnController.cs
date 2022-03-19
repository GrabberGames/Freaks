using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SpawnController
{
    [SerializeField] private int cm_num = 3; // close range

    private const int spawnFreaksNumber = 3;

    private int randomSpawn;
    private bool isRage = false;
    int spawnedFreaksNumber = 0;

    WaitForSeconds oneSecond = new WaitForSeconds(1f);

    public void FreaksSpawn()
    {
        randomSpawn = UnityEngine.Random.Range(0, 3);
        //Debug.Log(randomSpawn);
        spawnedFreaksNumber = spawnFreaksNumber;

        GameObject obj = ObjectPooling.instance.GetObject("BlackFreaks");
        obj.gameObject.GetComponent<NavMeshAgent>().Warp(GameManager.Instance.SpawnPoint[randomSpawn].position);
    }

    public void SetIsRageActivate(bool isRage)
    {
        this.isRage = isRage;
    }
}