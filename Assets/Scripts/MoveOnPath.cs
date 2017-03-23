using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour
{

    public EditorPathScript pathToFollow;
    public int currentWayPointId = 0;
    public float speed;
    private float reachDistance = 0.0f;
    public float rotationSpeed = 5.0f;
    public string pathName;
    Vector3 lastPosition;
    Vector3 currentPosition;
    enum PathFollowingType { GoBackToBeginning, PingPong }
    private PathFollowingType pathFollowingType = PathFollowingType.PingPong;
    private bool goingForward = true;


    // Use this for initialization
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(pathToFollow.pathObjects[currentWayPointId].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, pathToFollow.pathObjects[currentWayPointId].position, Time.deltaTime * speed);

        if (distance <= reachDistance) moveWithinPath();

        switch (pathFollowingType)
        {
            case PathFollowingType.GoBackToBeginning:
                if (currentWayPointId >= pathToFollow.pathObjects.Count)
                    currentWayPointId = 0;
                break;
            case PathFollowingType.PingPong:
                bool conditionA = currentWayPointId >= pathToFollow.pathObjects.Count;
                bool conditionB = currentWayPointId <= 0;
                if (conditionA || conditionB)
                    goingForward = !goingForward;
                if (conditionA) currentWayPointId = pathToFollow.pathObjects.Count - 1;
                if (conditionB) currentWayPointId = 0;
                break;
            default:
                break;
        }
    }

    private void moveWithinPath()
    {
        if (goingForward) currentWayPointId++;
        else currentWayPointId--;
    }
}
