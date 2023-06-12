using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour {
    public int speed = 5;
    public float acceleration = 0.1f;
    public float camera_range;
    public float CameraZoom;
    public GameObject Tracker;

    private Transform _playerTransform;

    Vector3 dir = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        _playerTransform = Tracker.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dir.magnitude < 0.001)
        {
            dir = Vector3.zero;
        }
        else if (dir.magnitude > 1)
        {
            dir = dir.normalized;
        }
        else
        {
            dir = (1.0f - acceleration) * dir;
        }
        if(transform.position.z > CameraZoom)
        {
            transform.Translate(new Vector3(0, 0, -0.08f));
        }
        Vector3 _playerPos = new Vector3(_playerTransform.transform.position.x, _playerTransform.transform.position.y,transform.position.z);
        if (_playerTransform.position.y <= 0)
        {
            _playerPos.y = 0;
        }
        if (_playerTransform.position.y >= camera_range)
        {
            _playerPos.y = camera_range;
        }
        if (_playerTransform.position.x >= camera_range)
        {
            _playerPos.x = camera_range;
        }
        if (_playerTransform.position.x <= -camera_range)
        {
            _playerPos.x = -camera_range;
        }
        dir = dir + (_playerPos - transform.position) * acceleration;
        /*if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x >= -camera_range)
        {
            dir = dir + Vector3.left * 0.5f * acceleration;

        }
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x <= camera_range)
        {
            dir = dir + Vector3.right * 0.5f * acceleration;

        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y <= camera_range)
        {
            dir = dir + Vector3.up * 0.5f * acceleration;
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y >= 0)
        {
            dir = dir + Vector3.down * 0.5f * acceleration;

        }*/
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }
}
