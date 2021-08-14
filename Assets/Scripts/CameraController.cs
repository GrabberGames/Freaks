using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    public float zoomSpeed = 10.0f;
    private float[] rangeY = { 142f, 348.25f };
    private float[] rangeZ = { 9.2f, 64.2f };
    public float currentY;
    public float currentZ;

    private float delta;
    private float nor;
    private Transform player; // Player object

    private Transform alter; // Alter object

    // private Vector3 lookOffset;    // The position of camera which view target
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Awake() {
       initialPosition = transform.position;
       initialRotation = transform.rotation ;
    }


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        currentY = transform.position.y;
        currentZ = transform.position.z;
        nor = (rangeY[1] - rangeY[0]) / (rangeZ[1] - rangeZ[0]);
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Move();
        alter = GameObject.Find("Alter").transform;
        player = GameObject.Find("Waron").transform;
    }
    void Zoom()
    {
        delta += Input.GetAxis("Mouse ScrollWheel") * -5 * zoomSpeed;
        currentY = Mathf.Clamp(currentY + delta * nor, rangeY[0], rangeY[1]);
        currentZ = Mathf.Clamp(currentZ + delta,       rangeZ[0], rangeZ[1]);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, currentY, currentZ);
    }

    private void Move()
    {

        if(Input.mousePosition.x > Screen.width){
            transform.Translate(2.0f, 0, 0);
        } 
       
        if(Input.mousePosition.x < 0){
            transform.Translate(-2.0f, 0, 0);
        } 
        
        if(Input.mousePosition.y > Screen.height){
            transform.Translate(0, 0, 4.0f);
            transform.position = new Vector3(transform.position.x, initialPosition.y , transform.position.z);
        } 

        if(Input.mousePosition.y < 10){
            transform.Translate(0, 0, -4.0f);
            transform.position = new Vector3(transform.position.x, initialPosition.y , transform.position.z);
        } 

        if (Input.GetKey("space"))
        {
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(new Vector3(75.0f, -180.0f , transform.rotation.z));
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        }

        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            transform.LookAt(alter);
            transform.rotation = Quaternion.Euler(new Vector3(75.0f, -180.0f , transform.rotation.z));
            transform.position = new Vector3(alter.position.x, transform.position.y, alter.position.z);
        }

    }
}
