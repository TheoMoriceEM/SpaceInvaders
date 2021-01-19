using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tm_PauseManager : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public Text pauseTxt;

    // Start is called before the first frame update
    void Start()
    {
        pauseTxt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            tm_PauseGame();
        }
    }

    void tm_PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
        pauseTxt.gameObject.SetActive(gameIsPaused);
    }
}
