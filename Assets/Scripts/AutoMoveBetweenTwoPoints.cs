using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveBetweenTwoPoints : MonoBehaviour
{

    // Configurable values.
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float velocity;
    private bool movingToEnd = true;
    private bool movingUpwards = true;
    private Vector2 nullVector = new Vector2();


    void Start()
    {
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (movingToEnd)
        {
            // Could be made to take Z-axis into account.
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(endPosition.x, endPosition.y, transform.position.z), Time.deltaTime * velocity);
            if (transform.position == endPosition) movingToEnd = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPosition.x, startPosition.y, transform.position.z), Time.deltaTime * velocity);
            if (transform.position == startPosition) movingToEnd = true;
        }
    }

}
