using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Rigidbody2D toggledBody;
    public Camera toggledCamera;
    public float offsetX;
    public float offsetY;

    private Vector3 CameraPosition => toggledCamera.transform.position;

    public float minSize;
    public float maxSize;
    public float zoomStep;

    private void Update()
    {
        var position = toggledBody.position;
        toggledCamera.transform.position = new Vector3(position.x + offsetX, position.y + offsetY, CameraPosition.z);
    }

    public void ZoomIn()
    {
        if (toggledCamera.orthographicSize > minSize)
        {
            toggledCamera.orthographicSize /= zoomStep;
        }
    }

    public void ZoomOut()
    {
        if (toggledCamera.orthographicSize < maxSize)
        {
            toggledCamera.orthographicSize *= zoomStep;
        }
    }
}