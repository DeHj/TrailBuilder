using Interfaces;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof (Camera))]
public class CameraManager : MonoBehaviour
{
    private ICameraTraceable ToggledBody { get; set; }
    public float offsetX;
    public float offsetY;

    private Camera _toggledCamera;

    private Vector3 CameraPosition
    {
        get => _toggledCamera.transform.position;
        set => _toggledCamera.transform.position = value;
    }

    public float minSize;
    public float maxSize;
    public float zoomStep;

    private void Start()
    {
        _toggledCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        CameraPosition = (Vector3)ToggledBody.GetPosition() + new Vector3(offsetX, offsetY, CameraPosition.z);

        HandleZoom();
    }

    private void HandleZoom()
    {
        if (Input.GetButtonUp("Camera Zoom"))
        {
            var input = Input.GetAxis("Camera Zoom");
            if (input > math.EPSILON)
            {
                ZoomIn();
            }
            else if (input < -math.EPSILON)
            {
                ZoomOut();
            }
        }
    }

    public void ZoomIn()
    {
        if (_toggledCamera.orthographicSize > minSize)
        {
            _toggledCamera.orthographicSize /= zoomStep;
        }
    }

    public void ZoomOut()
    {
        if (_toggledCamera.orthographicSize < maxSize)
        {
            _toggledCamera.orthographicSize *= zoomStep;
        }
    }

    public void SetToggledObject(ICameraTraceable toggledBody)
    {
        ToggledBody = toggledBody;
    }
}