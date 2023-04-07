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
            GameObject rider,
            SpringJoint2D foots,
            Vector3 bottomBracket,
            Vector3 steerHub,
            Vector3 bar)
        {
            GameObject = gameObject;
            Frame = frame;
            BackWheel = backWheel;
            FrontWheel = frontWheel;
            Rider = rider;
            Foots = foots;
            BottomBracket = bottomBracket;
            SteerHub = steerHub;
            Bar = bar;
        }

        public GameObject GameObject { get; }
        public GameObject Frame { get; }
        public GameObject FrontWheel { get; }
        public GameObject BackWheel { get; }
        public GameObject Rider { get; }
        //public SpringJoint2D Hands { get; }
        public SpringJoint2D Foots { get; }
        public Vector3 BackWheelPosition => BackWheel.transform.localPosition;
        public Vector3 FrontWheelPosition => FrontWheel.transform.localPosition;
        public Vector3 BottomBracket { get; }
        public Vector3 SteerHub { get; }
        public Vector3 Bar { get; }
    }
}