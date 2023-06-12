using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basic_Move : MonoBehaviour {
    public int speed = 5;
    public int RotationSpeed = 5;
    public float acceleration = 0.1f;
    public float frustumHeight;
    public float frustumWidth;
    public float distance_from_edge;
    public bool isHit = false;
    public Camera mainCam;
    public Mesh[] AirPlane;
    public GameObject Twincle;

    private float distance;
    private Vector3 dir = Vector3.zero;
    private Vector3 rot = Vector3.zero;
    private Quaternion _lookRotation;
    private Touch startTouch;
    private float camWidthHalf;
    private float camHeightHalf;
    private int check_interval = 5;
    private int check_count = 0;
    private Mesh current_mesh;
    private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];

    void Start()
    {
        if(PlayerPrefs.HasKey("CurrentPlaneNum"))
        {
            GetComponent<MeshFilter>().mesh = AirPlane[PlayerPrefs.GetInt("CurrentPlaneNum")];
        }
        distance = transform.position.z + 45; //if you change camera's position, then you must change this value of 45 to something else
        frustumHeight = 2.0f * distance * Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * mainCam.aspect;
        camWidthHalf = 0.5f * frustumWidth - distance_from_edge;
        camHeightHalf = 0.5f * frustumHeight - distance_from_edge;
    }

    // Update is called once per frame
    void Update () {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouch = touch;
            }
            else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 39));
                Vector3 startPosition = Camera.main.ScreenToWorldPoint(new Vector3(startTouch.position.x, startTouch.position.y, 39));
                dir = dir + touchPosition - startPosition;
                check_count++;
                if(check_count > check_interval)
                {
                    startTouch = touch;
                    check_count = 0;
                }
            }
            
            
        }
        if (dir.magnitude < 0.01f)
        {
            dir = Vector3.zero;
        }
        else if (dir.magnitude > 100) {
            dir = dir.normalized;
        } else
        {
            dir = (1.0f - acceleration) * dir;
        }
        //_lookRotation = Quaternion.LookRotation(dir.normalized);
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x >= mainCam.transform.position.x - camWidthHalf)
        {
            dir = dir + Vector3.left * 0.5f * acceleration;
            rot = rot + Vector3.forward;
            
        }
        else if (Input.GetKey(KeyCode.RightArrow) && transform.position.x <= mainCam.transform.position.x + camWidthHalf)
        {
            dir = dir + Vector3.right * 0.5f * acceleration;
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y <= mainCam.transform.position.y + camHeightHalf)
        {
            dir = dir + Vector3.up * 0.5f * acceleration;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && transform.position.y >= mainCam.transform.position.y - camHeightHalf )
        {
            dir = dir + Vector3.down * 0.5f * acceleration;
        }
        /*if (transform.position.x <= mainCam.transform.position.x - camWidthHalf && dir.x < 0.0f)
        {
            dir.x = 0.0f;
        }
        else if (transform.position.x >= mainCam.transform.position.x + camWidthHalf && dir.x > 0.0f)
        {
            dir.x = 0.0f;
        }
        if (transform.position.y >= mainCam.transform.position.y + camHeightHalf && dir.y > 0.0f)
        {
            dir.y = 0.0f;
        }
        else if (transform.position.y <= mainCam.transform.position.y - camHeightHalf && dir.y < 0.0f)
        {
            dir.y = 0.0f;
        }*/
        transform.position = Vector3.Lerp(transform.position, transform.position + dir * speed * Time.deltaTime
             , 1.0f);
        float _angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(-_angle, Vector3.forward), Time.deltaTime * RotationSpeed);
    }
    void OnTriggerStay(Collider get)
    {

        if (get.tag == "Terrain")
        {
            dir = -dir;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
            Debug.Log("Terrain Collsion");
        }
    }
    void OnParticleCollision(GameObject get)
    {
        if(isHit == false)
        {
            isHit = true;
        }
        //Debug.Log("Particle Hit");
        int safeLength = get.GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
        if (collisionEvents.Length < safeLength)
        collisionEvents = new ParticleCollisionEvent[safeLength];

        int numCollisionEvents = get.GetComponent<ParticleSystem>().GetCollisionEvents(gameObject, collisionEvents);
        int i = 0;
        Vector3 collisionHitLoc = collisionEvents[i].intersection;
        Instantiate(Twincle, collisionHitLoc, Quaternion.identity);

    }
}
