using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class IntroButtonController : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button setting;
    [SerializeField] private Button credit;
    [SerializeField] private Button quit;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject.name != null)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "Play":
                    SceneManager.LoadScene("LevelSelect");
                    break;
                case "Setting":
                    //SceneManager.LoadScene("Setting");
                    Debug.Log("Setting");
                    break;
                case "Credit":
                    Debug.Log("Credit");
                    //SceneManager.LoadScene("Credit");
                    break;
                case "Quit":
                    Debug.Log("Quit");
                    //SceneManager.LoadScene("Quit");
                    break;

            }
        }
    }
}
