using System.Linq;
using Configuration;
using Fabrics;
using Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace Prefabs.BikeFabrics.Rigid
{
    public class RigidBikeFabric : BikeFabric
    {
        public GameObject framePrefab;
        public GameObject wheelPrefab;

        public BikeConfiguration configuration;

        public override IBike BuildBike()
        {
            var (frame, bottomBracket, _, bar) = CreateFrame();
            var frameRigidbody = frame.GetComponent<Rigidbody2D>();

            CreateBackWheel(frameRigidbody);
            CreateFrontWheel(frameRigidbody);

            return new RigidBike(frame, frame.GetComponent<Rigidbody2D>(), bar, bottomBracket);
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
            var steerHub = frontWheel + new Vector2(
                -frameConfiguration.forkLength * math.cos(frameConfiguration.headAngle * Mathf.Deg2Rad),
                frameConfiguration.forkLength * math.sin(frameConfiguration.headAngle * Mathf.Deg2Rad));
            var bar = steerHub + new Vector2(
                frameConfiguration.stemLength * math.sin(frameConfiguration.headAngle * Mathf.Deg2Rad),
                frameConfiguration.stemLength * math.cos(frameConfiguration.headAngle * Mathf.Deg2Rad));

            frameCollider.points = new[]
            {
                Vector2.zero,
                steerHub,
                bar,
                steerHub,
                frontWheel,
                steerHub,
                bottomBracket
            };

            var lineRenderer = frame.GetComponent<LineRenderer>();
            lineRenderer.positionCount = frameCollider.points.Length;
            lineRenderer.loop = true;
            lineRenderer.SetPositions(frameCollider.points
                .Select(point => new Vector3(point.x, point.y, 0))
                .ToArray());

            return (frame, bottomBracket, steerHub, bar);
        }

        private GameObject CreateBackWheel(Rigidbody2D connectedFrame)
        {
            var backWheel = Instantiate(wheelPrefab, connectedFrame.transform);

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

        private GameObject CreateFrontWheel(Rigidbody2D connectedFrame)
        {
            var frontWheel = Instantiate(wheelPrefab, connectedFrame.transform);

            frontWheel.transform.localPosition = new Vector3(configuration.frame.wheelBase, 0);
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
            forwardJoint.connectedBody = connectedFrame;
            forwardJoint.connectedAnchor = frontWheel.transform.localPosition;

            return frontWheel;
        }
    }

    internal class RigidBike : IBike
    {
        private readonly GameObject _frameObject;
        private readonly Rigidbody2D _frameRigidbody;
        private readonly Vector2 _barPosition;
        private readonly Vector2 _pedalsPosition;

        public RigidBike(GameObject frameObject, Rigidbody2D frameRigidbody, Vector2 barPosition, Vector2 pedalsPosition)
        {
            _frameObject = frameObject;
            _frameRigidbody = frameRigidbody;
            _barPosition = barPosition;
            _pedalsPosition = pedalsPosition;
        }

        public void Destroy()
        {
            Object.Destroy(_frameObject);
        }

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
