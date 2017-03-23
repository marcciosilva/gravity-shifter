using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MakePlayerStick : MonoBehaviour {

    private Rigidbody2D _physicsBody;

    // Use this for initialization
    void Start () {
        _physicsBody = GetComponent<Rigidbody2D>();
        _physicsBody.gravityScale = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
		
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
