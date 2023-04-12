using System;

namespace Configuration
{
    [Serializable]
    public class ForkConfiguration
    {
        public float length;
        public float travel;

        public float dampingRatio;
        public float frequency;
        public float springLength;

        public float LowerLength => length - travel;
    }
}