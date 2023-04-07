using Models;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public CameraManager cameraManager;
    public BikeBuilder bikeBuilder;
    public UserInputManager userInputManager;

    private Bike bike;

    private void Start()
    {
        CreateNewBike();
    }

    public void CreateNewBike()
    {
        if (bike is not null)
        {
            Destroy(bike.GameObject);
        }

        bike = bikeBuilder.Build();
        cameraManager.toggledBody = bike.Rider.GetComponentInChildren<Rigidbody2D>();
        userInputManager.wheelJoint = bike.BackWheel.GetComponentInChildren<WheelJoint2D>();
        userInputManager.wheel = bike.BackWheel.GetComponentInChildren<Rigidbody2D>();
    }

    
}
