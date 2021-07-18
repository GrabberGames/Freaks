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

    private int me;
    private Building parentController;

    private void Start()
    {
    }

    void Update()
    {
        SetMaterial(colliders.Count == 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent != null && parent.name != "Road")
        {
            colliders.Add(other);
        }

        switch (me)
        {
            case (int)BuildingNum.Alter:
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (parent != null && parent.name == "EssenseL")
                {
                    colliders.Clear();
                }
                break;
        }
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
