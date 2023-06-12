using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Move : MonoBehaviour {
    public float speed;
	void Start () {
		
	}
	
	void Update () {
        transform.Rotate(0, 0, Time.deltaTime * speed);
    }
}
