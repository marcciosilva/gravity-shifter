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
    public float scaleFactor;
    // Boundaries for camera following.
    Transform upperBoundary;
    Transform rightBoundary;
    Transform leftBoundary;
    Transform lowerBoundary;

    float halfScreenWidth;
    float halfScreenHeight;

    // Use this for initialization.
    void Start()
    {
        // TODO update when screen size is changed?
        halfScreenWidth = Screen.width / 2.0f;
        halfScreenHeight = Screen.height / 2.0f;
        // Get boundary information.
        GameObject tmpGameObject;

        foreach (string boundaryString in boundaryStrings)
        {
            tmpGameObject = GameObject.Find(boundaryString);
            if (tmpGameObject != null)
            {
                if (boundaryString.Equals(boundaryStrings[0]))
                {
                    upperBoundary = tmpGameObject.GetComponent<Transform>();
                } else if (boundaryString.Equals(boundaryStrings[1]))
                {
                    rightBoundary = tmpGameObject.GetComponent<Transform>();
                } else if (boundaryString.Equals(boundaryStrings[2]))
                {
                    leftBoundary = tmpGameObject.GetComponent<Transform>();
                } else if (boundaryString.Equals(boundaryStrings[3]))
                {
                    lowerBoundary = tmpGameObject.GetComponent<Transform>();
                }
            }
        }


        
        _cam = GetComponent<Camera>();
        GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    // Update is called once per frame.
    void Update()
    {
        //_cam.orthographicSize = (Screen.height / 100f) / scaleFactor;
        Camera.main.orthographicSize = Screen.height / 32.0f / 2.0f;
        if (_playerTransform != null)
        {
            Vector3 playerPosition = _playerTransform.position;
            // TODO limit x position.
            Vector3 newCamPosition = new Vector3(playerPosition.x, playerPosition.y, this.transform.position.z);
            if ((leftBoundary != null && newCamPosition.x - halfScreenWidth <= leftBoundary.position.x)
                || (rightBoundary != null && newCamPosition.x + halfScreenWidth >= rightBoundary.position.x))
            {
                newCamPosition.x = transform.position.x; // Remains unchanged.
            }
            transform.position = newCamPosition;
            //this.transform.position = Vector3.Lerp(transform.position, new Vector3(playerPosition.x, playerPosition.y, this.transform.position.z), 0.5f);
        }
    }

}
