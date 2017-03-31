using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorHandler : MonoBehaviour
{

    private int _currentPosition = 0;
    private Vector3 _exitTextVertPosition; // Third coordinate is x-width of the text
    private Vector3 _startTextVertPosition; // Third coordinate is x-width of the text
    [Range(0.0f, 0.5f)]
    public float _delayBetweenActions = 0.144f; // seconds
    private float _timeSinceLastAction; // seconds
    private Vector3 _secondOptionPosition; // Third coordinate is x-width of the text
    private string _sceneName;
    public RectTransform[] availableCursorPositions;
    public string[] sceneNames;
    private RectTransform _cursorRecttransform;
    private float _halfCursorWidth;

    // Use this for initialization
    void Start()
    {
        _cursorRecttransform = GetComponent<RectTransform>();
        _halfCursorWidth = (_cursorRecttransform.rect.width / 2.0f) * 1.5f;
        _sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(
            availableCursorPositions[_currentPosition].anchoredPosition.x - availableCursorPositions[_currentPosition].rect.width / 2.0f - _halfCursorWidth,
            availableCursorPositions[_currentPosition].anchoredPosition.y
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
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentPosition = (_currentPosition + 1) % availableCursorPositions.Length;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (_currentPosition == 0)
            {
                _currentPosition = availableCursorPositions.Length - 1;
            }
            else
            {
                _currentPosition = (_currentPosition - 1) % availableCursorPositions.Length;
            }
            GetComponent<RectTransform>().anchoredPosition = new Vector3(
                availableCursorPositions[_currentPosition].anchoredPosition.x - availableCursorPositions[_currentPosition].rect.width / 2.0f - _halfCursorWidth,
                availableCursorPositions[_currentPosition].anchoredPosition.y
            );
        }
        else // Joystick.
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
                        availableCursorPositions[_currentPosition].anchoredPosition.x - availableCursorPositions[_currentPosition].rect.width / 2.0f - _halfCursorWidth,
                        availableCursorPositions[_currentPosition].anchoredPosition.y
                    );
                }
            }
        }


    }

}
