using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
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

    public GameObject asteroid;
    GameObject player;
    public GameObject boom;

    Camera cam;

    float height, width;

    public GameObject waitToStart; // panel

    public GameObject capsule;
    readonly float capsuleSpeed = 2f;

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

    public void LaunchGame()
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
        InitGame();
        LoadLevel();
        UpdateTexts();
    }

    void LoadLevel()
    {
        state = States.play;

        int maxEnemies = level > 6 ? 6 : level;
        for (int i = 1; i <= maxEnemies; i++)
        {
            float x = Random.Range(-width, width);
            float y = (-height + 1f) + (i - 1) * 2;
            Instantiate(asteroid, new Vector2(x, y), Quaternion.identity);
        }
    }

    void InitGame()
    {
        level = 1;
        score = 0;
        lives = 3;
    }

    void UpdateTexts()
    {
        levelTxt.text = "level: " + level;
        scoreTxt.text = "score: " + score;
        livesTxt.text = "lives: " + lives;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateTexts();
    }

    private void Update()
    {
        if (state == States.play)
        {
            EndOfLevel();
        }
    }

    void EndOfLevel()
    {
        // chercher les astéroïdes
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 1)
        {
            capsulePosition = enemies[0].transform.position;
        }

        if (enemies.Length == 0)
        {
            StartCoroutine(LevelUp());
        }
    }

    void InstantiateCapsule()
    {
        GameObject capsuleObject = Instantiate(capsule, capsulePosition, Quaternion.identity);
        capsuleObject.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, capsuleSpeed, 0);
    }

    IEnumerator LevelUp()
    {
        InstantiateCapsule();

        state = States.levelup;

        // afficher message "level up"
        messageTxt.text = "level up";
        messageTxt.gameObject.SetActive(true);

        // marquer une pause
        yield return new WaitForSecondsRealtime(10f);

        // cacher le message
        messageTxt.gameObject.SetActive(false);
        level += 1;
        LoadLevel();
        UpdateTexts();
    }

    public void KillPlayer()
    {
        StartCoroutine(PlayerAgain());
    }

    IEnumerator PlayerAgain()
    {
        state = States.dead;
        lives -= 1;
        player.SetActive(false);
        UpdateTexts();
        GameObject boomGO = Instantiate(boom, player.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);
        if (lives <= 0)
        {
            Destroy(boomGO);
            GameOver();
        }
        else
        {
            player.SetActive(true);
            state = States.play;
            Destroy(boomGO);
        }
    }

    void GameOver()
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
