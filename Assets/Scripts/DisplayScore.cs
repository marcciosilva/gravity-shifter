using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{

    private Text _text;
    public enum ScoreType { Latest, First, Second, Third, Fourth, Fifth}
    public ScoreType scoreType;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>();
        if (_text != null)
        {
            switch(scoreType)
            {
                case ScoreType.Latest:
                    _text.text = "SCORE: " + PlayerPrefs.GetInt("Latest score", 0);
                    break;
                case ScoreType.First:
                    _text.text = "First: " + PlayerPrefs.GetInt("First score", 0);
                    break;
                case ScoreType.Second:
                    _text.text = "Second: " + PlayerPrefs.GetInt("Second score", 0);
                    break;
                case ScoreType.Third:
                    _text.text = "Third: " + PlayerPrefs.GetInt("Third score", 0);
                    break;
                case ScoreType.Fourth:
                    _text.text = "Fourth: " + PlayerPrefs.GetInt("Fourth score", 0);
                    break;
                case ScoreType.Fifth:
                    _text.text = "Fifth: " + PlayerPrefs.GetInt("Fifth score", 0);
                    break;
                default:
                    break;
            }
            
        }
    }

}
