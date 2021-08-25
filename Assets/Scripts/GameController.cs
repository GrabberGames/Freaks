using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;



public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    // FX
    [SerializeField] private ParticleSystem fx_Move;

    // wShot the ray to the pos. of Mouse Pointer Clicked.
    private RaycastHit hit;
    private string hitColliderName;

    private int min = 0;
    private int sec = 0;

    public int wave_min = 0;
    public int wave_sec = 0;

    private SpawnController spawnController;



    private void Start()
    {
        spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();

        StartCoroutine(PlayTimer());
        StartCoroutine(WaveTimer());

    }


    private void Update()
    {
        FXmovePlayer();
    }


    IEnumerator PlayTimer()
    {
        while(true)
        {
            timerText.text = string.Format("{0:D1}:{1:D2}", min, sec);
            
            sec++;
            
            if (sec > 59)
            {
                sec = 0;
                min++;
            }
            
            yield return new WaitForSeconds(1);
        } 
    }


    IEnumerator WaveTimer()
    {
        // 2mins Timer
        while(wave_min < 2)
        {
            wave_sec++;

            if (wave_sec > 59)
            {
                wave_sec = 0;
                wave_min++;
            }
            yield return new WaitForSeconds(1);
        }

        if (wave_min >= 2)
        {
            Debug.Log("!!");
            spawnController.StartCoroutine("FreaksSpawn");
        }
    }


    // FX Play on Mouse Click pos.
    private void FXmovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Vector3 mPos;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
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
