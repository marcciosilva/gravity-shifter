using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    private InteractiveText _interactiveText;

    private int _currentMessage = 0;
    private string[] _messages = new string[] {
        "You signed up for the gravity shifter test program of our organization",
        "You will go through various test rooms, with the objective of getting to the exit under a time constraint",
        "The amount of gravity inversions you might apply on yourself will be limited, as it's of extreme importance to use the equipment efficiently",
        "You might try a room again upon failing, only three times during the whole program",
        "Good luck, and may you join our forces in the future!" };

    void Start()
    {
        _interactiveText = GetComponent<InteractiveText>();
        if (_interactiveText == null) this.enabled = false;
    }

    private void Update()
    {
        if (_currentMessage == 0)
        {
            ShowNewMessage();
        }
        else if (_currentMessage < _messages.Length && Input.GetButtonDown("Jump"))
        {
            ShowNewMessage();
        }
        else if (_currentMessage >= _messages.Length && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(EndTransmission());
            StartCoroutine(StartGame());

        } else if (Input.GetButtonDown("Invert Gravity"))
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        float fadeTime = GameObject.Find("_GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Level-1");
    }

    private void ShowNewMessage()
    {
        bool success = _interactiveText.ShowNewText(_messages[_currentMessage]);
        if (success)
        {
            _currentMessage++;
            StartCoroutine(WaitABit());
        }
    }

    IEnumerator EndTransmission()
    {
        bool success = _interactiveText.ShowNewText("");
        while (!success)
        {
            yield return new WaitForSeconds(0.1f);
            _interactiveText.ShowNewText("");
        }
    }

    IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(2.0f);
    }


}
