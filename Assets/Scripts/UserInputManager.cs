using Configuration;
using Models;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public BikeConfiguration configuration;
    [Tooltip("Scene manager, that provides creation of controlled bike")]
    public SceneManager sceneManager;

    public Bike bike;

    private void Update()
    {
        if (bike is not null)
        {
            HandleRightPress(Input.GetAxis("Horizontal"));   
        }

        var inputEnter = Input.GetButtonUp("Submit");
        if (inputEnter)
        {
            bike = sceneManager.CreateNewBike();
        }
    }

    private void HandleRightPress(float input)
    {
        if (input < 0.001f)
        {
            bike.TransmissionJoint.useMotor = false;
            return;
        }

        input *= configuration.transmission.speed;
        if (input > 0
            && input > -bike.BackWheel.GetComponentInChildren<Rigidbody2D>().angularVelocity)
        {
            var motor = new JointMotor2D
            {
                motorSpeed = input,
                maxMotorTorque = configuration.transmission.force
            };

            bike.TransmissionJoint.motor = motor;
            bike.TransmissionJoint.useMotor = true;
        }
    }
}
