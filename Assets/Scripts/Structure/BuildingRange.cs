using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRange : MonoBehaviour, InterfaceRange
{
    private GameObject NoBuildRange;
    private void Start()
    {
        NoBuildRange = transform.GetChild(0).gameObject;
        NoBuildRange.SetActive(false);
    }

    public void BuildingRangeON(bool check)
    {
        NoBuildRange.SetActive(check);
      
    }
}