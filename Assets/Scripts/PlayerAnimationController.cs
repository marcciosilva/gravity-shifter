﻿#undef DEBUG
using System.Collections;
using System.Collections.Generic;
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
    private int jumpState = Animator.StringToHash("Jumping");

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _physicsBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputController = GetComponent<MovementInputController>();

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

        if (_animator.GetBool("Jumping")) _animator.SetBool("Jumping", false);

        if (_inputController.isJumping)
        {
            _animator.SetBool("Jumping", true);
        }

        //if (Mathf.Abs(_physicsBody.velocity.y) > verticalVelocityThreshold)
        //{
        //    _animator.SetBool("Floating", true);
        //}
        //else
        //{
        //    _animator.SetBool("Floating", false);
        //}

        // Update sprite according to current gravity scale.
        if (_physicsBody.gravityScale >= 0.0f)
        {
            //_spriteRenderer.flipY = false;
            this.transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        } else
        {
            //_spriteRenderer.flipY = true;
            this.transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        }

    }
}
