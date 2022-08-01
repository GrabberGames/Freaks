using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waron_Q : MonoBehaviour
{
    Animator animator;
    public bool b = true;
    bool c = false;
    GameObject arm;
    Vector3 _lookDir;
    Vector3 dir;
    float _damage = 0f;
    Vector3 startPos;
    bool abc = false;
    public void Init(Vector3 lookDir, float damage, GameObject arm)
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
        _lookDir = new Vector3(lookDir.x - transform.position.x, 0, lookDir.z - transform.position.z).normalized;
        _damage = damage; 
        this.arm = arm;
        startPos = transform.position;
    }

    void Update()
    {
        if(c)
        {
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5000f, LayerMask.GetMask("blackfreaks")))
        {
            if (abc)
            {
                c = true;
                animator.Play("Broke");
                GameManager.Damage.OnAttacked(_damage, hit.transform.gameObject.GetComponent<Stat>());
            }
        }

        if (b)
        {
            transform.position = arm.transform.position;
            abc = false;
        }
        else
        {
            abc = true;
            if ((startPos - this.transform.position).sqrMagnitude > 3600)
            {
                c = true;
                P();
            }
            else
                transform.position += _lookDir * 125f * Time.deltaTime;
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
