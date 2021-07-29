using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ZoomSpeed = 10.0f;
    public float Distance = 10.0f;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
    }
    void Zoom()
    {
        Distance += Input.GetAxis("Mouse ScrollWheel") * -1 * ZoomSpeed;
        Distance = Mathf.Clamp(Distance, 30f, 121f);
        mainCamera.fieldOfView = Distance;
    }
}
