using Models;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public CameraManager cameraManager;
    [Tooltip("Bike builder, that creates bike model")]
    public BikeBuilder bikeBuilder;

    public Bike bike;

    private void Start()
    {
        CreateNewBike();
    }

    public Bike CreateNewBike()
    {
        if (bike is not null)
        {
            Destroy(bike.GameObject);
        }

        bike = bikeBuilder.Build();
        cameraManager.toggledBody = bike.Rider.GetComponentInChildren<Rigidbody2D>();

        return bike;
    }
}
