using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float zoomSpeed = 10.0f;

    private Transform player; // Player object
    private Transform alter; // Alter object
    private Camera mainCamera;

    private float[] rangeY = { 142f, 348.25f };
    private float[] rangeZ = { 64.2f, 9.2f };
    private float currentY;
    private float currentZ;
    private float delta;
    private float nor;

    private Vector3 initialPosition;
    private Quaternion initialRotation;


    void Awake() 
    {
       initialPosition = transform.position;
       initialRotation = transform.rotation;
    }


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        currentY = transform.position.y;
        currentZ = transform.position.z;
        nor = (rangeY[1] - rangeY[0]) / (rangeZ[0] - rangeZ[1]);
    }


    // Update is called once per frame
    void Update()
    {
        Zoom();
        Move();

        alter = GameObject.Find("Alter").transform;
        player = GameObject.Find("Waron").transform;
        currentY = transform.position.y;
        currentZ = transform.position.z;
    }


    private void Zoom()
    {
        if ((delta = Input.GetAxis("Mouse ScrollWheel") * -5 * zoomSpeed) != 0)
        {
            currentY = Mathf.Clamp(currentY + delta * nor, rangeY[0], rangeY[1]);
            currentZ = Mathf.Clamp(currentZ - delta,       rangeZ[1], rangeZ[0]);
        }

        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, currentY, currentZ);
    }


    private void Move()
    {
        if (Input.mousePosition.x > Screen.width) 
        {
            transform.Translate(2.0f, 0, 0);
        } 

        if(Input.mousePosition.x < 0) 
        {
            transform.Translate(-2.0f, 0, 0);
        } 
        
        if(Input.mousePosition.y > Screen.height)
        {
            currentZ += 4.0f;
            rangeZ[0] += 4; rangeZ[1] += 4;
            transform.position = new Vector3(transform.position.x, transform.position.y , currentZ);
        } 

        if(Input.mousePosition.y < 10)
        {
            currentZ -= 4.0f;
            rangeZ[0] -= 4; rangeZ[1] -= 4;
            transform.position = new Vector3(transform.position.x, transform.position.y , currentZ);
        }
        
        if (Input.GetKeyDown("space"))
        {
            rangeZ[0] = player.position.z + transform.position.y / -nor + ((transform.position.y - rangeY[0]) / nor);
            rangeZ[1] = rangeZ[0] - 55;
            transform.rotation = Quaternion.Euler(new Vector3(75.0f, 0, transform.rotation.z));
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z + transform.position.y / -nor);
            currentZ = transform.position.z;
        }

        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            rangeZ[0] = alter.position.z + transform.position.y / -nor + (transform.position.y - rangeY[0]) / nor;
            rangeZ[1] = rangeZ[0] - 55;
            transform.rotation = Quaternion.Euler(new Vector3(75.0f, 0, transform.rotation.z));
            transform.position = new Vector3(alter.position.x, transform.position.y, alter.position.z + transform.position.y / -nor);
            currentZ = transform.position.z;
        }
    }
}