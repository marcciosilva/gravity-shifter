using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    //private LevelManager _levelManager;
    //private MusicManager _musicManager;
    //private bool _activeState = false;

    // Use this for initialization
    //void Start () {
        //GameObject tmp = GameObject.Find("MusicManager");
        //if (tmp != null) _musicManager = tmp.GetComponent<MusicManager>();
    //}

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(Fade());
            SceneManager.LoadScene("MainMenu");
        } else if (Input.GetButtonDown("Invert Gravity"))
        {
            StartCoroutine(Fade());
            Application.Quit();
        }
        //if (gameObject.activeInHierarchy) Debug.Log("Puto");
        //if (gameObject.activeInHierarchy != _activeState)
        //{
        //    _activeState = !_activeState;
        //    if (_activeState)
        //    {
        //        OnActive();
        //    }
        //    else OnInactive();
        //}
    }

    //private void OnActive()
    //{
    //    if (_musicManager != null) _musicManager.PauseAllAudio();
    //}

    //private void OnInactive()
    //{
    //    if (_musicManager != null) _musicManager.ResumeAllAudio();
    //}

    IEnumerator Fade()
    {
        float fadeTime = GameObject.Find("_GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
    }

}
