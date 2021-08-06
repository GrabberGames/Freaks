using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    public float zoomSpeed = 10.0f;
    public float currentZoom;

    public Transform player; // Player object

    public Transform alter; // Alter object

    public Vector3 lookOffset;    // The position of camera which view target


  
    void LateUpdate()
    { 

    }



    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        lookOffset = new Vector3(0, 25.0f, -5.0f);
        currentZoom =110.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Move();
 
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
            transform.Translate(0, 2.0f, 0);
        } 

        if(Input.mousePosition.y < 0){
            transform.Translate(0, -2.0f, 0);
        } 

        if (Input.GetKey("space"))
        {
            transform.LookAt(player);
            transform.position = player.position + lookOffset;
        }

        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            transform.LookAt(alter);
            transform.position = alter.position + lookOffset;
        }

    }
}
