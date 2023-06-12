using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Circular : MonoBehaviour {
    public enum BulletState
    {
        Create,
        Move,
        Death
    }
    BulletState BS;

    public float speed;
    public Vector3 dir = Vector3.zero;
    public float frus_height_half;
    public float frus_width_half;
    public float rad;
    public float cent_pos_x;
    public float cent_pos_y;

    float speed_randomizer;
    float actual_speed;
    float min_height;
    float max_height;
    float min_width;
    float max_width;
    float check_interval;
    float cur_time = 0;
    float distance;
    float bullet_death = 2.0f;

    void Start()
    {
        BS = BulletState.Move;
        speed_randomizer = Random.Range(0.75f, 2.0f);
        actual_speed = speed * speed_randomizer * 0.00001f;
        check_interval = 100 * Time.deltaTime;
        min_height = -frus_height_half - 5;
        max_height = frus_height_half + 10;
        min_width = -frus_width_half - 5;
        max_width = frus_width_half + 5;
        rad = Mathf.PI / 180f * Vector3.Angle(new Vector3(cent_pos_x + 1, cent_pos_y, 0), new Vector3(transform.position.x - cent_pos_x, transform.position.y - cent_pos_y, 0));
        distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - cent_pos_x, 2f) + Mathf.Pow(transform.position.y - cent_pos_y, 2));
        cent_pos_x = PlayerPrefs.GetFloat("CircularCenterPosX");
        cent_pos_y = PlayerPrefs.GetFloat("CircularCenterPosY");
    }

    void Update()
    {
        if (BS == BulletState.Create)
        {

        }
        else if (BS == BulletState.Move)
        {
            degreeToRad();
            distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - cent_pos_x, 2f) + Mathf.Pow(transform.position.y - cent_pos_y, 2));
            //dir = distance * (-Mathf.Sin(rad) * Vector3.right + Mathf.Cos(rad) * Vector3.up) * 0.00001f * actual_speed;
            dir = distance * (-Mathf.Sin(rad) * Vector3.right + Mathf.Cos(rad) * Vector3.up).normalized;
            transform.Translate(dir * actual_speed);
            if (cur_time > check_interval)
            {
                cent_pos_x = PlayerPrefs.GetFloat("CircularCenterPosX");
                cent_pos_y = PlayerPrefs.GetFloat("CircularCenterPosY");
                if (transform.position.y < min_height || transform.position.y > max_height || transform.position.x < min_width || transform.position.x > max_width || distance < 1)
                {
                    BS = BulletState.Death;
                    StartCoroutine("bulletDeath");
                }
                else
                {
                    cur_time = 0;
                }
            }
            else
            {
                cur_time += Time.deltaTime;
            }
        }
        else if (BS == BulletState.Death)
        {

        }
    }
    void degreeToRad()
    {
        Vector3 _delta = new Vector3(transform.position.x - cent_pos_x, transform.position.y - cent_pos_y, 0);
        rad = Mathf.PI / 180f * Vector3.Angle(new Vector3(1, 0, 0), new Vector3(transform.position.x - cent_pos_x, transform.position.y - cent_pos_y, 0));
        if (_delta.y < 0)
        {
            rad = 2 * Mathf.PI - rad;
        }
    }
    IEnumerator bulletDeath()
    {
        yield return new WaitForSecondsRealtime(bullet_death);
        Destroy(gameObject);
    }
}
