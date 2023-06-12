using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Behaviour : MonoBehaviour
{

    public GameObject[] BG;
    public GameObject[] ForeGround1;
    public GameObject[] ForeGround2;
    public GameObject[] ForeGround3;
    public GameObject CurFG;
    public GameObject NextFG;
    public GameObject Moon;
    public float BGStayTime;
    public float BGTransInterval;
    public float BGAlphaTransAmount;
    public int CurBG;
    public int NextBG;
    private Color prev_c;
    private Color next_c;
    private SpriteRenderer prev_rend;
    private SpriteRenderer next_rend;
    private bool _cur_state;
    private float _check_time;
    public float _check_time_for_BGStay;

    void Start()
    {
        CurFG = Instantiate(ForeGround1[0], new Vector3(33, -16, 0.1f), Quaternion.identity);
        NextFG = Instantiate(ForeGround3[0], new Vector3(33, -15, 0), Quaternion.identity);
        NextBG = (CurBG + 1) % BG.Length;
        prev_rend = BG[CurBG].GetComponent<SpriteRenderer>();
        next_rend = BG[NextBG].GetComponent<SpriteRenderer>();
        prev_c = prev_rend.color;
        next_c = next_rend.color;
        _cur_state = false;
        _check_time = 0;
        _check_time_for_BGStay = 0;
    }

    void Update()
    {
        if(Moon.transform.position.y > 0)
        {
            Moon.transform.Translate(new Vector3(0, -Time.deltaTime * 1.5f, 0));
        }
        CurFG.transform.Translate(new Vector3(-Time.deltaTime, -Time.deltaTime * 0.125f, 0));
        NextFG.transform.Translate(new Vector3(-Time.deltaTime * 0.5f, -Time.deltaTime * 0.125f, 0));
        _check_time += Time.deltaTime;
        if (_check_time > BGTransInterval)
        {
            _check_time_for_BGStay += _check_time;
            if (!_cur_state)
            {
                //_check_time_for_BGStay += _check_time;
                if (_check_time_for_BGStay > BGStayTime)
                {
                    if(CurBG != BG.Length - 1)
                    {
                        _cur_state = true;
                    }
                    
                    _check_time_for_BGStay -= BGStayTime;
                }
            }
            else
            {
                prev_c.a -= BGAlphaTransAmount;
                next_c.a += BGAlphaTransAmount;
                prev_rend.color = new Color(prev_c.r, prev_c.g, prev_c.b, prev_c.a);
                next_rend.color = new Color(next_c.r, next_c.g, next_c.b, next_c.a);
                if (prev_c.a <= 0)
                {
                    CurBG = (CurBG + 1) % BG.Length;
                    NextBG = (NextBG + 1) % BG.Length;
                    prev_rend = BG[CurBG].GetComponent<SpriteRenderer>();
                    next_rend = BG[NextBG].GetComponent<SpriteRenderer>();
                    prev_c = prev_rend.color;
                    next_c = next_rend.color;
                    _cur_state = false;
                }
            }
            _check_time -= BGTransInterval;
        }
    }
}