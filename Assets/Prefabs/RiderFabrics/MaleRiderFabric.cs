using Fabrics;
using Interfaces;
using Prefabs.Riders;
using UnityEngine;

namespace Prefabs.Rider_fabrics
{
    public class MaleRiderFabric : RiderFabric
    {
        public GameObject riderPrefab;

        public override IRider BuildRider()
        {
            var riderObject = Instantiate(riderPrefab);

            var footsConnection = riderObject.GetComponentInChildren<FootsConnection>();
            var handsConnection = riderObject.GetComponentInChildren<HandsConnection>();

            return new MaleRider(riderObject, footsConnection.connection, handsConnection.connection);
        }
    }

    internal class MaleRider : IRider
    {
        private readonly GameObject _gameObject;
        private readonly HingeJoint2D _footsConnection;
        private readonly HingeJoint2D _handsConnection;

        public MaleRider(GameObject gameObject, HingeJoint2D footsConnection, HingeJoint2D handsConnection)
        {
            _gameObject = gameObject;
            _footsConnection = footsConnection;
            _handsConnection = handsConnection;
        }

        public void Destroy()
        {
            Object.Destroy(_gameObject);
        }

        public Vector2 GetPosition()
        {
            return _gameObject.transform.position;
        }

        public void ConnectHands(Rigidbody2D connectedBody, Vector2 anchorPosition)
        {
            _handsConnection.connectedBody = connectedBody;
            _handsConnection.connectedAnchor = anchorPosition;
        }

        public void ConnectFoots(Rigidbody2D connectedBody, Vector2 anchorPosition)
        {
            _footsConnection.connectedBody = connectedBody;
            _footsConnection.connectedAnchor = anchorPosition;
        }
    }
}