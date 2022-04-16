using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ListenPositioner audioListener;

    public float panSpeed = 100f;
    public float panBorderThickness = 10f;

    public float minY = 100f;
    public float maxY = 300f;

    public float scrollSpeed = 50f;

    public bool _fixListenerPosition;

    private void Awake()
    {
        audioListener = GetComponentInChildren<ListenPositioner>();
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("up") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("down") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("right") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("left") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -210, 175);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -200, 20);

        transform.position = pos;

        if(!_fixListenerPosition)
        {
            audioListener.ListenerPosition();
            _fixListenerPosition = true;
        }
    }
}