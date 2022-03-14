using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    [SerializeField] private Image hpBar;

    public void Visualization(float nowHP, float _fill)
    {
        if (_fill < 0.0f || _fill > 1.0f)
        {
            Debug.Log("input fill amount over value");
            return;
        }

        hpBar.fillAmount = _fill;
    }
}
