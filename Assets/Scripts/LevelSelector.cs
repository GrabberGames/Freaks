using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button tutorial;
    [SerializeField] private Button single;
    [SerializeField] private Button multi;





    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch(EventSystem.current.currentSelectedGameObject.name)
            {
                case "Tutorial":
                    //SceneManager.LoadScene("Tutorial");
                    break;
                case "Single":
                    SceneManager.LoadScene("Map");
                    break;
                case "Multi(LAN)":
                    //SceneManager.LoadScene("Multi");
                    break;
            }
        }
    }
}
