using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBuilding :  Stat
{
    GameObject go;
    private WhiteFreaksController ConnectingFreaks;
    private Building building;

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
            ConnectingFreaks.gameObject.SetActive(true);
            ConnectingFreaks.SetDestination(BuildingManager.Instance.Alter, false);

            building.StopBuilding();
            BuildingPooling.instance.ReturnObject(go);
        }
    }

    public void SetConnetingFreaks(WhiteFreaksController whiteFreaksController)
    {
        ConnectingFreaks = whiteFreaksController;
    }
    public void SetBuilding(Building building)
    {
        this.building = building;
    }
}
