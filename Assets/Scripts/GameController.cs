using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;



public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image mask;

    // FX
    [SerializeField] private ParticleSystem fx_Move;

    // wShot the ray to the pos. of Mouse Pointer Clicked.
    private SpawnController spawnController;
    private RaycastHit hit;

    private string hitColliderName;
    private int min = 0;
    private int sec = 0;
    private float originalSize;
    private float value = 0;

    public int wave_min = 0;
    public int wave_sec = 0;






    private void Start()
    {
        spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();
        originalSize = mask.rectTransform.rect.width;

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

            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize - value);

            if (wave_sec > 59)
            {
                wave_sec = 0;
                wave_min++;
            }
            yield return new WaitForSeconds(1);
        }

        if (wave_min >= 2)
        {
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize);

            Debug.Log("!!");
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
