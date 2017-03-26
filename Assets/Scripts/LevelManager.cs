using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private const string _loseSceneName = "Lose";
    public int currentLevel;
    private int maxLevel = 6;
    private float[] _currentLevelDurations = new float[] { 10, 10, 15, 10, 15, 35 }; // seconds
    private float[] _maxLevelDurations = new float[] { 10, 10, 15, 10, 15, 35 }; // seconds
    private float[] _perfectLevelDurations = new float[] { 6, 6, 15, 8, 13, 29 }; // seconds
    private float[] _maxInversionsPerLevel = new float[] { 2, 3, 9, 9, 7, 15 };
    private float[] _perfectInversionsPerLevel = new float[] { 0, 1, 5, 5, 2, 9 };
    private float _inversionsLeft;
    private Text _inversionsText;
    private Text _timerText;
    public bool isGamePaused = false;
    private GameObject _canvasPaused;
    public bool isPlayerAlive = true;
    private MusicManager _musicManager;
    private static float score = 0.0f;
    private bool onExit = false;

    // Use this for initialization
    void Start()
    {
        GameObject tmp = GameObject.Find("TextTimeLeft");
        if (tmp != null)
        {
            _timerText = tmp.GetComponent<Text>();
        }
        tmp = GameObject.Find("TextInversionsLeft");
        if (tmp != null)
        {
            _inversionsText = tmp.GetComponent<Text>();
        }
        else
        {
            this.enabled = false;
        }
        tmp = GameObject.Find("PauseGUI");
        if (tmp != null)
        {
            _canvasPaused = tmp;
            _canvasPaused.SetActive(isGamePaused);
        }
        else
        {
            this.enabled = false;
        }
        _inversionsLeft = _maxInversionsPerLevel[currentLevel - 1];
        tmp = GameObject.Find("MusicManager");
        if (tmp != null)
        {
            _musicManager = tmp.GetComponent<MusicManager>();
        }
    }

    public bool UseGravityInversion()
    {
        if (_inversionsLeft > 0)
        {
            _inversionsLeft--;
            return true;
        }
        else return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused)
            {
                _musicManager.PauseAllAudio();
                Time.timeScale = 0;
                _canvasPaused.SetActive(true);
            }
            else
            {
                _musicManager.ResumeAllAudio();
                Time.timeScale = 1;
                _canvasPaused.SetActive(false);
            }

        }

        if (!isGamePaused)
        {
            _inversionsText.text = _inversionsLeft + "/" + _maxInversionsPerLevel[currentLevel - 1];
            _timerText.text = "TIME LEFT - " + string.Format("{0}:{1:00}", (int)_currentLevelDurations[currentLevel - 1] / 60, (int)_currentLevelDurations[currentLevel - 1] % 60);
            _currentLevelDurations[currentLevel - 1] -= Time.deltaTime;
            if (_currentLevelDurations[currentLevel - 1] <= 1) lostLevel();
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
        if (!onExit)
        {
            onExit = true;
            score += ((_maxLevelDurations[currentLevel - 1] - _currentLevelDurations[currentLevel - 1]) / _perfectLevelDurations[currentLevel - 1]) * 50
                + (_inversionsLeft / (_maxInversionsPerLevel[currentLevel - 1] - _perfectInversionsPerLevel[currentLevel - 1])) * 50;
            Debug.Log("Score: " + score);
            //Debug.Log("Current level is " + currentLevel);
            int nextLevel = currentLevel + 1;
            if (nextLevel <= maxLevel)
            {
                //Debug.Log("Loading level " + nextLevel);
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

}
