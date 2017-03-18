using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    private const string _loseSceneName = "Lose";
    private float _levelDuration = 5;
    private Text _timerText;


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

    }
	
	// Update is called once per frame
	void Update () {
        _timerText.text = "TIME LEFT - " + string.Format("{0}:{1:00}", (int)_levelDuration / 60, (int)_levelDuration % 60);
        _levelDuration -= Time.deltaTime;
        if (_levelDuration <= 1) SceneManager.LoadScene(_loseSceneName);
    }
}
