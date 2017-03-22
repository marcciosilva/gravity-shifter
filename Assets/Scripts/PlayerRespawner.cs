using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{

    private Vector3 _startPosition;
    private Rigidbody2D _physicsBody;
    private float _initGravityScale;

    // Use this for initialization
    void Start()
    {
        _startPosition = transform.position;
        _physicsBody = GetComponent<Rigidbody2D>();
        if (_physicsBody != null)
        {
            _initGravityScale = _physicsBody.gravityScale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Reached level end");
            Respawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get root element's tag
        if (collision.transform.root.CompareTag("Obstacle"))
        {
            // Obstacle is hit
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = _startPosition;
        if (_physicsBody != null)
        {
            _physicsBody.gravityScale = _initGravityScale;
            _physicsBody.velocity = new Vector2();
        }
    }

}
