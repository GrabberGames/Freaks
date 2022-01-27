using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelector : MonoBehaviour
{
    public GameObject[] herosInfo;


    void Start() {
        herosInfo[0].SetActive(false);
        herosInfo[1].SetActive(false);
    }


    public void HeroSelect(int indx) {
        if (indx == 0) {    // Waron
            herosInfo[0].SetActive(true);
            herosInfo[1].SetActive(false);
        }
        else if (indx == 1) {
            herosInfo[1].SetActive(true);
            herosInfo[0].SetActive(false);
        }
    }
}
