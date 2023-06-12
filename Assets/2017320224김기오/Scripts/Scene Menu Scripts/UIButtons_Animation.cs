using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons_Animation : MonoBehaviour {
    public GameObject[] UIButton;

    float Text_Ani_Move_Distance = 10.0f;
    float Text_Ani_Delta = 0.5f;

    void OnEnable ()
    {
        StartCoroutine("UIButtonsAnimation");
    }
	
	void Update () {
		
	}

    IEnumerator UIButtonsAnimation ()
    {
        for(int curButtonNum = 0; curButtonNum < UIButton.Length; curButtonNum++)
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
}
