using System.Linq;
using Configuration;
using Fabrics;
using Interfaces;
using Prefabs.BikeComponents;
using Unity.Mathematics;
using UnityEngine;

namespace Prefabs.BikeFabrics.Hardtail
{
    public class HardtailBikeFabric : BikeFabric
    {
        public GameObject framePrefab;
        public GameObject backWheelPrefab;
        public GameObject frontWheelPrefab;
        public GameObject forkWheelPrefab;

        public BikeConfiguration configuration;

        public override IBike BuildBike()
        {
            var (frame, bottomBracket, headTubeDown, bar) = CreateFrame();
            var frameRigidbody = frame.GetComponent<Rigidbody2D>();

            var fork = CreateFork(frameRigidbody, headTubeDown)
                .GetComponent<Fork>();

            CreateBackWheel(frameRigidbody);
            CreateFrontWheel(fork.lowerBody, fork.WheelConnectedPosition);

            return new HardtailBike(frame, frame.GetComponent<Rigidbody2D>(), bar, bottomBracket);
        }

        private (GameObject frame, Vector3 bottomBracket, Vector3 steerHub, Vector3 bar) CreateFrame()
        {
            var frame = Instantiate(framePrefab);

            frame.transform.rotation = Quaternion.identity;
            frame.name = "Bike";

            var frameCollider = frame.GetComponent<PolygonCollider2D>();

            var frameConfiguration = configuration.frame;

            var frontWheel = new Vector2(frameConfiguration.wheelBase, 0);
            var bottomBracket = new Vector2(frameConfiguration.chainStay, -frameConfiguration.bottomBracketDrop);
            var headTubeDown = frontWheel + new Vector2(
                -frameConfiguration.forkLength * math.cos(frameConfiguration.headAngle * Mathf.Deg2Rad),
                frameConfiguration.forkLength * math.sin(frameConfiguration.headAngle * Mathf.Deg2Rad));
            var headTubeUp = headTubeDown + new Vector2(
                -frameConfiguration.headTubeLength * math.cos(frameConfiguration.headAngle * Mathf.Deg2Rad),
                frameConfiguration.headTubeLength * math.sin(frameConfiguration.headAngle * Mathf.Deg2Rad));
            var bar = headTubeUp + new Vector2(
                frameConfiguration.stemLength * math.sin(frameConfiguration.headAngle * Mathf.Deg2Rad),
                frameConfiguration.stemLength * math.cos(frameConfiguration.headAngle * Mathf.Deg2Rad));

            frameCollider.points = new[]
            {
                Vector2.zero,
                headTubeUp,
                bar,
                headTubeUp,
                headTubeDown,
                bottomBracket
            };

            var lineRenderer = frame.GetComponent<LineRenderer>();
            lineRenderer.positionCount = frameCollider.points.Length;
            lineRenderer.loop = true;
            lineRenderer.SetPositions(frameCollider.points
                .Select(point => new Vector3(point.x, point.y, 0))
                .ToArray());

            return (frame, bottomBracket, headTubeDown, bar);
        }

        private GameObject CreateBackWheel(Rigidbody2D connectedFrame)
        {
            var backWheel = Instantiate(backWheelPrefab, connectedFrame.transform);

            backWheel.transform.localPosition = Vector3.zero;
            backWheel.transform.rotation = Quaternion.identity;
            backWheel.name = "Back Wheel";
            var wheelCollider = backWheel.GetComponent<CircleCollider2D>();
            backWheel.transform.localScale *= configuration.backWheel.diameter / wheelCollider.radius / 2;

            var joint = backWheel.GetComponent<WheelJoint2D>();
            var suspension = new JointSuspension2D
            {
                frequency = configuration.backWheel.tireFrequency,
                dampingRatio = configuration.backWheel.tireDampingRatio
            };
            joint.suspension = suspension;

            var backJoint = backWheel.GetComponent<WheelJoint2D>();
            backJoint.connectedBody = connectedFrame;
            backJoint.connectedAnchor = backWheel.transform.localPosition;

            return backWheel;
        }

        private GameObject CreateFrontWheel(Rigidbody2D connectedBody, Vector2 connectionPosition)
        {
            var frontWheel = Instantiate(frontWheelPrefab, connectedBody.transform);

            frontWheel.transform.localPosition = connectionPosition;
            frontWheel.transform.rotation = Quaternion.identity;
            frontWheel.name = "Front Wheel";
            var wheelCollider = frontWheel.GetComponent<CircleCollider2D>();
            frontWheel.transform.localScale *= configuration.frontWheel.diameter / wheelCollider.radius / 2;

            var joint = frontWheel.GetComponent<WheelJoint2D>();
            var suspension = new JointSuspension2D
            {
                frequency = configuration.frontWheel.tireFrequency,
                dampingRatio = configuration.frontWheel.tireDampingRatio
            };
            joint.suspension = suspension;

            var forwardJoint = frontWheel.GetComponent<WheelJoint2D>();
            forwardJoint.connectedBody = connectedBody;
            forwardJoint.connectedAnchor = frontWheel.transform.localPosition;

            return frontWheel;
        }

        private GameObject CreateFork(Rigidbody2D connectedFrame, Vector2 connectionPosition)
        {
            var fork = Instantiate(forkWheelPrefab, connectedFrame.transform);

            fork.transform.localPosition = connectionPosition;
            fork.transform.rotation = Quaternion.identity;
            fork.name = "Fork";

            var connection = fork.GetComponent<HingeJoint2D>();
            connection.connectedAnchor = connectionPosition;
            connection.connectedBody = connectedFrame;
            connection.limits = new JointAngleLimits2D()
            {
                min = -90 + configuration.frame.headAngle,
                max = -90 + configuration.frame.headAngle,
            };

            return fork;
        }
    }

    internal class HardtailBike : IBike
    {
        private readonly GameObject _frameObject;
        private readonly Rigidbody2D _frameRigidbody;
        private readonly Vector2 _barPosition;
        private readonly Vector2 _pedalsPosition;

        public HardtailBike(GameObject frameObject, Rigidbody2D frameRigidbody, Vector2 barPosition, Vector2 pedalsPosition)
        {
            _frameObject = frameObject;
            _frameRigidbody = frameRigidbody;
            _barPosition = barPosition;
            _pedalsPosition = pedalsPosition;

            Transform = frameObject.transform;
        }

        public void Destroy()
        {
            Object.Destroy(_frameObject);
        }

        public Transform Transform { get; }

        public (Rigidbody2D connectedBody, Vector2 anchorPosition) GetConnectionWithBar()
        {
            return (_frameRigidbody, _barPosition);
        }

        public (Rigidbody2D connectedBody, Vector2 anchorPosition) GetConnectionWithPedals()
        {
            return (_frameRigidbody, _pedalsPosition);
        }
    }
}
