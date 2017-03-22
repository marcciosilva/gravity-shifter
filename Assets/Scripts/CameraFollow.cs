using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    /**
     * Objects with "Boundary" tag should be defined (gotta have Transform component) if the Camera's movement
     * is meant to be limited.
     * Camera position is lerped with player position (which is obtained through a search for "Player" tag).
     * */

    private Transform _playerTransform;
    private string _playerTag = "Player";
    private string _boundaryTag = "Boundary";
    // Camera limits
    private Vector4 camLimits; // minX, maxX, minY, maxY.
    private const int PIXELS_PER_UNIT = 32;
    public float camLerp = 4.0f;
    private float halfScreenWidth;
    private float halfScreenHeight;
    private bool _fixedVirtualResolution = true;

    // Use this for initialization.
    void Start()
    {
        if (_fixedVirtualResolution)
        {
            halfScreenWidth = 640 / 2.0f / PIXELS_PER_UNIT;
            halfScreenHeight = 360 / 2.0f / PIXELS_PER_UNIT;
        }
        else
        {
            halfScreenWidth = Screen.width / 2.0f / PIXELS_PER_UNIT;
            halfScreenHeight = Screen.height / 2.0f / PIXELS_PER_UNIT;
        }
        // Get objects tagged as Boundary, and adjust camera movement
        // limits to them.
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag(_boundaryTag);
        camLimits = new Vector4();
        camLimits.x = Mathf.Infinity; // minX
        camLimits.y = -Mathf.Infinity; // maxX
        camLimits.z = Mathf.Infinity; // minY
        camLimits.w = -Mathf.Infinity; // maxY
        foreach (GameObject boundary in boundaries)
        {
            if (boundary.transform.position.x < camLimits.x)
            {
                camLimits.x = boundary.transform.position.x + halfScreenWidth;
            }
            if (boundary.transform.position.x > camLimits.y)
            {
                camLimits.y = boundary.transform.position.x - halfScreenWidth;
            }
            if (boundary.transform.position.y < camLimits.z)
            {
                camLimits.z = boundary.transform.position.y + halfScreenHeight;
            }
            if (boundary.transform.position.y > camLimits.w)
            {
                camLimits.w = boundary.transform.position.y - halfScreenHeight;
            }
        }
        GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    // Update is called once per frame.
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(Mathf.Clamp(_playerTransform.position.x, camLimits.x, camLimits.y), Mathf.Clamp(_playerTransform.position.y, camLimits.z, camLimits.w), this.transform.position.z),
                Time.deltaTime * camLerp);
        }
    }

}
