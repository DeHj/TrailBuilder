using UnityEngine;

namespace Interfaces
{
    /// <summary>
    ///     Derive that to create an object can be connected (by hinge joints) to a bike.
    /// </summary>
    public interface IAttachableToBike
    {
        void ConnectHands(Rigidbody2D connectedBody, Vector2 anchorPosition);
        void ConnectFoots(Rigidbody2D connectedBody, Vector2 anchorPosition);
    }
}