#define JUMP_ENABLED
//#define DEBUG
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
/**
 * If jumping is required, some form of collider (as trigger) must be used.
 * */
public class MovementInputController : MonoBehaviour
{

    private Rigidbody2D _physicsBody;
    public float acceleration;
    public float desacceleration;
    public float maxSpeed;
    public bool horizontalMovementEnabled = true;
    public bool autoHorizontalMovement = false;
    public bool rotationEnabled = false;
    public float jumpSpeed = 20f;
    public bool onAir;
    private bool _shouldJump = false;
    private LevelManager _levelManager;
    private AudioSource _jumpAudio;
    private AudioSource _hitAudio;



    // Use this for initialization.
    void Start()
    {
        _physicsBody = GetComponent<Rigidbody2D>();
        _physicsBody.freezeRotation = !rotationEnabled;
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        AudioSource[] _audioSources = GetComponents<AudioSource>();
        if (_audioSources.Length >= 2) // Need jump and hit sound
        {
            _jumpAudio = _audioSources[0];
            _hitAudio = _audioSources[1];
        }
    }

    // Update is called once per frame.
    void Update()
    {
        if (!_levelManager.isGamePaused)
        {
#if (JUMP_ENABLED)
            if (Input.GetButtonDown("Jump") && !onAir)
            {
                _shouldJump = true;
#if (DEBUG)
                //Debug.Log("Should jump");
#endif
            }
#endif
        }
    }

    private void FixedUpdate()
    {
        if (_shouldJump)
        {
            // Multiply by transform.localScale.y's sign to take into account jumping under
            // different gravity scales.
            _physicsBody.AddForce(Vector3.up * Mathf.Sign(this.transform.localScale.y) * jumpSpeed);
            // Play jump sound.
            _jumpAudio.Play();
            _shouldJump = false;
#if (DEBUG)
            //Debug.Log("Should not jump");
#endif
        }
        if (horizontalMovementEnabled) CheckHorizontalMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Used for foot sensor and jumping.
        if (collision.name != "Exit door")
        {
            onAir = false;
            Debug.Log("On floor");
        }
        else
        {
            this.enabled = false;
            Debug.Log("Calling reachedExit");
            _levelManager.reachedExit();
            // To avoid re-calling reachedExit on further contact with the collider.
            collision.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Used for foot sensor and jumping.
        if (collision.name != "Exit door")
        {
            onAir = true;
            Debug.Log("On air");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deathzone")
        {
            this.enabled = false;
            _levelManager.isPlayerAlive = false;
            _levelManager.lostLevel();
            // Play hit sound.
            _hitAudio.Play();
        }
    }

    private void CheckHorizontalMovement()
    {
        if (!autoHorizontalMovement)
        {
            // Value of input.
            float axis = Input.GetAxis("Horizontal");
            if (axis != 0)
            {
                float xVelocity;
                // Velocity when going forward.
                if (axis > 0) xVelocity = Mathf.Min(_physicsBody.velocity.x + axis * acceleration * Time.deltaTime, maxSpeed);
                // Velocity when going back.
                else xVelocity = Mathf.Max(_physicsBody.velocity.x + axis * acceleration * Time.deltaTime, -maxSpeed);
                // Modify component's velocity on x (keeping y).
                // TODO change this to support vertical movement.
                _physicsBody.velocity = new Vector2(xVelocity, _physicsBody.velocity.y);
            }
            else
            {
                _physicsBody.velocity = new Vector2(_physicsBody.velocity.x / desacceleration, _physicsBody.velocity.y);
            }
        }
        else
        {
            _physicsBody.AddForce(new Vector2(1.0f, 0.0f) * acceleration * Time.deltaTime);
            _physicsBody.velocity = Vector2.ClampMagnitude(_physicsBody.velocity, maxSpeed);
        }
    }
}
