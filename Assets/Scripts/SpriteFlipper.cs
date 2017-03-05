using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteFlipper))]
public class SpriteFlipper : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;
    private bool facingRight = true;
    private string playerTag = "Player";
    private Rigidbody2D _physicsBody;
    public bool horizontalFlipEnabled = false;
    public bool verticalFlipEnabled = true;

    // Use this for initialization
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _physicsBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontalFlipEnabled)
        {
            // Check if tag corresponds to player
            if (CompareTag(playerTag))
            {
                if ((facingRight && Input.GetAxis("Horizontal") < 0.0f) || (!facingRight && Input.GetAxis("Horizontal") > 0.0f))
                {
                    facingRight = !facingRight;
                    _spriteRenderer.flipX = !_spriteRenderer.flipX;
                }
            }
        }
        // Check flip Y according to gravity scale
        if (verticalFlipEnabled && _physicsBody != null)
        {
            if (_physicsBody.gravityScale < 0.0f) _spriteRenderer.flipY = true;
            else _spriteRenderer.flipY = false;
        }
    }
}
