using Configuration;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public WheelJoint2D wheelJoint;
    public Rigidbody2D wheel;
    public BikeConfiguration configuration;
    //[Tooltip()]
    public SceneManager sceneManager;

    private void Update()
    {
        HandleRightPress(Input.GetAxis("Horizontal"));

        var inputEnter = Input.GetButtonUp("Submit");
        if (inputEnter)
        {
            sceneManager.CreateNewBike();
        }v
    }

    private void HandleRightPress(float input)
    {
        input *= configuration.transmission.speed;
        if (input < 0.001f)
        {
            wheelJoint.useMotor = false;
            return;
        }

        if (input > 0
            && input > -wheel.angularVelocity)
        {
            var motor = new JointMotor2D
            {
                motorSpeed = input,
                maxMotorTorque = configuration.transmission.force
            };

            wheelJoint.motor = motor;
            wheelJoint.useMotor = true;
        }
    }
}
