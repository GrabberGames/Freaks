using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private List<Collider> colliders = new List<Collider>();

    private Material red, green;
    public Material material;

    private void Start()
    {
        red = GameObject.Find("BuildingController").GetComponent<BuildingController>().materials[0];
        green = GameObject.Find("BuildingController").GetComponent<BuildingController>().materials[1];
        material = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if(colliders.Count > 0)
        {
            gameObject.GetComponent<Renderer>().material = red;
            
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = green;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if(parent == null || parent.name != "Road")
        {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }


    public bool isBuildable()
    {
        return colliders.Count == 0;
    }

}
