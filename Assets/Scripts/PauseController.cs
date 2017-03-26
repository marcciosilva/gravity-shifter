using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(Fade());
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1.0f;
        }
        else if (Input.GetButtonDown("Invert Gravity"))
        {
            StartCoroutine(Fade());
            Application.Quit();
        }
    }

    IEnumerator Fade()
    {
        float fadeTime = GameObject.Find("_GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
    }

}
