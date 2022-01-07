using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FreaksAttack : MonoBehaviour
{
    private FreaksController freaksController;
    private GameObject near;
    bool playerInRange;
    public bool isEnemyFound = false;
    // Start is called before the first frame update
    void Start()
    {
        //freaksController = transform.parent.gameObject.transform.GetComponent<FreaksController>();
        freaksController = GetComponent<FreaksController>();
    }


    void OnTriggerEnter(Collider other)//게임오브젝트가 특정 거리 내에 들어온 경우
    {
        if (other.transform.name == "Alter" || other.transform.name == "Waron" || other.transform.name == "Kail")
        {
            isEnemyFound = true;
            freaksController.near = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)//게임오브젝트가 특정 거리 내에 벗어난 경우
    {
        if (other.transform.name == "Alter" || other.transform.name == "Waron" || other.transform.name == "Kail")
        {
            isEnemyFound = false;
            freaksController.near = null;
        }
    }
}
