using UnityEngine;

namespace Interfaces
{
    public interface IRider : ICameraTraceable
    {
        // todo: figure out what is anchorPosition
        void ConnectHands(Rigidbody2D connectedBody, Vector2 anchorPosition);
        void ConnectFoots(Rigidbody2D connectedBody, Vector2 anchorPosition);
    }
}