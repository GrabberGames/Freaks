using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void HeroSelect() 
    {
        // IF Single Btn is Clicked
        SceneManager.LoadScene(3);
    }


    public void BackToMenu() {
        // IF Back To Menu Btn is Clicked
        SceneManager.LoadScene(1);
    }
}
