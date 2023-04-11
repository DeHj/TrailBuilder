using Fabrics;
using Interfaces;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public CameraManager cameraManager;
    [Tooltip("Bike fabric, that creates bike model")]
    public BikeFabric bikeFabric;
    [Tooltip("Rider fabric, that creates rider model")]
    public RiderFabric riderFabric;

    private IBike _bike;
    private IRider _rider;

    public Transform spawnPosition;

    private void Start()
    {
        BuildBikeAndRider();
    }

    private void Update()
    {
        if (Input.GetButtonUp("Submit"))
        {
            _bike?.Destroy();
            _rider?.Destroy();

            BuildBikeAndRider();
        }
    }

    private void BuildBikeAndRider()
    {
        _bike = bikeFabric.BuildBike();
        _rider = riderFabric.BuildRider();

        _bike.Transform.position = spawnPosition.position;
        _rider.Transform.position = spawnPosition.position + new Vector3(0.5f, 1.0f);

        var pedalsConnection = _bike.GetConnectionWithPedals();
        _rider.ConnectFoots(pedalsConnection.connectedBody, pedalsConnection.anchorPosition);

        var barConnection = _bike.GetConnectionWithBar();
        _rider.ConnectHands(barConnection.connectedBody, barConnection.anchorPosition);

        cameraManager.SetToggledObject(_rider);
    }
}