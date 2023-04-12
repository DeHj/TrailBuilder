using Unity.Mathematics;
using UnityEngine;

namespace UserControl
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BrakeControl : MonoBehaviour
    {
        public float force;

        public string axis;

        private Rigidbody2D _brakedRigidBody;

        private void Start()
        {
            _brakedRigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var inputForce = Input.GetAxis(axis) * force;
            if (inputForce > 0)
            {
                var direction = math.sign(_brakedRigidBody.angularVelocity);
                var abs = math.abs(_brakedRigidBody.angularVelocity);

                _brakedRigidBody.angularVelocity = direction * math.max(0, abs - force * force);
            }
        }
    }
}
