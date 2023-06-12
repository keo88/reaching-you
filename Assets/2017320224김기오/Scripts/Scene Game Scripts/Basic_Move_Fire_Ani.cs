using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Move_Fire_Ani : MonoBehaviour {
    public Texture[] Images;
    Renderer rend;
    public int Ani_speed;
    public int _now_ani_time;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        int count = 0;
        if(_now_ani_time >= Ani_speed)
        {
            _now_ani_time = 0;
            if(count == 0)
            {
                rend.material.mainTexture = Images[count];
                count = 1;
            }
            else
            {
                rend.material.mainTexture = Images[count];
                count = 0;
            }
        }
        else
        {
            _now_ani_time += 1;
        }
        
	}
}
