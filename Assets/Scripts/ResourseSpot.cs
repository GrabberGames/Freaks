using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourseSpot : MonoBehaviour
{
    public int RemainResource;
    [SerializeField]
    private int GetResourcePerOnce = 50;
    private Material navy, blue, sky_blue;
    private Material material;
    private bool isDig = false;
    void Start()
    {
        material = this.gameObject.GetComponent<Renderer>().material;
        navy = GetComponentInParent<ResourceManager>().materials[0];
        blue = GetComponentInParent<ResourceManager>().materials[1];
        sky_blue = GetComponentInParent<ResourceManager>().materials[2];
    }
    // Update is called once per frame
    void Update()
    {
        SetColor();
        Digging();
    }
    void Digging()
    {
        if (isDig)
        {
            RemainResource -= GetResourcePerOnce;
            isDig = false;
        }
    }
    void SetColor()
    {
        if(RemainResource >= 801)
        {
            gameObject.GetComponent<Renderer>().material = navy;
        }
        else if(RemainResource >= 401)
        {
            gameObject.GetComponent<Renderer>().material = blue;
        }
        else if(RemainResource >= 1)
        {
            gameObject.GetComponent<Renderer>().material = sky_blue;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
