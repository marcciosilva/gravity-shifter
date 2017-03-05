using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{

    private Transform _playerTransform;
    private string _playerTag = "Player";
    public Vector3 cameraOffset = new Vector3();
    Camera _cam;
    public float scaleFactor;

    // Use this for initialization
    void Start()
    {
        _cam = GetComponent<Camera>();
        GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _cam.orthographicSize = (Screen.height / 100f) / scaleFactor;
        if (_playerTransform != null)
        {
            Vector3 playerPosition = _playerTransform.position;
            // TODO limit x position
            this.transform.position = new Vector3(playerPosition.x, playerPosition.y, this.transform.position.z) + cameraOffset;
        }
    }

}
