using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Rigidbody2D toggledBody;
    public Camera toggledCamera;

    // Update is called once per frame
    void Update()
    {
        var position = toggledBody.position;
        toggledCamera.transform.position = new Vector3(position.x, position.y, toggledCamera.transform.position.z);
    }
}