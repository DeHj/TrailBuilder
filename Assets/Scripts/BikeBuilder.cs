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
        var bike = new GameObject("Bike");
        bike.transform.position = spawnPosition.transform.position;

        var frame = Instantiate(framePrefab, bike.transform);
        var (bottomBracket, steerHub, bar) = ConfigureFrame(frame);
        
        var backWheel = Instantiate(wheelPrefab, bike.transform);
        ConfigureBackWheel(backWheel);

        var frontWheel = Instantiate(wheelPrefab, bike.transform);
        ConfigureFrontWheel(frontWheel);

        var frameRigidbody = frame.GetComponentInChildren<Rigidbody2D>();

        var backJoint = backWheel.GetComponentInChildren<WheelJoint2D>();
        backJoint.connectedBody = frameRigidbody;
        backJoint.connectedAnchor = backWheel.transform.localPosition;

        var forwardJoint = frontWheel.GetComponentInChildren<WheelJoint2D>();
        forwardJoint.connectedBody = frameRigidbody;
        forwardJoint.connectedAnchor = frontWheel.transform.localPosition;

        var riderObject = Instantiate(riderPrefab, bike.transform);
        riderObject.transform.localPosition = bar - new Vector3(bikeConfiguration.rider.hands.attackLength, 0, 0);

        var riderModel = riderObject.GetComponent<RiderModel>();

        riderModel.footsJoint.connectedBody = frameRigidbody;
        riderModel.footsJoint.connectedAnchor = bottomBracket;
        riderModel.footsJoint.autoConfigureDistance = false;
        riderModel.footsJoint.distance = bikeConfiguration.rider.foots.attackLength;
        riderModel.footsJoint.frequency = bikeConfiguration.rider.foots.frequency;
        riderModel.footsJoint.dampingRatio = bikeConfiguration.rider.foots.dampingRatio;

        riderModel.handsJoint.connectedBody = frameRigidbody;
        riderModel.handsJoint.connectedAnchor = bar;
        riderModel.handsJoint.autoConfigureDistance = false;
        riderModel.handsJoint.distance = bikeConfiguration.rider.hands.attackLength;
        riderModel.handsJoint.frequency = bikeConfiguration.rider.hands.frequency;
        riderModel.handsJoint.dampingRatio = bikeConfiguration.rider.hands.dampingRatio;

        return new Bike(bike, frame, backWheel, frontWheel, riderObject, riderModel.footsJoint, bottomBracket, steerHub, bar);
    }

    private void ConfigureBackWheel(GameObject backWheel)
    {
        backWheel.transform.localPosition = Vector3.zero;
        backWheel.transform.rotation = Quaternion.identity;
        backWheel.name = "Back Wheel";
        var wheelCollider = backWheel.GetComponentInChildren<CircleCollider2D>();
        backWheel.transform.localScale *= bikeConfiguration.backWheel.diameter / wheelCollider.radius / 2;

        var joint = backWheel.GetComponentInChildren<WheelJoint2D>();
        var suspension = new JointSuspension2D
        {
            frequency = bikeConfiguration.backWheel.tireFrequency,
            dampingRatio = bikeConfiguration.backWheel.tireDampingRatio
        };
        joint.suspension = suspension;
    }

    private void ConfigureFrontWheel(GameObject frontWheel)
    {
        frontWheel.transform.localPosition = new Vector3(bikeConfiguration.frame.wheelBase, 0);
        frontWheel.transform.rotation = Quaternion.identity;
        frontWheel.name = "Front Wheel";
        var wheelCollider = frontWheel.GetComponentInChildren<CircleCollider2D>();
        frontWheel.transform.localScale *= bikeConfiguration.frontWheel.diameter / wheelCollider.radius / 2;

        var joint = frontWheel.GetComponentInChildren<WheelJoint2D>();
        var suspension = new JointSuspension2D
        {
            frequency = bikeConfiguration.frontWheel.tireFrequency,
            dampingRatio = bikeConfiguration.frontWheel.tireDampingRatio
        };
        joint.suspension = suspension;
    }

    private (Vector3 bottomBracket, Vector3 steerHub, Vector3 bar) ConfigureFrame(GameObject frame)
    {
        frame.transform.localPosition = Vector3.zero;
        frame.transform.rotation = Quaternion.identity;
        frame.name = "Frame";

        var frameCollider = frame.GetComponentInChildren<PolygonCollider2D>();

        var configuration = bikeConfiguration.frame;

        var frontWheel = new Vector2(configuration.wheelBase, 0);
        var bottomBracket = new Vector2(configuration.chainStay, -configuration.bottomBracketDrop);
        var steerHub = new Vector2(
            frontWheel.x - configuration.forkLength * math.cos(configuration.headAngle * Mathf.Deg2Rad),
            frontWheel.y + configuration.forkLength * math.sin(configuration.headAngle * Mathf.Deg2Rad));
        var bar = new Vector2(
            steerHub.x + configuration.stemLength * math.sin(configuration.headAngle * Mathf.Deg2Rad),
            steerHub.y + configuration.stemLength * math.cos(configuration.headAngle * Mathf.Deg2Rad));

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

        var lineRenderer = frame.GetComponentInChildren<LineRenderer>();
        lineRenderer.positionCount = frameCollider.points.Length;
        lineRenderer.loop = true;
        lineRenderer.SetPositions(frameCollider.points
            .Select(point => new Vector3(point.x, point.y, 0))
            .ToArray());

        return (bottomBracket, steerHub, bar);
    }
}