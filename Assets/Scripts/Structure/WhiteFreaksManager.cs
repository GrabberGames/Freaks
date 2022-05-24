using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFreaksManager : MonoBehaviour
{
    private int allFreaksCount = 10;
    private int idleFreaksCount = 10;
    private int busyFreaksCount = 0;


    private static WhiteFreaksManager mInstance;
    public static WhiteFreaksManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<WhiteFreaksManager>();
            }
            return mInstance;
        }
    }

    public void increaseIdle()
    {
        idleFreaksCount++;
        busyFreaksCount--;
    }
    public void increaseBusy()
    {
            busyFreaksCount++;
            idleFreaksCount--;
      
    }
    public void increaseFreaks()
    {
        allFreaksCount++;
        idleFreaksCount++;
    }

    GameObject whiteFreaks;
    public GameObject GetWhiteFreaks()
    {
        if (idleFreaksCount == 0)
        {
            SystemMassage.Instance.PrintSystemMassage("명령을 수행할 화이트프릭스가 없습니다.");
            return null;
        }
        else
        {
            increaseBusy();
            whiteFreaks =  ObjectPooling.Instance.GetObject("WhiteFreaks");
            return whiteFreaks;
        }  
    }


    public void ReturnWhiteFreaks(GameObject freaks)
    {
        ObjectPooling.Instance.ReturnObject(freaks);
        increaseIdle();
    }

}
