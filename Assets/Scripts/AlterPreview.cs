using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterPreview : Building
{

    [SerializeField] Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetMaterial(bool canBuild)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        if (canBuild)
        {
            foreach(var r in renderers)
            {
                r.material = materials[0];
            }
        }
        else
        {
            foreach (var r in renderers)
            {
                r.material = materials[1];
            }
        }
    }
}
