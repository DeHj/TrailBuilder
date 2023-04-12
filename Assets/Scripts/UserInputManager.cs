using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public float maxSlowDownRatio;

    private void Update()
    {
        HandleSlowdown(Input.GetAxis("Slowdown"));
    }

    private void HandleSlowdown(float ratio)
    {
        Time.timeScale = 1 + ratio * (1 / maxSlowDownRatio - 1);
    }

    /*
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

    /*
    private static void BreakWheel(Rigidbody2D wheel, float force, float breakForce)
    {
        var direction = math.sign(wheel.angularVelocity);
        var abs = math.abs(wheel.angularVelocity);

        wheel.angularVelocity = direction * math.max(0, abs - force * breakForce);
    }*/
}
