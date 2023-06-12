using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreAnimation : MonoBehaviour {
    public GameObject[] UIScore;
    public int Text_Ani_Move_Distance_for_Score;

    private float Text_Ani_Move_Distance = 10.0f;
    private float Text_Ani_Delta = 0.5f;
    private float Text_Ani_Delta_for_Score = 5.0f;

    void Start () {
		
	}
	void Update () {
	}
    void OnEnable()
    {
        StartCoroutine("_UIScoreAnimation");
    }
    IEnumerator _UIScoreAnimation()
    {
        for(int curButtonNum = 0; curButtonNum < 1; curButtonNum++)
        {
            RectTransform rect = UIScore[0].GetComponent<RectTransform>();
            Vector2 curPos = rect.anchoredPosition;
            Vector2 originPos = new Vector2(curPos.x, curPos.y - Text_Ani_Move_Distance_for_Score);
            float cnt = Text_Ani_Move_Distance_for_Score / Text_Ani_Delta_for_Score;
            for (float i = Text_Ani_Move_Distance_for_Score; i >= 0; i -= Text_Ani_Delta_for_Score)
            {
                curPos = originPos + i * Vector2.up;
                UIScore[0].GetComponent<RectTransform>().anchoredPosition = curPos;
                yield return null;
            }
        }
        
        for (int curButtonNum = 1; curButtonNum < UIScore.Length; curButtonNum++)
        {
            RectTransform rect = UIScore[curButtonNum].GetComponent<RectTransform>();
            Vector2 curPos = rect.anchoredPosition;
            Vector2 originPos = new Vector2(curPos.x, curPos.y);
            Text text = UIScore[curButtonNum].GetComponentInChildren<Text>();
            float j = 0.0f;
            float cnt = Text_Ani_Move_Distance / Text_Ani_Delta;
            float factor = 1 / cnt;
            for (float i = Text_Ani_Move_Distance; i >= 0; i -= Text_Ani_Delta)
            {
                curPos = originPos + i * Vector2.up;
                UIScore[curButtonNum].GetComponent<RectTransform>().anchoredPosition = curPos;
                UIScore[curButtonNum].GetComponentInChildren<Text>().color = new Color(text.color.r, text.color.g, text.color.b, j);
                j += factor;
                yield return null;
            }

        }
        yield break;
    }
}
