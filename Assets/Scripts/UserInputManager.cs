using Configuration;
using Models;
using Unity.Mathematics;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public BikeConfiguration configuration;
    [Tooltip("Scene manager, that provides creation of controlled bike")]
    public SceneManager sceneManager;

    public float maxSlowDownRatio;

    private Bike Bike => sceneManager.Bike;

    private void Update()
    {
        if (Bike is not null)
        {
            HandleForwardRide(Input.GetAxis("Ride"));
            HandleBreaking(Input.GetAxis("Breaking"));
            HandleFootwork(Input.GetAxis("Footwork"));
            HandleHandwork(Input.GetAxis("Handwork"));
        }

        if (Input.GetButtonUp("Submit"))
        {
            sceneManager.CreateNewBike();
        }

        var ratio = Input.GetAxis("Slowdown");
        Time.timeScale = 1 + ratio * (1 / maxSlowDownRatio - 1);
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
        if (force > 0)
        {
            BreakWheel(Bike.BackWheel.GetComponentInChildren<Rigidbody2D>(), force, configuration.backBrakeForce);
            BreakWheel(Bike.FrontWheel.GetComponentInChildren<Rigidbody2D>(), force, configuration.frontBrakeForce);   
        }
    }

    private void HandleFootwork(float deviation)
    {
        Bike.Rider.footsJoint.distance = deviation switch
        {
            > math.EPSILON => configuration.rider.foots.attackLength + (configuration.rider.foots.maxLength - configuration.rider.foots.attackLength) * deviation,
            < math.EPSILON => configuration.rider.foots.attackLength + (configuration.rider.foots.attackLength - configuration.rider.foots.minLength) * deviation,
            _ => configuration.rider.foots.attackLength
        };
    }

    private void HandleHandwork(float deviation)
    {
        Bike.Rider.handsJoint.distance = deviation switch
        {
            > math.EPSILON => configuration.rider.hands.attackLength + (configuration.rider.hands.maxLength - configuration.rider.hands.attackLength) * deviation,
            < math.EPSILON => configuration.rider.hands.attackLength + (configuration.rider.hands.attackLength - configuration.rider.hands.minLength) * deviation,
            _ => configuration.rider.hands.attackLength
        };
    }

    private static void BreakWheel(Rigidbody2D wheel, float force, float breakForce)
    {
        var direction = math.sign(wheel.angularVelocity);
        var abs = math.abs(wheel.angularVelocity);

        wheel.angularVelocity = direction * math.max(0, abs - force * breakForce);
    }
}
