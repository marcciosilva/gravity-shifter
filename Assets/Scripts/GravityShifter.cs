using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityShifter : MonoBehaviour {

    private Rigidbody2D _physicsBody;
    public GameObject gravityParticleSystem;
    private GameObject _currentGravityParticleSystem;
    private LevelManager _levelManager;

    // Use this for initialization
    void Start () {
        _physicsBody = GetComponent<Rigidbody2D>();
        _levelManager = GameObject.Find("LevelManager").GetComponent <LevelManager> ();
    }
	
	// Update is called once per frame
	void Update () {
        if (!_levelManager.isGamePaused && Input.GetButtonDown("Invert Gravity"))
        {
            // Invert game object's gravity scale.
            _physicsBody.gravityScale *= -1.0f;
            if (gravityParticleSystem != null)
                _currentGravityParticleSystem = Instantiate(gravityParticleSystem, _physicsBody.transform.position, Quaternion.identity);
                Destroy(_currentGravityParticleSystem, 1);
        }
    }

}
