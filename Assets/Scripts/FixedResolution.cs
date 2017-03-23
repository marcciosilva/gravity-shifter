using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedResolution : MonoBehaviour {

    private Camera _cam;
    private float _orthographicSize = 360.0f / 32.0f / 2.0f;

    // Use this for initialization
    void Start () {
        _cam = GetComponent<Camera>();
        _cam.orthographicSize = _orthographicSize;
    }
	
	// Update is called once per frame
	void Update () {
        // This causes the world to be always of the same apparent size
        // Changes in resolution will result in viewing more or less of the same world
        //_cam.orthographicSize = Screen.height / 32.0f / 2.0f;
        // I chose to just set the orthographicSize as fixed, and allow 2x and 4x resolutions
    }
}
