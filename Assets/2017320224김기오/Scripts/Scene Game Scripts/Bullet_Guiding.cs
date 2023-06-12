using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Guiding : MonoBehaviour {
    public enum BulletState
    {
        Create,
        Move,
        Death
    }
    BulletState BS;

    public GameObject Player;
    public float ElaspedTime;
    public float speed;
    public Vector3 dir = Vector3.zero;
    public int Maneuverence;
    public float frus_height_half;
    public float frus_width_half;

    Transform _player_transform;
    float speed_randomizer;
    float actual_speed;
    float min_height;
    float max_height;
    float min_width;
    float max_width;
    float check_interval;
    float cur_time = 0;
    float bullet_death = 2.0f;

    // Use this for initialization
    void Start()
    {
        _player_transform = Player.GetComponent<Transform>();
        BS = BulletState.Move;
        speed_randomizer = Random.Range(0.75f, 2.0f);
        actual_speed = speed * speed_randomizer;
        check_interval = 100 * Time.deltaTime;
        min_height = -frus_height_half - 5;
        max_height = frus_height_half + 10;
        min_width = -frus_width_half - 5;
        max_width = frus_width_half + 5;
        ElaspedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (BS == BulletState.Create)
        {

        }
        else if (BS == BulletState.Move)
        {
            Vector3 _player_location = _player_transform.position;
            dir = dir * Maneuverence + (_player_location - transform.position).normalized;
            actual_speed += Time.deltaTime * 1f;
            transform.Translate(dir.normalized * 0.01f * actual_speed * Time.deltaTime);
            if (cur_time > check_interval)
            {
                if (transform.position.y < min_height || transform.position.y > max_height || transform.position.x < min_width || transform.position.x > max_width || ElaspedTime > 5.0f)
                {
                    BS = BulletState.Death;
                    StartCoroutine("bulletDeath");
                }
                else
                {
                    ElaspedTime += cur_time;
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
    IEnumerator bulletDeath()
    {
        yield return new WaitForSecondsRealtime(bullet_death);
        Destroy(gameObject);
    }
}
