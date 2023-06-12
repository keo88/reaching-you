using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BulletType
{
    Normal,
    Percision,
    Circular,
    Guiding
}

public class Game_Mananger : MonoBehaviour {
    public GameObject player;
    public GameObject original_bullet;
    public GameObject circular_bullet;
    public GameObject guiding_bullet;
    public GameObject Fade_Mask;
    public GameObject StarEffect;
    public GameObject[] UIPopUpAtPlayerDeath;
    public GameObject UICurrentScore;
    public GameObject UIHighscore;
    public Camera camera;
    public BulletType BS = BulletType.Percision;
    GameObject new_bullet;
    public GameObject BGM;
    public GameObject DeathSound;
    public float bullet_interval;
    public float stage_interval;
    public float range;
    public float check_time_for_interval = 0;
    public float check_time_for_stage = 0;
    public float elasped_time = 0;
    public float bullet_speed;
    public float bullet_spread;
    public float bullet_safe_range;
    public int current_score;
    public bool CircularBulletChangePosActive = true;
    public bool PlayerInvincible;
    // Bullet freq/ Percision freq/ Circular freq/ Guiding freq / Normal freq 
    float[,] stage_const  = { { 10.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f } , { 0.3f, 0.05f, 0.9f, 0.95f, 1.0f, 1.0f}, { 0.25f, 0.2f, 0.2f, 0.25f, 1.0f, 1.0f }, { 0.1f, 0.0f, 1.0f, 1.0f,1.0f, 1.0f } };
    private int _stage_num = 3; // manually change (actual stage number - 1)

    private bool _fadeOut_has_been_executed;
    private int cur_stage;
    private GameObject bgm;

    void Awake ()
    {
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
        StartCoroutine("circularBulletPosChange");

        _fadeOut_has_been_executed = false;
        cur_stage = 1;
        bgm = Instantiate(BGM);
}

	void Start ()
    {
        
    }
	
	void Update () {
        elasped_time += Time.deltaTime;
        if (player.GetComponent<Basic_Move>().isHit == true && !PlayerInvincible)
        {
            cur_stage = 0;
            
            if (!_fadeOut_has_been_executed)
            {
                Destroy(bgm);
                GameObject deathsound = Instantiate(DeathSound);
                Destroy(deathsound, DeathSound.GetComponent<AudioSource>().clip.length);
                _fadeOut_has_been_executed = true;
                StartCoroutine("fadeOut");
            }
        }
        else
        {
            current_score = Mathf.RoundToInt(elasped_time);
            UICurrentScore.GetComponent<Text>().text = current_score.ToString();
            if (check_time_for_interval <= stage_const[cur_stage, 0])
            {
                check_time_for_interval += Time.deltaTime;
            }
            else
            {
                float determiner = Random.Range(0.0f, 1.0f);
                if (determiner < stage_const[cur_stage, 1])
                {
                    BS = BulletType.Percision;
                    bulletCreatePercision();
                }
                else if (determiner < stage_const[cur_stage, 2])
                {
                    BS = BulletType.Circular;
                    for (int i = 0; i < 5; i++)
                    {
                        bulletCreateCircular(0, -20);
                    }
                }
                else if (determiner < stage_const[cur_stage, 3])
                {
                    BS = BulletType.Guiding;
                    for (int i = 0; i < 5; i++)
                    {
                        bulletCreateGuiding();
                    }
                }
                else
                {
                    BS = BulletType.Normal;
                    for (int i = 0; i < 1; i++)
                    {
                        float rand_dir_const = Random.Range(-0.5f, 0.5f);
                        Vector3 dir = (Vector3.left * rand_dir_const + Vector3.down).normalized;
                        bulletCreateNormal(dir);
                    }
                }
                check_time_for_stage += check_time_for_interval;
                check_time_for_interval = 0;
            }
            if (check_time_for_stage > stage_interval)
            {
                check_time_for_stage = 0;
                if (_stage_num > cur_stage && elasped_time < 80)
                {
                    cur_stage += 1;
                }
                else if(elasped_time < 80)
                {
                    cur_stage = 1;
                }
                else
                {
                    cur_stage = 0;
                }
            }
        }
	}
    void bulletCreateNormal()
    {
        Vector3 player_pos
            = Vector3.right * player.GetComponent<Transform>().position.x
            + Vector3.up * player.GetComponent<Transform>().position.y
            + Vector3.forward * player.GetComponent<Transform>().position.z;
        float rand_range_x = Random.Range(-1.0f, 1.0f);
        float rand_range_y = Random.Range(-1.0f, 1.0f);
        float rand_dir_const = Random.Range(-0.5f, 0.5f);
        Vector3 dir = (Vector3.left * rand_dir_const + Vector3.down).normalized;
        Vector3 bullet_pos = new Vector3(rand_range_x * range, rand_range_y * range + 10, -6);
        if ((bullet_pos - player_pos).magnitude < bullet_safe_range)
        {
            bulletCreateNormal();
        }
        else
        {
            new_bullet = Instantiate(original_bullet, bullet_pos, Quaternion.identity);
            new_bullet.GetComponent<Bullet_Move>().speed = bullet_speed;
            new_bullet.GetComponent<Bullet_Move>().dir = dir;
            new_bullet.GetComponent<Bullet_Move>().frus_height_half = player.GetComponent<Basic_Move>().frustumHeight / 2 + 5;
            new_bullet.GetComponent<Bullet_Move>().frus_width_half = player.GetComponent<Basic_Move>().frustumWidth / 2 + 5;
        }
    }
    void bulletCreateNormal(Vector3 dir)
    {
        Vector3 player_pos
            = Vector3.right * player.GetComponent<Transform>().position.x
            + Vector3.up * player.GetComponent<Transform>().position.y
            + Vector3.forward * player.GetComponent<Transform>().position.z;
        float rand_range_x = Random.Range(-1.0f, 1.0f);
        float rand_range_y = Random.Range(-1.0f, 1.0f);
        Vector3 bullet_pos = new Vector3(rand_range_x * range, rand_range_y * range + 10, -6);
        if ((bullet_pos - player_pos).magnitude < bullet_safe_range)
        {
            bulletCreateNormal(dir);
        }
        else
        {
            for (int i = Random.Range(1, 5); i > 0; i--)
            {
                bullet_pos = bullet_pos + Vector3.right * Random.Range(-3.0f, 3.0f) + Vector3.up * Random.Range(-3.0f, 3.0f);
                new_bullet = Instantiate(original_bullet, bullet_pos, Quaternion.identity);
                new_bullet.GetComponent<Bullet_Move>().speed = bullet_speed;
                new_bullet.GetComponent<Bullet_Move>().dir = dir;
                new_bullet.GetComponent<Bullet_Move>().frus_height_half = player.GetComponent<Basic_Move>().frustumHeight / 2 + 5;
                new_bullet.GetComponent<Bullet_Move>().frus_width_half = player.GetComponent<Basic_Move>().frustumWidth / 2 + 5;
            }
        }
    }
    void bulletCreatePercision ()
    {
        Vector3 player_pos 
            = Vector3.right * player.GetComponent<Transform>().position.x
            + Vector3.up * player.GetComponent<Transform>().position.y
            + Vector3.forward * player.GetComponent<Transform>().position.z;
        for (int  i = 0; i < 1; i ++)
        {
            float rand_range_x = Random.Range(-1.0f, 1.0f);
            float rand_range_y = Random.Range(-1.0f, 1.0f);
            Vector3 bullet_pos = new Vector3(rand_range_x * range, rand_range_y * range + 10, -6);
            Vector3 dir = (player_pos - bullet_pos).normalized;
            if ((bullet_pos - player_pos).magnitude < bullet_safe_range || bullet_pos.y < player.transform.position.y)
            {
                bulletCreatePercision();
            }
            else
            {
                new_bullet = Instantiate(original_bullet, bullet_pos, Quaternion.identity);
                new_bullet.GetComponent<Bullet_Move>().speed = bullet_speed;
                new_bullet.GetComponent<Bullet_Move>().dir = dir;
                new_bullet.GetComponent<Bullet_Move>().frus_height_half = player.GetComponent<Basic_Move>().frustumHeight / 2;
                new_bullet.GetComponent<Bullet_Move>().frus_width_half = player.GetComponent<Basic_Move>().frustumWidth / 2;
            }
        }
    }
    void bulletCreateGuiding()
    {
        Vector3 player_pos
            = Vector3.right * player.GetComponent<Transform>().position.x
            + Vector3.up * player.GetComponent<Transform>().position.y
            + Vector3.forward * player.GetComponent<Transform>().position.z;
        for (int i = 0; i < 1; i++)
        {
            float rand_range_x = Random.Range(-1.0f, 1.0f);
            float rand_range_y = Random.Range(-1.0f, 1.0f);
            Vector3 bullet_pos = new Vector3(rand_range_x * range, rand_range_y * range + 10, -6);
            Vector3 dir = (player_pos - bullet_pos).normalized;
            if ((bullet_pos - player_pos).magnitude < bullet_safe_range || bullet_pos.y < player.transform.position.y)
            {
                bulletCreateGuiding();
            }
            else
            {
                new_bullet = Instantiate(guiding_bullet, bullet_pos, Quaternion.identity);
                new_bullet.GetComponent<Bullet_Guiding>().Player = player;
                new_bullet.GetComponent<Bullet_Guiding>().speed = bullet_speed;
                new_bullet.GetComponent<Bullet_Guiding>().dir = dir;
                new_bullet.GetComponent<Bullet_Guiding>().frus_height_half = player.GetComponent<Basic_Move>().frustumHeight / 2;
                new_bullet.GetComponent<Bullet_Guiding>().frus_width_half = player.GetComponent<Basic_Move>().frustumWidth / 2;
            }
        }
    }

    void bulletCreateCircular(float x, float y)
    {
        Vector3 player_pos
            = Vector3.right * player.GetComponent<Transform>().position.x
            + Vector3.up * player.GetComponent<Transform>().position.y
            + Vector3.forward * player.GetComponent<Transform>().position.z;
        float rand_range_x = Random.Range(-1.0f, 1.0f);
        float rand_range_y = Random.Range(-1.0f, 1.0f);
        //float rand_range_z = Random.Range(-0.5f, 0.5f);
        Vector3 bullet_pos = new Vector3(rand_range_x * range, rand_range_y * range + 10, -6);
        if ((bullet_pos - player_pos).magnitude < bullet_safe_range)
        {
            bulletCreateCircular(x, y);
        }
        else
        {
            new_bullet = Instantiate(circular_bullet, bullet_pos, Quaternion.identity);
            new_bullet.GetComponent<Bullet_Circular>().speed = bullet_speed;
            new_bullet.GetComponent<Bullet_Circular>().cent_pos_x = x;
            new_bullet.GetComponent<Bullet_Circular>().cent_pos_y = y;
            new_bullet.GetComponent<Bullet_Circular>().frus_height_half = player.GetComponent<Basic_Move>().frustumHeight / 2 + 5;
            new_bullet.GetComponent<Bullet_Circular>().frus_width_half = player.GetComponent<Basic_Move>().frustumWidth / 2 + 5;
        }
    }
    public void changeSceneToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void replayGame()
    {
        SceneManager.LoadScene("Game");
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
        yield break;
    }
    IEnumerator fadeOut()
    {
        Time.timeScale = 0.0f;
        if (PlayerPrefs.HasKey("highscore"))
        {
            if (PlayerPrefs.GetInt("highscore") < (int)current_score)
            {
                PlayerPrefs.SetInt("highscore", (int)current_score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("highscore", (int)current_score);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        for (float f = 0f; f <= 1; f += 0.05f)
        {
            Color c = Fade_Mask.GetComponent<Image>().color;
            c.a = f;
            Fade_Mask.GetComponent<Image>().color = c;
            yield return null;
        }
        for (int i = 0; i < UIPopUpAtPlayerDeath.Length; i++)
        {
            UIPopUpAtPlayerDeath[i].SetActive(true);
        }
        UIHighscore.GetComponent<Text>().text = PlayerPrefs.GetInt("highscore").ToString();
        Time.timeScale = 1.0f;
        yield break;
    }
    IEnumerator circularBulletPosChange()
    {
        PlayerPrefs.SetFloat("CircularCenterPosX", 0.0f);
        PlayerPrefs.SetFloat("CircularCenterPosY", -20.0f);
        while (true)
        {
            yield return new WaitForSeconds(8.0f);
            if (CircularBulletChangePosActive)
            {
                float rand_pos_x = Random.Range(-0.5f, 0.5f) * range;
                float rand_pos_y = Random.Range(-0.5f, 0.5f) * range;
                Instantiate(StarEffect, new Vector3(rand_pos_x, rand_pos_y, -6), Quaternion.identity);
                yield return new WaitForSeconds(2.0f);
                PlayerPrefs.SetFloat("CircularCenterPosX", rand_pos_x);
                PlayerPrefs.SetFloat("CircularCenterPosY", rand_pos_y);
            }
            else
            {
                PlayerPrefs.SetFloat("CircularCenterPosX", 0);
                PlayerPrefs.SetFloat("CircularCenterPosY", -20);
            }
        }
    }
}
