using Unity.Mathematics;
using UnityEngine;

namespace UserControl
{
    [RequireComponent(typeof(WheelJoint2D), typeof(Rigidbody2D))]
    public class MotorWheelControl : MonoBehaviour
    {
        public float speed;
        public float force;

        public string axis;

        private WheelJoint2D _wheelMotor;
        private Rigidbody2D _wheelRigidbody;

        private void Start()
        {
            _wheelMotor = GetComponent<WheelJoint2D>();
            _wheelRigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var input = Input.GetAxis(axis);
            if (input < math.EPSILON)
            {
                _wheelMotor.useMotor = false;
                return;
            }

            input *= speed;
            if (input > 0
                && input > -_wheelRigidbody.angularVelocity)
            {
                var motor = new JointMotor2D
                {
                    motorSpeed = input,
                    maxMotorTorque = force
                };

                _wheelMotor.motor = motor;
                _wheelMotor.useMotor = true;
            }
        }
    }
}
