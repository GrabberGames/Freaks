using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private enum BuildingNum
    {
        Alter,
        Tower,
        Workshop
    }

    private List<Collider> colliders = new List<Collider>();
    private GameObject belowObject;
    private int me;
    private bool areyouSure = false;
    private Building parentController;
    private WorkshopController workshopController;



    private void Awake()
    {
        workshopController = GetComponentInParent<WorkshopController>();
    }


    void Update()
    {
        SetMaterial(areyouSure);
    }


    public GameObject GetBelowObject()
    {
        return belowObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large")
            belowObject = other.gameObject;
        switch (me)
        {
            case (int)BuildingNum.Alter:
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (parent != null && (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large"))
                {
                    areyouSure = true;
                }
                break;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;
        switch (me)
        {
            case (int)BuildingNum.Alter:
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                areyouSure = false;
                break;
        }
    }


    public bool IsBuildable()
    {
        switch (me)
        {
            case (int)BuildingNum.Alter:
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (belowObject != null)
                {
                    return areyouSure && belowObject.transform.parent.transform.parent.name == "Arrow";
                }
                else
                    return false;

        }
        return false;
    }


    public void Init(int n)
    {
        me = n;
        switch (me)
        {
            case (int)BuildingNum.Alter:
                parentController = GetComponentInParent<AlterController>();
                break;

            case (int)BuildingNum.Tower:
                break;

            case (int)BuildingNum.Workshop:
                parentController = GetComponentInParent<WorkshopController>();
                break;
        }

        parentController.SetOpacity(true);
    }


    private void SetMaterial(bool isRed)
    {
        parentController.SetMaterial(isRed);
    }


    public void Destroy()
    {
        parentController.SetOpacity(false);
    }
}
