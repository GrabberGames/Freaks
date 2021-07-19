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
    private Building parentController;

    private WorkshopController workshopController;

    private void Awake()
    {
        workshopController = GetComponentInParent<WorkshopController>();
    }

    void Update()
    {
        SetMaterial(colliders.Count == 0);

    }
    public GameObject GetBelowObject()
    {
        return belowObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent != null && parent.name != "Road")
        {
            colliders.Add(other);
        }
        if (parent.transform.parent != null && parent.transform.parent.name != "Arrow")
        {
            colliders.Add(other);
        }
        //print(other.gameObject.name);
        switch (me)
        {
            case (int)BuildingNum.Alter:
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (parent.parent != null && parent.transform.parent.name == "Arrow")
                {
                    colliders.Clear();
                }
                break;
        }
        belowObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

    public bool IsBuildable()
    {
        return colliders.Count == 0;
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
