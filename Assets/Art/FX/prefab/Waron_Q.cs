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
        _lookDir = new Vector3(lookDir.x - transform.position.x, 0, lookDir.z - transform.position.z).normalized;
        _damage = damage; 
        this.arm = arm;
        startPos = transform.position;
        Invoke("P", 2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks"))
        {
            animator.Play("Broke");
            GameManager.Damage.OnAttacked(_damage, other.GetComponent<Stat>());
        }
    }

    void Update()
    {
        if (b)
            transform.position = arm.transform.position;
        else
        {
            if ((startPos - this.transform.position).sqrMagnitude > 900)
                return;
            else
                transform.position += _lookDir * 50f * Time.deltaTime;
        }
    }
    void P()
    {
        animator.Play("Broke");
    }
    void DT()
    {
        Destroy(this.gameObject);
    }
}
