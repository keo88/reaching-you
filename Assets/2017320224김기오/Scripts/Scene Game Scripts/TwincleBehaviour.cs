using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwincleBehaviour : MonoBehaviour {
    public float TotalLifeTime;
    public float Speed;

    private float _current_time = 0;
	void Start () {
        StartCoroutine("Animation");
	}
	
	void Update () {
	}
    IEnumerator Animation()
    {
        while (transform.localScale.x > 0.05f)
        {
            transform.localScale -= 0.01f * new Vector3(Speed, Speed, 0);
            yield return new WaitForSecondsRealtime(0.02f);
        }
        Destroy(gameObject);
        yield break;
    }
}
