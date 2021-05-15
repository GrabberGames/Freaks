using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public GameObject Minion;

    public int spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MinionSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator MinionSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(Minion, this.transform.position, Quaternion.identity);
            Debug.Log("Spawned");
        }
    }
}
