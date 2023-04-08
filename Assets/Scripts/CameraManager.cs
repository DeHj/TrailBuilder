using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Rigidbody2D toggledBody;
    public Camera toggledCamera;
    public float offsetX;
    public float offsetY;

    // Update is called once per frame
    void Update()
    {
        var position = toggledBody.position;

        toggledCamera.transform.position = new Vector3(position.x + offsetX, position.y + offsetY, toggledCamera.transform.position.z);
    }
}