using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
    public float TotalLifeTime;
    public float Speed;
    private float _rotation_angle = 30;
    private float _rotation_acceleration = 30;

    private float _current_time = 0;
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine("Animation");
    }

    void Update()
    {
    }
    IEnumerator Animation()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        while (transform.localScale.x < 3.0f)
        {
            //transform.Rotate(0, 0, Time.deltaTime * Speed * 300);
            transform.localScale += 0.4f * new Vector3(Speed, Speed, 0);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        while (transform.localScale.x > 0.05f)
        {
            transform.Rotate(0, 0, Time.deltaTime * Speed * (_rotation_angle += _rotation_acceleration));
            transform.localScale -= 0.05f * new Vector3(Speed, Speed, 0);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        Destroy(gameObject);
        yield break;
    }
}
