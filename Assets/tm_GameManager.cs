using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tm_GameManager : MonoBehaviour
{
    public enum States
    {
        wait, play, levelup, dead
    }
    public static States state;

    int level;
    int score;
    int lives;

    public Text levelTxt;
    public Text scoreTxt;
    public Text livesTxt;

    public Text messageTxt;

    public GameObject enemy;
    GameObject player;
    public GameObject boom;

    Camera cam;

    float height, width;

    public GameObject waitToStart; // panel

    public GameObject capsule;
    readonly float capsuleSpeed = 4f;

    private Vector2 capsulePosition;

    // Start is called before the first frame update
    void Start()
    {
        messageTxt.gameObject.SetActive(false);

        player = GameObject.FindWithTag("Player");

        messageTxt.gameObject.SetActive(false);

        cam = Camera.main;
        height = cam.orthographicSize;
        width = height * cam.aspect;

        waitToStart.gameObject.SetActive(true);
        int highscore = PlayerPrefs.GetInt("highscore");
        if (highscore > 0)
        {
            messageTxt.text = "highscore: " + highscore;
            messageTxt.gameObject.SetActive(true);
        }

        state = States.wait;
    }

    public void tm_LaunchGame()
    {
        // interface
        waitToStart.gameObject.SetActive(false);
        messageTxt.gameObject.SetActive(false);

        // restaurer après une partie
        player.SetActive(true);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // lancer une partie
        tm_InitGame();
        tm_LoadLevel();
        tm_UpdateTexts();
    }

    void tm_LoadLevel()
    {
        state = States.play;

        int maxEnemies = level > 6 ? 6 : level;
        for (int i = 1; i <= maxEnemies; i++)
        {
            float x = Random.Range(-width, width);
            float y = (-height + 1f) + (i - 1) * 2;
            Instantiate(enemy, new Vector2(x, y), Quaternion.identity);
        }
    }

    void tm_InitGame()
    {
        level = 1;
        score = 0;
        lives = 3;
    }

    void tm_UpdateTexts()
    {
        levelTxt.text = "level: " + level;
        scoreTxt.text = "score: " + score;
        livesTxt.text = "lives: " + lives;
    }

    public void tm_AddScore(int points)
    {
        score += points;
        tm_UpdateTexts();
    }

    private void Update()
    {
        if (state == States.play)
        {
            tm_EndOfLevel();
        }
    }

    void tm_EndOfLevel()
    {
        // chercher les astéroïdes
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 1)
        {
            capsulePosition = enemies[0].transform.position;
        }

        if (enemies.Length == 0)
        {
            StartCoroutine(tm_LevelUp());
        }
    }

    void tm_InstantiateCapsule()
    {
        GameObject capsuleObject = Instantiate(capsule, capsulePosition, Quaternion.identity);
        capsuleObject.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, capsuleSpeed, 0);
    }

    IEnumerator tm_LevelUp()
    {
        tm_InstantiateCapsule();

        state = States.levelup;

        // afficher message "level up"
        messageTxt.text = "level up";
        messageTxt.gameObject.SetActive(true);

        // marquer une pause
        yield return new WaitForSecondsRealtime(6f);

        // cacher le message
        messageTxt.gameObject.SetActive(false);
        level += 1;
        tm_LoadLevel();
        tm_UpdateTexts();
    }

    public void tm_KillPlayer()
    {
        StartCoroutine(tm_PlayerAgain());
    }

    IEnumerator tm_PlayerAgain()
    {
        state = States.dead;
        lives -= 1;
        player.SetActive(false);
        tm_UpdateTexts();
        GameObject boomGO = Instantiate(boom, player.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);
        if (lives <= 0)
        {
            Destroy(boomGO);
            tm_GameOver();
        }
        else
        {
            player.SetActive(true);
            state = States.play;
            Destroy(boomGO);
        }
    }

    void tm_GameOver()
    {
        state = States.wait;

        int highscore = PlayerPrefs.GetInt("highscore");
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            messageTxt.text = "new highscore: " + score;
        }
        else
        {
            messageTxt.text = "game over\nhighscore: " + highscore;
        }

        messageTxt.gameObject.SetActive(true);
        waitToStart.gameObject.SetActive(true);
    }
}
