using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingUIManager : MonoBehaviour {
    public GameObject CurrentAirplane;
    public Mesh[] Airplane;
    public GameObject[] UIButton;

    Mesh _cur_mesh;
    int CurrentViewPlaneNumber;
    float Text_Ani_Move_Distance = 10.0f;
    float Text_Ani_Delta = 0.5f;

    void Start()
    {
        CurrentViewPlaneNumber = 0;
    }

    void OnEnable()
    {
        StartCoroutine("UIButtonsAnimation");
    }

    void Update()
    {

    }

    IEnumerator UIButtonsAnimation()
    {
        for (int curButtonNum = 0; curButtonNum < UIButton.Length; curButtonNum++)
        {
            RectTransform rect = UIButton[curButtonNum].GetComponent<RectTransform>();
            Vector2 curPos = rect.anchoredPosition;
            Vector2 originPos = new Vector2(curPos.x, curPos.y);
            Text text = UIButton[curButtonNum].GetComponentInChildren<Text>();
            float j = 0.0f;
            float cnt = Text_Ani_Move_Distance / Text_Ani_Delta;
            float factor = 1 / cnt;
            for (float i = Text_Ani_Move_Distance; i >= 0; i -= Text_Ani_Delta)
            {
                curPos = originPos + i * Vector2.up;
                UIButton[curButtonNum].GetComponent<RectTransform>().anchoredPosition = curPos;
                UIButton[curButtonNum].GetComponentInChildren<Text>().color = new Color(text.color.r, text.color.g, text.color.b, j);
                j += factor;
                yield return null;
            }

        }
        yield break;
    }
    public void onClickNext()
    {
        if(CurrentViewPlaneNumber < Airplane.Length - 1)
        {
            CurrentViewPlaneNumber += 1;
        }
        else
        {
            CurrentViewPlaneNumber = 0;
        }
        _cur_mesh = Airplane[CurrentViewPlaneNumber];
        CurrentAirplane.GetComponent<MeshFilter>().mesh = _cur_mesh;
    }
    public void onClickPrev()
    {
        if(CurrentViewPlaneNumber > 0)
        {
            CurrentViewPlaneNumber -= 1;
        }
        else
        {
            CurrentViewPlaneNumber = 6;
        }
        _cur_mesh = Airplane[CurrentViewPlaneNumber];
        CurrentAirplane.GetComponent<MeshFilter>().mesh = _cur_mesh;
    }
    public void onClickSelect()
    {
        PlayerPrefs.SetInt("CurrentPlaneNum", CurrentViewPlaneNumber);
        Debug.Log(PlayerPrefs.GetInt(("CurrentPlaneNum")));
    }
}

