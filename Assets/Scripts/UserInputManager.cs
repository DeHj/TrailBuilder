using Configuration;
using Models;
using Unity.Mathematics;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public BikeConfiguration configuration;
    [Tooltip("Scene manager, that provides creation of controlled bike")]
    public SceneManager sceneManager;

    private Bike Bike => sceneManager.bike;

    private void Update()
    {
        if (Bike is not null)
        {
            HandleForwardRide(Input.GetAxis("Horizontal"));
            HandleBreaking(Input.GetAxis("Vertical"));
        }

        if (Input.GetButtonUp("Submit"))
        {
            sceneManager.CreateNewBike();
        }
    }

    private void HandleForwardRide(float input)
    {
        if (input < 0.001f)
        {
            Bike.TransmissionJoint.useMotor = false;
            return;
        }

        input *= configuration.transmission.speed;
        if (input > 0
            && input > -Bike.BackWheel.GetComponentInChildren<Rigidbody2D>().angularVelocity)
        {
            var motor = new JointMotor2D
            {
                motorSpeed = input,
                maxMotorTorque = configuration.transmission.force
            };

            Bike.TransmissionJoint.motor = motor;
            Bike.TransmissionJoint.useMotor = true;
        }
    }

    private void HandleBreaking(float force)
    {
        force *= -1;
        if (force > 0)
        {
            BreakWheel(Bike.BackWheel.GetComponentInChildren<Rigidbody2D>(), force, configuration.backBrakeForce);
            BreakWheel(Bike.FrontWheel.GetComponentInChildren<Rigidbody2D>(), force, configuration.frontBrakeForce);   
        }
    }

    private static void BreakWheel(Rigidbody2D wheel, float force, float breakForce)
    {
        var direction = math.sign(wheel.angularVelocity);
        var abs = math.abs(wheel.angularVelocity);

        wheel.angularVelocity = direction * math.max(0, abs - force * breakForce);
    }
}
