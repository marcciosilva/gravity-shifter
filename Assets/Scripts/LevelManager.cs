using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private const string _loseSceneName = "Lose";
    public int currentLevel;
    private int maxLevel = 5;
    private float[] _levelDurations = new float[] { 10, 10, 15, 10, 20 }; // seconds
    private Text _timerText;
    public bool isGamePaused = false;
    private GameObject _canvasPaused;
    public bool isPlayerAlive = true;

    // Use this for initialization
    void Start()
    {
        GameObject tmp = GameObject.Find("TextTimeLeft");
        if (tmp != null)
        {
            _timerText = tmp.GetComponent<Text>();
        }
        else
        {
            this.enabled = false;
        }
        tmp = GameObject.Find("CanvasPaused");
        if (tmp != null)
        {
            _canvasPaused = tmp;
            _canvasPaused.SetActive(isGamePaused);
        }
        else
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused)
            {
                Time.timeScale = 0;
                _canvasPaused.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                _canvasPaused.SetActive(false);
            }

        }

        if (!isGamePaused)
        {
            _timerText.text = "TIME LEFT - " + string.Format("{0}:{1:00}", (int)_levelDurations[currentLevel - 1] / 60, (int)_levelDurations[currentLevel - 1] % 60);
            _levelDurations[currentLevel - 1] -= Time.deltaTime;
            if (_levelDurations[currentLevel - 1] <= 1) lostLevel();
        }

    }

    IEnumerator LoadScene(string sceneName)
    {
        float fadeTime = GameObject.Find("_GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    public void lostLevel()
    {
        StartCoroutine("LoadScene", _loseSceneName);
    }

    public void reachedExit()
    {
        Debug.Log("Current level is " + currentLevel);
        int nextLevel = currentLevel + 1;
        if (nextLevel <= maxLevel)
        {
            Debug.Log("Loading level " + nextLevel);
            StartCoroutine("LoadScene", "Level-" + nextLevel);
            this.enabled = false;
        }
        else
        {
            // TODO implement win screen.
            StartCoroutine("LoadScene", "MainMenu");
            this.enabled = false;
        }
    }
}
