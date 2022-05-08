using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TT : MonoBehaviour
{
    [SerializeField] MB3_MeshBaker test;
    // Start is called before the first frame update

    [SerializeField] List<Transform> parents = new List<Transform>();

    public bool isTrigger;

    private void OnValidate()
    {
        if (isTrigger)
        {
            Debug.Log($"dqdqd {test.objsToMesh.Count}");

            for (int i = 0; i < parents.Count; i++)
            {
                var tmp = parents[i].GetComponentsInChildren<Renderer>();

                if (!tmp.Length.Equals(0))
                {
                    for (int j = 0; j < tmp.Length; j++)
                    {
                        if (tmp[j].GetComponent<MeshFilter>() != null)
                        {
                            test.objsToMesh.Add(tmp[j].gameObject);
                        }
                    }
                }

                Debug.Log($"renderer = {tmp.Length}");
            }
            isTrigger = false;
        }
    }
}
