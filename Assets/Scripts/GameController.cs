using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    private List<FreaksController> _blackFreaksList = new List<FreaksController>();


    public List<FreaksController> GetAliveBlackFreaksList()
    {
        return _blackFreaksList;
    }
    public void SignOfFreaksDead()
    {
        for (int i = 0; i < _blackFreaksList.Count; i++)
        {
            if(_blackFreaksList[i].HP <= 0)
            {
                _blackFreaksList.RemoveAt(i);
            }
        }
    }
}
