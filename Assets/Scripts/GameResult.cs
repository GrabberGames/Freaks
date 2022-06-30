using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour
{

    [Header("Victory UI")]
    [SerializeField] private GameObject VicUI;
    [SerializeField] private Text VicTime;

    [Header("Defeat UI")]
    [SerializeField] private GameObject DeUI;
    [SerializeField] private Text DeTime;

    private static GameResult rInstance;
    public static GameResult Instance
    {
        get
        {
            if (rInstance == null)
            {
                rInstance = FindObjectOfType<GameResult>();
            }
            return rInstance;
        }
    }

    private void Start()
    {
        VicUI.SetActive(false);
        DeUI.SetActive(false);
    }


    public void ActiveVicUI()
    {
       // Time.timeScale = 0;
        VicTime.text = string.Format("{0:D2}:{1:D2}", (int)(StageManager.Instance.nowPlayTime / 60), (int)(StageManager.Instance.nowPlayTime % 60));
        VicUI.SetActive(true);
    }

    public void ActiveDeUI()
    {
       // Time.timeScale = 0;
        DeTime.text = string.Format("{0:D2}:{1:D2}", (int)(StageManager.Instance.nowPlayTime / 60), (int)(StageManager.Instance.nowPlayTime % 60));
        DeUI.SetActive(true);
    }

    public void GameToHome()
    {
        Debug.Log("버튼 눌림");
        SceneManager.LoadScene("Intro");
        // .LoadScene("Intro");
    }

}
