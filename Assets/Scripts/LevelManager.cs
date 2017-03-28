using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private const string _loseSceneName = "Lose";
    public int currentLevel;
    private int maxLevel = 8;
    private float[] _currentLevelDurations = new float[] { 10, 10, 15, 10, 15, 35, 30, 15 }; // seconds
    private float[] _maxLevelDurations = new float[] { 10, 10, 15, 10, 15, 35, 30, 15 }; // seconds
    private float[] _perfectLevelDurations = new float[] { 5.42f, 5.238f, 6.87f, 7.14f, 12.5f, 28.2f, 16.4f, 9.5f }; // seconds
    private float[] _maxInversionsPerLevel = new float[] { 2, 3, 5, 9, 7, 15, 4, 20 };
    private float[] _perfectInversionsPerLevel = new float[] { 0, 1, 1, 5, 2, 9, 2, 14 };
    private float _inversionsLeft;
    private Text _inversionsText;
    private Text _livesLeftText;
    private Text _timerText;
    public bool isGamePaused = false;
    private GameObject _canvasPaused;
    public bool isPlayerAlive = true;
    private MusicManager _musicManager;
    private static float _score = 0.0f;
    private bool _reachedExit = false;
    private bool _lostLevel = false;
    private static int _maxLives = 3;
    private static int _currentLives = 3;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Lives: " + _currentLives);
        // Reset score.
        if (currentLevel == 1) _score = 0.0f;
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
        tmp = GameObject.Find("TextLivesLeft");
        if (tmp != null)
        {
            _livesLeftText = tmp.GetComponent<Text>();
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
            // Patch for not showing life reset upon winning.
            if (!_reachedExit) _livesLeftText.text = _currentLives + "/" + _maxLives;
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
        if (!_lostLevel)
        {
            _lostLevel = true;
            _currentLives--;
            if (_currentLives == 0)
            {
                PlayerPrefs.SetInt("Latest score", (int)_score);
                UpdateHighscores((int)_score);
                StartCoroutine(LoadScene(_loseSceneName));
                _currentLives = _maxLives;
            } else
            {
                StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
            }
        }
    }

    public void reachedExit()
    {
        if (!_reachedExit)
        {
            _reachedExit = true;
            float timeElapsedInLevel = _maxLevelDurations[currentLevel - 1] - _currentLevelDurations[currentLevel - 1];
            Debug.Log("Time elapsed in level: " + timeElapsedInLevel);
            // Max score for each level is 100.
            // Score is multiplied by remaining lives once you win the game.
            _score += 
                ((1.0f / timeElapsedInLevel) / (1.0f / _perfectLevelDurations[currentLevel - 1])) * 70
                + (_inversionsLeft / (_maxInversionsPerLevel[currentLevel - 1] - _perfectInversionsPerLevel[currentLevel - 1])) * 30;
            _score = Mathf.Ceil(_score);
            Debug.Log("Score: " + _score);
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
                _score *= _currentLives; // Maximum would be maxLevel * 100 * _currentLives.
                // If I have 6 levels, the max score would be 1800.
                PlayerPrefs.SetInt("Latest score", (int)_score);
                UpdateHighscores((int)_score);
                StartCoroutine("LoadScene", "Win");
                _currentLives = _maxLives;
                this.enabled = false;
            }
        }
    }

    private void UpdateHighscores(int score)
    {
        int[] scores = {
            PlayerPrefs.GetInt("First score", 0),
            PlayerPrefs.GetInt("Second score", 0),
            PlayerPrefs.GetInt("Third score", 0),
            PlayerPrefs.GetInt("Fourth score", 0),
            PlayerPrefs.GetInt("Fifth score", 0)
        };
        for (int i = 0; i < scores.Length; i++)
        {
            if (score > scores[i])
            {
                // Adjust the other scores if the new score rekt them.
                for (int j = 4; j > i; j--)
                {
                    scores[j] = scores[j - 1];
                }
                scores[i] = score;
                break;
            }
        }
        PlayerPrefs.SetInt("First score", scores[0]);
        PlayerPrefs.SetInt("Second score", scores[1]);
        PlayerPrefs.SetInt("Third score", scores[2]);
        PlayerPrefs.SetInt("Fourth score", scores[3]);
        PlayerPrefs.SetInt("Fifth score", scores[4]);
    }



}
