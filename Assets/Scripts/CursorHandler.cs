using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorHandler : MonoBehaviour
{

    private int _currentPosition = 0;
    private Vector3 _exitTextVertPosition; // Third coordinate is x-width of the text
    private Vector3 _startTextVertPosition; // Third coordinate is x-width of the text
    private float _delayBetweenActions = 0.5f; // seconds
    private float _timeSinceLastAction; // seconds
    private Vector3 _secondOptionPosition; // Third coordinate is x-width of the text
    private string _sceneName;
    public Vector2[] availableCursorPositions;
    public string[] sceneNames;

    // Use this for initialization
    void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(
            availableCursorPositions[_currentPosition].x,
            availableCursorPositions[_currentPosition].y
        );
        updateCursor();
        checkSelection();
    }

    private void checkSelection()
    {
        // This assumes that sceneNames array in the editor has the same size
        // as the availableCursorPositions array.
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Fade());
            if (sceneNames[_currentPosition] == "Exit")
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(sceneNames[_currentPosition]);
            }
        }
    }

    IEnumerator Fade()
    {
        // TODO make this into an utility or something.
        float fadeTime = GameObject.Find("_GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
    }

    private void updateCursor()
    {
        _timeSinceLastAction += Time.deltaTime;
        if (Input.GetAxis("Vertical") != 0 && (_timeSinceLastAction > _delayBetweenActions))
        {
            if (Input.GetAxis("Vertical") < -0.5f)
            {
                _timeSinceLastAction = 0;
                _currentPosition = (_currentPosition + 1) % availableCursorPositions.Length;

            }
            else if (Input.GetAxis("Vertical") > 0.5f)
            {
                _timeSinceLastAction = 0;
                if (_currentPosition == 0)
                {
                    _currentPosition = availableCursorPositions.Length - 1;
                }
                else
                {
                    _currentPosition = (_currentPosition - 1) % availableCursorPositions.Length;
                }
            }
            if (_timeSinceLastAction == 0)
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector3(
                    availableCursorPositions[_currentPosition].x,
                    availableCursorPositions[_currentPosition].y
                );
            }
        }
    }

}
