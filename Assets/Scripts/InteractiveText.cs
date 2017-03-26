using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveText : MonoBehaviour
{

    private Text _text;
    private string textToDisplay;
    private bool _busy = false;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    /**
     * Returns true if text was shown.
     * */
    public bool ShowNewText(string msg)
    {
        textToDisplay = msg;
        if (_busy)
        {
            return false;
        }
        else
        {
            StartCoroutine(AnimateText(textToDisplay));
            _busy = true;
            return true;
        }
    }


    IEnumerator AnimateText(string strComplete)
    {
        // Wait before erasing text.
        yield return new WaitForSeconds(1.0f);
        // Clean display.
        if (_text.text != "")
        {
            while (_text.text.Length > 0)
            {
                _text.text = _text.text.Remove(_text.text.Length - 1);
                yield return new WaitForSeconds(0.001f);
            }
        }
        // Write text.
        int i = 0;
        while (i < strComplete.Length)
        {
            _text.text += strComplete[i++];
            yield return new WaitForSeconds(0.01f);
        }
        _busy = false;
    }
}
