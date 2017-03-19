using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public static readonly string[] boundaryStrings = { "Upper_boundary", "Right_boundary", "Left_boundary", "Lower_boundary" };
    private Transform _playerTransform;
    private string _playerTag = "Player";
    Camera _cam;
    // Camera limits
    Vector4 camLimits; // minX, maxX, minY, maxY.
    private float _orthographicSize = 360.0f / 32.0f / 2.0f;


    const int PIXELS_PER_UNIT = 32;
    public float camLerp = 4.0f;
    float halfScreenWidth;
    float halfScreenHeight;

    // Use this for initialization.
    void Start()
    {
        // TODO update when screen size is changed?
        camLimits = new Vector4();
        halfScreenWidth = 640 / 2.0f / PIXELS_PER_UNIT;
        halfScreenHeight = 360 / 2.0f / PIXELS_PER_UNIT;
        // Get boundary information.
        GameObject tmpGameObject;

        foreach (string boundaryString in boundaryStrings)
        {
            tmpGameObject = GameObject.Find(boundaryString);
            if (tmpGameObject != null)
            {
                if (boundaryString.Equals(boundaryStrings[0]))
                {
                    // maxY.
                    camLimits.w = tmpGameObject.GetComponent<Transform>().position.y - halfScreenHeight;
                } else if (boundaryString.Equals(boundaryStrings[1]))
                {
                    // maxX.
                    camLimits.y = tmpGameObject.GetComponent<Transform>().position.x - halfScreenWidth;
                } else if (boundaryString.Equals(boundaryStrings[2]))
                {
                    // minX.
                    camLimits.x = tmpGameObject.GetComponent<Transform>().position.x + halfScreenWidth;
                } else if (boundaryString.Equals(boundaryStrings[3]))
                {
                    // minY.
                    camLimits.z = tmpGameObject.GetComponent<Transform>().position.y + halfScreenHeight;
                }
            }
        }
        _cam = GetComponent<Camera>();
        _cam.orthographicSize = _orthographicSize;
        GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // This causes the world to be always of the same apparent size
        // Changes in resolution will result in viewing more or less of the same world
        //_cam.orthographicSize = Screen.height / 32.0f / 2.0f;
        // I chose to just set the orthographicSize as fixed, and allow 2x and 4x resolutions
    }

    private void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            Vector3 playerPosition = _playerTransform.position;
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(Mathf.Clamp(playerPosition.x, camLimits.x, camLimits.y), Mathf.Clamp(playerPosition.y, camLimits.z, camLimits.w), this.transform.position.z),
                Time.deltaTime * camLerp);
        }
    }

    private void LateUpdate()
    {

    }

}
