using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hi : MonoBehaviour
{
    [SerializeField]
    public List<Transform> listTransform = new List<Transform>();
    public bool isTrigger;
    public bool isDestroy;

    private void OnValidate()
    {
        if (isTrigger)
        {
            for(int j=0;j<listTransform.Count; j++)
            {
                var tmp = listTransform[j].GetComponentsInChildren<Collider>();

                Debug.Log($"collider = {tmp.Length}");

                var tmp2 = listTransform[j].GetComponentsInChildren<Rigidbody>();

                Debug.Log($"rigid = {tmp.Length}");

                if (isDestroy)
                {
                    if (!tmp.Length.Equals(0))
                    {
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            StartCoroutine(this.Destroy(tmp[i]));
                        }
                    }
                    if (!tmp2.Length.Equals(0))
                    {

                        for (int i = 0; i < tmp2.Length; i++)
                        {
                            StartCoroutine(this.Destroy(tmp2[i]));
                        }

                    }
                }
            }
        



            isTrigger = false;
            isDestroy = false;
        }
    }

    IEnumerator Destroy(Collider go)
    {
        yield return new WaitForSeconds(1.0f);

        DestroyImmediate(go);
    }

    IEnumerator Destroy(Rigidbody go)
    {
        yield return new WaitForSeconds(1.0f);

        DestroyImmediate(go);
    }
}
