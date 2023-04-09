using Models;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public CameraManager cameraManager;
    [Tooltip("Bike builder, that creates bike model")]
    public BikeBuilder bikeBuilder;

    public Bike Bike { get; private set; }

    private void Start()
    {
        CreateNewBike();
    }

    public void CreateNewBike()
    {
        if (Bike is not null)
        {
            Destroy(Bike.GameObject);
        }

        Bike = bikeBuilder.Build();
        cameraManager.toggledBody = Bike.Rider.cameraFollowedObject;
    }
}