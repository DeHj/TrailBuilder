using Unity.Mathematics;
using UnityEngine;

namespace UserControl
{
    [RequireComponent(typeof(SpringJoint2D))]
    public class SpringControl : MonoBehaviour
    {
        public float dampingRatio;
        public float frequency;
        public float maxLength;
        public float attackLength;
        public float minLength;

        public string axis;

        private SpringJoint2D _spring;

        private void Start()
        {
            _spring = GetComponent<SpringJoint2D>();
            _spring.dampingRatio = dampingRatio;
            _spring.frequency = frequency;
            _spring.autoConfigureDistance = false;
            _spring.distance = attackLength;
        }

        private void Update()
        {
            var input = Input.GetAxis(axis);
            _spring.distance = input switch
            {
                > math.EPSILON => attackLength + (maxLength - attackLength) * input,
                < -math.EPSILON => attackLength + (attackLength - minLength) * input,
                _ => attackLength
            };
        }
    }
}