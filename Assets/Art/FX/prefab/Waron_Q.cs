using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waron_Q : MonoBehaviour
{
    Animator animator;
    public bool b = true;
    GameObject arm;
    Vector3 _lookDir;
    Vector3 dir;
    float _damage = 0f;
    Vector3 startPos;
    public void Init(Vector3 lookDir, float damage, GameObject arm)
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
        _lookDir = lookDir;
        _damage = damage; 
        this.arm = arm;
        startPos = transform.position;
        //Invoke("DT", 2.6f);
    }
    private void OnTriggerEnter(Collider other)
    {
        animator.Play("Broke");
        if (other.transform.CompareTag("BlackFreaks"))
        {
            GameManager.Damage.OnAttacked(_damage, other.GetComponent<Stat>());
        }
    }

    void Update()
    {
        if (b)
            transform.position = arm.transform.position;
        else
        {
            if ((startPos - this.transform.position).magnitude > 30)
                return;
            else
                dir = transform.position + _lookDir.normalized * 50f * Time.deltaTime;
            //dir.y = 0;
            transform.position = dir;
        }
    }
    void DT()
    {
        Destroy(this.gameObject);
    }
}
