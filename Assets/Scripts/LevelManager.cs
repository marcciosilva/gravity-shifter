using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    private const string _loseSceneName = "Lose";
    private float _levelDuration = 30; // seconds
    private Text _timerText;
    public bool isGamePaused = false;
    private GameObject _canvasPaused;

	// Use this for initialization
	void Start () {
        GameObject tmp = GameObject.Find("TextTimeLeft");
        if (tmp != null)
        {
            _timerText = tmp.GetComponent<Text>();
        } else
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
	void Update () {
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
            _timerText.text = "TIME LEFT - " + string.Format("{0}:{1:00}", (int)_levelDuration / 60, (int)_levelDuration % 60);
            _levelDuration -= Time.deltaTime;
            if (_levelDuration <= 1) SceneManager.LoadScene(_loseSceneName);
        }


    }
}
