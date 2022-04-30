using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;



public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    // FX
    [SerializeField] private ParticleSystem fx_Move;

    private bool isRage;

    private int nowPlayTime;
    private int beforeSpawnTime;

    private int spawnIntervalTime = 120;
    private int spawnBlackfreaksCount = 3;
    private int spawnedBlackfreaksCount;

    private int tempSpawnPointNumber;

    private void Start()
    {
        beforeSpawnTime = 0;

        StartCoroutine(PlayTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
             FreaksSpawn();
    }


    IEnumerator PlayTimer()
    {
        while(true)
        {
            nowPlayTime = (int)Time.time;
            timerText.text = string.Format("{0:D1}:{1:D2}", (int)(nowPlayTime / 60), (int)(nowPlayTime % 60));

            if (nowPlayTime >= beforeSpawnTime + spawnIntervalTime)
            {
                FreaksSpawn();
            }

            yield return null;
        } 
    }

    private void FreaksSpawn()
    {
        beforeSpawnTime = nowPlayTime;
        tempSpawnPointNumber = Random.Range(0, GameManager.Instance.SpawnPoint.Length);

        if (isRage)
        {
            for (int i = 0; i < GameManager.Instance.SpawnPoint.Length; i++)
            {
                if (isRage.Equals(i.Equals(tempSpawnPointNumber)))
                    continue;

                StartCoroutine(CoFreaksSpawn(i));
            }
        }
       
    }

    IEnumerator CoFreaksSpawn(int point)
    {
        spawnedBlackfreaksCount = 0;

        while (spawnedBlackfreaksCount < spawnBlackfreaksCount)
        {
            GameObject obj = ObjectPooling.instance.GetObject("BlackFreaks");
            obj.gameObject.GetComponent<NavMeshAgent>().Warp(GameManager.Instance.SpawnPoint[point].position);

            spawnedBlackfreaksCount++;
            yield return new WaitForSeconds(1);
        }
    }
    public void SetActiveIsRage(bool value)
    {
        this.isRage = value;
    }

    // FX Play on Mouse Click pos.
    private void FXmovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Vector3 mPos;

            int layerMask = (1 << LayerMask.NameToLayer("Building")) + (1 << LayerMask.NameToLayer("Walkable"));

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity,layerMask))
            {
                mPos = hit.point; mPos.y = 0.3f;
                fx_Move.transform.position = mPos;
                fx_Move.Play(true);
            }
        }

        if (fx_Move.isStopped)
        {
            fx_Move.Stop();
        }
    }    
}
