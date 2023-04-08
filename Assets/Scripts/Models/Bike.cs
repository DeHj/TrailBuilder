using UnityEngine;

namespace Models
{
    public class Bike
    {
        public Bike(
            GameObject gameObject,
            GameObject frame,
            GameObject backWheel,
            GameObject frontWheel,
            Rider rider,
            Vector3 bottomBracket,
            Vector3 steerHub,
            Vector3 bar)
        {
            GameObject = gameObject;
            Frame = frame;
            BackWheel = backWheel;
            FrontWheel = frontWheel;
            Rider = rider;
            BottomBracket = bottomBracket;
            SteerHub = steerHub;
            Bar = bar;
        }

        public GameObject GameObject { get; }
        public GameObject Frame { get; }
        public GameObject FrontWheel { get; }
        public GameObject BackWheel { get; }
        public WheelJoint2D TransmissionJoint => BackWheel.GetComponentInChildren<WheelJoint2D>();
        public Rider Rider { get; }
        public Vector3 BackWheelPosition => BackWheel.transform.localPosition;
        public Vector3 FrontWheelPosition => FrontWheel.transform.localPosition;
        public Vector3 BottomBracket { get; }
        public Vector3 SteerHub { get; }
        public Vector3 Bar { get; }
    }
}