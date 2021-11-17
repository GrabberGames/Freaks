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
        freaksController = transform.parent.gameObject.transform.GetComponent<FreaksController>();
    }

    // Update is called once per frame
    void Update()
    {
        //가까이 있다면 이동
        if (playerInRange)
        {
            freaksController.move(near);
        }
        else
        {
            isEnemyFound = false;
        }
    }
    void OnTriggerEnter(Collider other)//게임오브젝트가 특정 거리 내에 들어온 경우
    {
        playerInRange = true;
        near = other.gameObject;
    }
    void OnTriggerExit(Collider other)//게임오브젝트가 특정 거리 내에 벗어난 경우
    {
        playerInRange = false;
        near = null;
    }
}
