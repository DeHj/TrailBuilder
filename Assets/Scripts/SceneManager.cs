using Models;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public CameraManager cameraManager;
    [Tooltip("Bike builder, that creates bike model")]
    public BikeBuilder bikeBuilder;
    public SegmentRenderer segmentRenderer;
    public SegmentGenerator segmentGenerator;

    public Bike Bike { get; private set; }

    private void Start()
    {
        CreateNewBike();

        if (segmentGenerator.enabled)
        {
            segmentGenerator.Generate();
        }

        if (segmentRenderer.enabled)
        {
            segmentRenderer.Render();
        }
    }

    public void CreateNewBike()
    {
        if (Bike is not null)
        {
            Destroy(Bike.GameObject);
        }

        Bike = bikeBuilder.Build();
        cameraManager.toggledBody = Bike.Rider.GetComponentInChildren<Rigidbody2D>();
    }
}
