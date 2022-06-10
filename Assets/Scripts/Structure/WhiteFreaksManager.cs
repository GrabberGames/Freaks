using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFreaksManager : MonoBehaviour
{
    private int _allFreaksCount = 10;
    public int allFreaksCount { get => _allFreaksCount; }
    private int _idleFreaksCount = 10;
    public int idleFreaksCount { get => _idleFreaksCount; }
    private int _busyFreaksCount = 0;
    public int busyFreaksCount { get => _busyFreaksCount; }

    private List<WhiteFreaksController> _whiteFreaksList = new List<WhiteFreaksController>();


    public List<WhiteFreaksController> GetWhiteFreaksList()
    {
        return _whiteFreaksList;
    }
    public void SignOfWhiteFreaksDecrease()
    {
        for (int i = 0; i < _whiteFreaksList.Count; i++)
        {
            if (_whiteFreaksList[i].IsIdle == true)
            {
                _whiteFreaksList.RemoveAt(i);
            }
        }
    }


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
        _idleFreaksCount++;
        _busyFreaksCount--;
    }
    public void increaseBusy()
    {
            _busyFreaksCount++;
            _idleFreaksCount--;
      
    }
    /*
    public void increaseFreaks()
    {
        allFreaksCount++;
        idleFreaksCount++;
    }*/
    public void increaseFreaks(int num)
    {
        _allFreaksCount+=num;
        _idleFreaksCount+=num;
    }

    GameObject whiteFreaks;
    public GameObject GetWhiteFreaks()
    {
        if (_idleFreaksCount == 0)
        {
            SystemMassage.Instance.PrintSystemMassage("명령을 수행할 화이트프릭스가 없습니다.");
            return null;
        }
        else
        {
            increaseBusy();
            whiteFreaks =  ObjectPooling.Instance.GetObject("WhiteFreaks");
            _whiteFreaksList.Add(whiteFreaks.GetComponent<WhiteFreaksController>());
            return whiteFreaks;
        }  
    }


    public void ReturnWhiteFreaks(GameObject freaks)
    {
        SignOfWhiteFreaksDecrease();
        freaks.GetComponent<WhiteFreaksController>().IsIdle = true;
        ObjectPooling.Instance.ReturnObject(freaks);
        increaseIdle();
    }

}
