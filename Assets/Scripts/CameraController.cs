using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    public float zoomSpeed = 10.0f;
    public float currentZoom;

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
        currentZoom =110.0f;
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
        currentZoom += Input.GetAxis("Mouse ScrollWheel") * -3 * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, 30f, 131f);
        mainCamera.fieldOfView = currentZoom;
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
