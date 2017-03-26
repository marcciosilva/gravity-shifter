﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityShifter : MonoBehaviour
{

    private Rigidbody2D _physicsBody;
    public GameObject gravityParticleSystem;
    private GameObject _currentGravityParticleSystem;
    private LevelManager _levelManager;
    private AudioSource _hitAudio;

    // Use this for initialization
    void Start()
    {
        _physicsBody = GetComponent<Rigidbody2D>();
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        AudioSource[] _audioSources = GetComponents<AudioSource>();
        if (_audioSources.Length >= 3) // Need third audio source from player
        {
            _hitAudio = _audioSources[2];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_levelManager.isGamePaused && Input.GetButtonDown("Invert Gravity"))
        {
            // Invert game object's gravity scale.
            _physicsBody.gravityScale *= -1.0f;
            if (gravityParticleSystem != null)
                _currentGravityParticleSystem = Instantiate(gravityParticleSystem, _physicsBody.transform.position, Quaternion.identity);
            Destroy(_currentGravityParticleSystem, 1);
            // Play gravity shift sound.
            _hitAudio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Exit door")
        {
            // TODO tidy this shit up, appears on two scripts.
            this.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deathzone")
        {
            this.enabled = false;
        }
    }

}
