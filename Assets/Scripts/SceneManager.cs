using Fabrics;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public CameraManager cameraManager;
    [Tooltip("Bike fabric, that creates bike model")]
    public BikeFabric bikeFabric;
    [Tooltip("Rider fabric, that creates rider model")]
    public RiderFabric riderFabric;

    private void Start()
    {
        var bike = bikeFabric.BuildBike();
        var rider = riderFabric.BuildRider();

        var pedalsConnection = bike.GetConnectionWithPedals();
        rider.ConnectFoots(pedalsConnection.connectedBody, pedalsConnection.anchorPosition);

        var barConnection = bike.GetConnectionWithBar();
        rider.ConnectHands(barConnection.connectedBody, barConnection.anchorPosition);

        cameraManager.SetToggledObject(rider);
    }
}