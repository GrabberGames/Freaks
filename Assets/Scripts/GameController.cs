using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;



public class GameController : MonoBehaviour
{
    public Image GaugeBar;
    public TextMeshProUGUI timerText;
    
    public int wave_min = 0;
    public int wave_sec = 0;

    // FX
    [SerializeField] private ParticleSystem MouseClickFX;

    // Shot the ray to the pos. of Mouse Pointer Clicked.
    private SpawnController spawnController;
    private RaycastHit hit;

    private string hitColliderName;
    private int min = 0;
    private int sec = 0;
    private float value = 0;
    private float orignalSize;
    



    private void Start()
    {
        spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();
        orignalSize = GaugeBar.rectTransform.rect.width;
        
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
            value += 0.825f;
            GaugeBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, orignalSize - value);

            if (wave_sec > 59)
            {
                wave_sec = 0;
                wave_min++;
            }
            yield return new WaitForSeconds(1);
        }

        if (wave_min >= 2)
        {
<<<<<<< Updated upstream
=======
            GaugeBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, orignalSize);
            Debug.Log("!!");
>>>>>>> Stashed changes
            spawnController.FreaksSpawn();
            wave_min = 0;
            wave_sec = 0;
            StartCoroutine(WaveTimer());
            value = 0;
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
                MouseClickFX.transform.position = mPos;
                MouseClickFX.Play(true);
            }
        }

        if (MouseClickFX.isStopped)
        {
            MouseClickFX.Stop();
        }
    }
}
