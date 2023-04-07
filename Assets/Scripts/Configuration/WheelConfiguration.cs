using System;

namespace Configuration
{
    [Serializable]
    public class WheelConfiguration
    {
        public float diameter;
        public float tireHeight;

        public float tireDampingRatio;
        public float tireFrequency;
    }
}