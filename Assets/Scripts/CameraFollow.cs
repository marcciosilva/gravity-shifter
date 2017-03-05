using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform _playerTransform;
    private string _playerTag = "Player";
    public Vector3 cameraOffset = new Vector3();

    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform != null)
        {
            Vector3 playerPosition = _playerTransform.position;
            // TODO limit x position
            this.transform.position = new Vector3(playerPosition.x, this.transform.position.y, this.transform.position.z) + cameraOffset;
        }
    }

}
