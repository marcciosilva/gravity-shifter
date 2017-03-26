using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorHandler : MonoBehaviour
{

    private int _currentPosition = 0;
    private int _availablePositions = 3;
    private Vector3 _exitTextVertPosition; // Third coordinate is x-width of the text
    private Vector3 _startTextVertPosition; // Third coordinate is x-width of the text
    private float _delayBetweenActions = 0.5f; // seconds
    private float _timeSinceLastAction; // seconds
    private Vector3 _secondOptionPosition; // Third coordinate is x-width of the text
    private string _sceneName;

    // Use this for initialization
    void Start()
    {
        // TODO should decouple this to make it reusable for any type of menu.
        _sceneName = SceneManager.GetActiveScene().name;
        GameObject tmp = GameObject.Find("TextStartGame");
        if (tmp != null)
        {
            _startTextVertPosition = tmp.transform.position;
            _startTextVertPosition.z = tmp.GetComponent<RectTransform>().rect.width;
        }
        tmp = GameObject.Find("TextExitGame");
        if (tmp != null)
        {
            _exitTextVertPosition = tmp.transform.position;
            _startTextVertPosition.z = tmp.GetComponent<RectTransform>().rect.width;
        }
        if (_sceneName == "MainMenu")
        {
            tmp = GameObject.Find("TextCredits");
        }
        else if (_sceneName == "Lose")
        {
            tmp = GameObject.Find("TextMainMenu");
        }
        if (tmp != null)
        {
            _secondOptionPosition = tmp.transform.position;
            _startTextVertPosition.z = tmp.GetComponent<RectTransform>().rect.width;
        }
    }

    // Update is called once per frame
    void Update()
    {

        updateCursor();
        checkSelection();

    }

    private void checkSelection()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Fade());
            switch (_currentPosition)
            {
                case 0:
                    if (_sceneName == "MainMenu") SceneManager.LoadScene("Tutorial");
                    else SceneManager.LoadScene("Level-1");
                    break;
                case 1:
                    if (_sceneName == "MainMenu") SceneManager.LoadScene("Credits");
                    else SceneManager.LoadScene("MainMenu");
                    break;
                case 2:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator Fade()
    {
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
                _currentPosition = Mathf.Abs(_currentPosition + 1) % _availablePositions;

            }
            else if (Input.GetAxis("Vertical") > 0.5f)
            {
                Debug.Log("Blah");
                _timeSinceLastAction = 0;
                if (_currentPosition == 0)
                {
                    _currentPosition = _availablePositions - 1;
                }
                else
                {
                    _currentPosition = Mathf.Abs(_currentPosition - 1) % _availablePositions;
                }
            }
            if (_timeSinceLastAction == 0)
            {
                switch (_currentPosition)
                {
                    case 0:
                        transform.position = new Vector3(transform.position.x, _startTextVertPosition.y, transform.position.z);
                        break;
                    case 1:
                        transform.position = new Vector3(transform.position.x, _secondOptionPosition.y, transform.position.z);
                        break;
                    case 2:
                        transform.position = new Vector3(transform.position.x, _exitTextVertPosition.y, transform.position.z);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
