using Models;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public CameraManager cameraManager;
    [Tooltip("Bike builder, that creates bike model")]
    public BikeBuilder bikeBuilder;
    public UserInputManager userInputManager;

    private Bike _bike;

    private void Start()
    {
        userInputManager.bike = CreateNewBike();
    }

    public Bike CreateNewBike()
    {
        if (_bike is not null)
        {
            Destroy(_bike.GameObject);
        }

        _bike = bikeBuilder.Build();
        cameraManager.toggledBody = _bike.Rider.GetComponentInChildren<Rigidbody2D>();

        return _bike;
    }
}
