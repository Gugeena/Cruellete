using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused;
    public GameObject pausePanel;
    Animator pauseAnim;
    bool running;
    private void Start()
    {
        running = false;
        gamePaused = false;
        pauseAnim = pausePanel.GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !running)
        {
            if (!gamePaused) StartCoroutine(pauseGame());
            else StartCoroutine(unPauseGame());
        }
    }

    public void unPause()
    {
        StartCoroutine(unPauseGame());
    }

    private IEnumerator pauseGame()
    {
        running = true;
        pausePanel.SetActive(true);
        pauseAnim.Play("pauseOn");
        gamePaused = true;
        yield return new WaitForSecondsRealtime(0.35f);
        running = false;
        Time.timeScale = 0.0f;
    }

    private IEnumerator unPauseGame()
    {
        running = true;
        gamePaused = false;
        Time.timeScale = 1.0f;
        pauseAnim.Play("pauseOff");
        yield return new WaitForSecondsRealtime(0.4f);
        pausePanel.SetActive(false);
        running = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BuffedReload()
    {
        SceneManager.LoadScene(6);
    }
}
