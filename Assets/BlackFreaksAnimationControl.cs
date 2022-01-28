using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFreaksAnimationControl : MonoBehaviour
{
    FreaksController freaks;
    private void Start()
    {
        freaks = GetComponentInParent<FreaksController>();
    }
    public void CanNormalAttackChange()
    {
        Debug.Log("Called");
        if(freaks == null)
            freaks = GetComponentInParent<FreaksController>();
        freaks.CanNormalAttackChange();
    }
}
