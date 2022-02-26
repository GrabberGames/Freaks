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
    public void Init(Vector3 lookDir, float damage, GameObject arm)
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
        _lookDir = lookDir;
        _lookDir.y = -10f;
        _damage = damage; 
        this.arm = arm;
        Invoke("DT", 3f);
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
            dir = transform.position + _lookDir.normalized * 30f * Time.deltaTime;
            //dir.y = 0;
            transform.position = dir;
        }
    }
    void DT()
    {
        Destroy(this.gameObject);
    }
}
