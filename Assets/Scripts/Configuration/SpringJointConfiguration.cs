using System;

namespace Configuration
{
    [Serializable]
    public class SpringJointConfiguration
    {
        public float dampingRatio;
        public float frequency;
        public float maxLength;
        public float minLength;
        public float attackLength;
    }
}