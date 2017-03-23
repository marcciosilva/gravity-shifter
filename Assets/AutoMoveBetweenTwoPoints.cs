using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AutoMoveBetweenTwoPoints : MonoBehaviour
{

    public Vector3 leftmostPosition;
    // Configurable values.
    public Vector3 rightmostPosition;
    public bool moveOnX;
    public bool moveOnY;
    public float velocityOnX;
    public float velocityOnY;
    private bool initialMovement = true;
    private Vector3 _startPosition;
    private Vector2 _startPositionWithOffset;
    private Rigidbody2D _physicsBody;


    void Start()
    {
        _startPosition = transform.position;
        _startPositionWithOffset.x = _startPosition.x - leftmostPosition.x;
        _startPositionWithOffset.y = _startPosition.y - leftmostPosition.y;
        _physicsBody = GetComponent<Rigidbody2D>();
        _physicsBody.gravityScale = 0.0f;
    }

    void Update()
    {
        if (moveOnX)
        {
            float newXPosition = _startPositionWithOffset.x + Time.time * velocityOnX;
            transform.position = new Vector3(leftmostPosition.x + Mathf.PingPong(newXPosition, rightmostPosition.x), transform.position.y, transform.position.z);
        }
        if (moveOnY)
        {
            float newYPosition = _startPositionWithOffset.y + Time.time * velocityOnY;
            transform.position = new Vector3(transform.position.x, leftmostPosition.y + Mathf.PingPong(newYPosition, rightmostPosition.y), transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Make player stay on platform.
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = null;
        }
    }

}
