using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenPositioner : MonoBehaviour
{
    private Vector3 screenCenter;

    Ray ray;
    RaycastHit hit;

    public void ListenerPosition()
    {
        screenCenter = new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f, 0.0f);

        ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit, 5000.0f, (int)eLayerMask.Ground))
        {
            transform.position = hit.point;
        }
    }

    
}
