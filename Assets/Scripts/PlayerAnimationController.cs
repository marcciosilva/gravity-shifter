#undef DEBUG
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimationController : MonoBehaviour
{
    private const float verticalVelocityThreshold = 0.2f;
    private const float horizontalVelocityThreshold = 0.1f;
    private Animator _animator;
    private Rigidbody2D _physicsBody;
    private SpriteRenderer _spriteRenderer;
    // TODO clean this shit up.
    private MovementInputController _inputController;
    private LevelManager _levelManager;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _physicsBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputController = GetComponent<MovementInputController>();
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_physicsBody.velocity.x > horizontalVelocityThreshold)
        {
#if (DEBUG)
            Debug.Log("Running-Right");
#endif
            _animator.SetBool("Running-Right", true);
            _spriteRenderer.flipX = false;
            _animator.SetBool("Running-Left", false);
        }
        else if (_physicsBody.velocity.x < -horizontalVelocityThreshold)
        {
#if (DEBUG)
            Debug.Log("Running-Left");
#endif
            _spriteRenderer.flipX = true;
            _animator.SetBool("Running-Left", true);
            _animator.SetBool("Running-Right", false);
        }
        else
        {
#if (DEBUG)
            Debug.Log("Not running");
#endif
            _animator.SetBool("Running-Right", false);
            _animator.SetBool("Running-Left", false);
        }

        if (_animator.GetBool("OnAir")) _animator.SetBool("OnAir", false);

        if (_inputController.onAir)
        {
            _animator.SetBool("OnAir", true);
        }

        if (!_levelManager.isPlayerAlive)
        {
            _animator.SetBool("Dead", true);
        }


        // Update sprite according to current gravity scale.
        if (_physicsBody.gravityScale >= 0.0f)
        {
            //_spriteRenderer.flipY = false;
            this.transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
        else
        {
            //_spriteRenderer.flipY = true;
            this.transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        }

    }
}
