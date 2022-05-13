using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRange : MonoBehaviour, InterfaceRange
{
    public GameObject NoBuildRange;
    private void Start()
    {


   
        if (ConstructionPreviewManager.Instance.isPreviewMode)
            NoBuildRange.SetActive(true);
        else
            NoBuildRange.SetActive(false);
          


    }


    public void BuildingRangeON(bool check)
    {
        NoBuildRange.SetActive(check);
      
    }
}