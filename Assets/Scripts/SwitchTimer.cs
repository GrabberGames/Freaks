using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTimer : MonoBehaviour
{
    [SerializeField] private int timer = 90;


    public bool isTimerON = false;

    private void Update()
    {
        if (isTimerON)
        {
            StartCoroutine(Timer());
            isTimerON = false;
        }
    }





    public IEnumerator Timer()
    {
        while (true)
        {
            if (timer > 0)
            {
                timer--;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                timer = 90;
                //targetRenderer.material = switchMats[1];
                
                break;
            }
        }
    }



}
