using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
    public GameObject Fade_Mask;
    public GameObject[] UI;
    public Camera camera;

    void Awake()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);
        float targetaspect = 9.0f / 16.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;
            Rect rect = camera.rect;
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
        StartCoroutine("fadeIn");
    }

    void Start()
    {
        
    }
    void Update()
    {

    }
    public void changeSceneToGame()
    {
        StartCoroutine("fadeOutToGame");
    }
    public void quitGame()
    {
        StartCoroutine("fadeOutToQuit");
    }
    IEnumerator fadeIn()
    {
        for (float f = 1f; f >= 0; f -= 0.025f)
        {
            Color c = Fade_Mask.GetComponent<Image>().color;
            c.a = f;
            Fade_Mask.GetComponent<Image>().color = c;
            yield return null;
        }
        for(int i = 0; i < UI.Length; i++)
        {
            UI[i].SetActive(true);
        }
        yield break;
    }
    IEnumerator fadeOutToGame()
    {
        for (float f = 0f; f <= 1; f += 0.05f)
        {
            Color c = Fade_Mask.GetComponent<Image>().color;
            c.a = f;
            Fade_Mask.GetComponent<Image>().color = c;
            yield return null;
        }
        SceneManager.LoadScene("Game");
        yield break;
    }
    IEnumerator fadeOutToQuit()
    {
        for (float f = 0f; f <= 1; f += 0.05f)
        {
            Color c = Fade_Mask.GetComponent<Image>().color;
            c.a = f;
            Fade_Mask.GetComponent<Image>().color = c;
            yield return null;
        }
        Application.Quit();
        yield break;
    }
}
