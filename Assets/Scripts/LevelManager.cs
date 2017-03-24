using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private const string _loseSceneName = "Lose";
    public int currentLevel;
    private int maxLevel = 1;
    private float[] _levelDurations = new float[] { 10, 10, 15, 10, 15, 35 }; // seconds
    private Text _timerText;
    public bool isGamePaused = false;
    private GameObject _canvasPaused;
    public bool isPlayerAlive = true;
    private AudioSource _audioSource;
    private const float _audioSourceVolume = 1.0f;

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
        // Audio.
        tmp = GameObject.Find("Music");
        if (tmp != null)
        {
            _audioSource = tmp.GetComponent<AudioSource>();
            _audioSource.volume = _audioSourceVolume;
            if (EditorSceneManager.GetActiveScene().name.Contains("Level")) _audioSource.enabled = true;
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
                _audioSource.Pause();
            }
            else
            {
                Time.timeScale = 1;
                _canvasPaused.SetActive(false);
                _audioSource.UnPause();
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
        if (_audioSource != null && !sceneName.Contains("Level"))
        {
            int i;
            int fadePhases = 10;
            for (i = fadePhases; i > 0; i--)
            {
                _audioSource.volume = i * 0.1f;
                yield return new WaitForSeconds(fadeTime / fadePhases);
            }
            //Destroy(_audioSource);
            _audioSource.enabled = false;
        } else
        {
            yield return new WaitForSeconds(fadeTime);
        }
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
