using UnityEngine;

namespace Configuration
{
    public class BikeConfiguration : MonoBehaviour
    {
        public FrameConfiguration frame;
        public WheelConfiguration backWheel;
        public WheelConfiguration frontWheel;
        public TransmissionConfiguration transmission;
        public RiderConfiguration rider;

        public float backBrakeForce;
        public float frontBrakeForce;
    }
}