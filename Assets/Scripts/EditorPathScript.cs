using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPathScript : MonoBehaviour
{
    private const float wireSphereRadius = 0.3f;
    public Color rayColor = Color.white;
    public List<Transform> pathObjects = new List<Transform>();
    Transform[] pathObjectsWithParent;

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        pathObjectsWithParent = GetComponentsInChildren<Transform>();
        pathObjects.Clear();
        foreach (Transform pathObject in pathObjectsWithParent)
        {
            if (pathObject != this.transform)
            {
                pathObjects.Add(pathObject);
            }
        }

        for (int i = 0; i < pathObjects.Count; i++)
        {
            Vector3 current = pathObjects[i].position;
            if (i > 0)
            {
                Vector3 previous = pathObjects[i - 1].position;
                Gizmos.DrawLine(previous, current);
                Gizmos.DrawWireSphere(current, wireSphereRadius);
            }
            else
            {
                Gizmos.DrawWireSphere(current, wireSphereRadius);
            }
        }

    }

}
