using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTimer : MonoBehaviour
{
    public bool isTimerON = false;

    [SerializeField] private int timer = 90;


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
