using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ListenPositioner audioListener;

    private static CameraController cInstance;
    public static CameraController Instance
    {
        get
        {
            if (cInstance == null)
            {
                cInstance = FindObjectOfType<CameraController>();
            }
            return cInstance;
        }
    }

    public GameObject player;

    public bool isFixedToHero = false;

    public float panSpeed = 100f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 50f;

    public bool _fixListenerPosition;

    private float wheelscroll;
    private float wheelspeed = 25.0f;

    [SerializeField] private Transform targetGround;
    Vector3 targetDist;
 
    Vector3 pos;
    private void Awake()
    {
        audioListener = GetComponentInChildren<ListenPositioner>();

    }

    private void Update()
    {
         pos = transform.position;
 
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Y))
        {
            if (GameManager.Instance.models.playerModel.PlayerNowHp <= 0)
                return;

            isFixedToHero = !isFixedToHero;
        }

        if (isFixedToHero)
        {
            if (player == null) 
                player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;
            transform.position = new Vector3(player.transform.position.x, 233.6f, player.transform.position.z - 100);
            return;
        }


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

      
       // pos.y = Mathf.Clamp(pos.y, minY, maxY);
       

        //¸¶¿ì½ºÈÙ·Î ÁÜÀÎ,ÁÜ¾Æ¿ô
        targetDist = pos - targetGround.position;
        targetDist = Vector3.Normalize(targetDist);
        wheelscroll = Input.GetAxis("Mouse ScrollWheel") * wheelspeed;

        if (transform.position.y <= 125.0f && wheelscroll < 0)
        {
            pos.y = 125.0f;
        } 
        else if (transform.position.y >= 230.0f && wheelscroll > 0)
        {
            pos.y = 230.0f;
        }
        else
        {
            if (transform.position.y <= 150f)
            {
                pos.x = Mathf.Clamp(pos.x, -280, 250);
                pos.z = Mathf.Clamp(pos.z, -220, 90);
            }
            else if (transform.position.y <= 180f)
            {
                pos.x = Mathf.Clamp(pos.x, -270, 235);
                pos.z = Mathf.Clamp(pos.z, -240, 70);
            }
            else if (transform.position.y <= 200)
            {
                pos.x = Mathf.Clamp(pos.x, -260, 225);           
                pos.z = Mathf.Clamp(pos.z, -250, 50);
            }
            else
            {
                pos.x = Mathf.Clamp(pos.x, -250, 215);
                pos.z = Mathf.Clamp(pos.z, -260, 20);
            }
            pos += targetDist * wheelscroll;
        }

       

        transform.position = pos;

        if(!_fixListenerPosition)
        {
            audioListener.ListenerPosition();
            _fixListenerPosition = true;
        }


       


    }
    public void ReleaseFixCamera()
    {
        isFixedToHero = false;
    }
    public void CameraMoveToPlayer()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;
        transform.position = new Vector3(player.transform.position.x, 233.6f, player.transform.position.z - 100);
    }
}