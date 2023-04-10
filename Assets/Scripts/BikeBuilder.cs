using System.Linq;
using Configuration;
using Models;
using Unity.Mathematics;
using UnityEngine;

public class BikeBuilder : MonoBehaviour
{
    public BikeConfiguration bikeConfiguration;

    public GameObject wheelPrefab;
    public GameObject framePrefab;
    public GameObject riderPrefab;

    public GameObject spawnPosition;

    public Bike Build()
    {
        var (frame, bottomBracket, steerHub, bar) = CreateFrame();
        var frameRigidbody = frame.GetComponent<Rigidbody2D>();

        var backWheel = CreateBackWheel(frameRigidbody);
        var frontWheel = CreateFrontWheel(frameRigidbody);
        var riderModel = CreateRider(frameRigidbody, bar, bottomBracket);

        return new Bike(frame, frame, backWheel, frontWheel, riderModel, bottomBracket, steerHub, bar);
    }

    private GameObject CreateBackWheel(Rigidbody2D connectedFrame)
    {
        var backWheel = Instantiate(wheelPrefab, connectedFrame.transform);

        backWheel.transform.localPosition = Vector3.zero;
        backWheel.transform.rotation = Quaternion.identity;
        backWheel.name = "Back Wheel";
        var wheelCollider = backWheel.GetComponent<CircleCollider2D>();
        backWheel.transform.localScale *= bikeConfiguration.backWheel.diameter / wheelCollider.radius / 2;

        var joint = backWheel.GetComponent<WheelJoint2D>();
        var suspension = new JointSuspension2D
        {
            frequency = bikeConfiguration.backWheel.tireFrequency,
            dampingRatio = bikeConfiguration.backWheel.tireDampingRatio
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

        frontWheel.transform.localPosition = new Vector3(bikeConfiguration.frame.wheelBase, 0);
        frontWheel.transform.rotation = Quaternion.identity;
        frontWheel.name = "Front Wheel";
        var wheelCollider = frontWheel.GetComponent<CircleCollider2D>();
        frontWheel.transform.localScale *= bikeConfiguration.frontWheel.diameter / wheelCollider.radius / 2;

        var joint = frontWheel.GetComponent<WheelJoint2D>();
        var suspension = new JointSuspension2D
        {
            frequency = bikeConfiguration.frontWheel.tireFrequency,
            dampingRatio = bikeConfiguration.frontWheel.tireDampingRatio
        };
        joint.suspension = suspension;

        var forwardJoint = frontWheel.GetComponent<WheelJoint2D>();
        forwardJoint.connectedBody = connectedFrame;
        forwardJoint.connectedAnchor = frontWheel.transform.localPosition;

        return frontWheel;
    }

    private (GameObject frame, Vector3 bottomBracket, Vector3 steerHub, Vector3 bar) CreateFrame()
    {
        var frame = Instantiate(framePrefab);

        frame.transform.position = spawnPosition.transform.position;
        frame.transform.rotation = Quaternion.identity;
        frame.name = "Frame";

        var frameCollider = frame.GetComponent<PolygonCollider2D>();

        var configuration = bikeConfiguration.frame;

        var frontWheel = new Vector2(configuration.wheelBase, 0);
        var bottomBracket = new Vector2(configuration.chainStay, -configuration.bottomBracketDrop);
        var steerHub = frontWheel + new Vector2(
            -configuration.forkLength * math.cos(configuration.headAngle * Mathf.Deg2Rad),
            configuration.forkLength * math.sin(configuration.headAngle * Mathf.Deg2Rad));
        var bar = steerHub + new Vector2(
            configuration.stemLength * math.sin(configuration.headAngle * Mathf.Deg2Rad),
            configuration.stemLength * math.cos(configuration.headAngle * Mathf.Deg2Rad));

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

    private Models.Rider CreateRider(Rigidbody2D connectedFrame, Vector3 bar, Vector3 bottomBracket)
    {
        var riderObject = Instantiate(riderPrefab, connectedFrame.transform);
        riderObject.name = "Rider";
        riderObject.transform.localPosition = bar - new Vector3(bikeConfiguration.rider.hands.attackLength, 0, 0);

        var riderModel = riderObject.GetComponent<Models.Rider>();

        riderModel.footsJoint.autoConfigureDistance = false;
        riderModel.footsJoint.distance = bikeConfiguration.rider.foots.attackLength;
        riderModel.footsJoint.frequency = bikeConfiguration.rider.foots.frequency;
        riderModel.footsJoint.dampingRatio = bikeConfiguration.rider.foots.dampingRatio;

        riderModel.footsConnectionJoint.autoConfigureConnectedAnchor = false;
        riderModel.footsConnectionJoint.connectedBody = connectedFrame;
        riderModel.footsConnectionJoint.connectedAnchor = bottomBracket;

        riderModel.handsJoint.autoConfigureDistance = false;
        riderModel.handsJoint.distance = bikeConfiguration.rider.hands.attackLength;
        riderModel.handsJoint.frequency = bikeConfiguration.rider.hands.frequency;
        riderModel.handsJoint.dampingRatio = bikeConfiguration.rider.hands.dampingRatio;

        riderModel.handsConnectionJoint.autoConfigureConnectedAnchor = false;
        riderModel.handsConnectionJoint.connectedBody = connectedFrame;
        riderModel.handsConnectionJoint.connectedAnchor = bar;

        return riderModel;
    }
}