using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBuilding :  Stat
{
    GameObject go;
    void Start()
    {
        base.Init();
        go = this.gameObject;
    }
    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            go.SetActive(false);
            BuildingPooling.instance.ReturnObject(go);
        }
    } 
}
