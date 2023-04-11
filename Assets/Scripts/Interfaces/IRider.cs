using UnityEngine;

namespace Interfaces
{
    public interface IRider : IDestroyable, ICameraTraceable, ITransformable
    {
        void ConnectHands(Rigidbody2D connectedBody, Vector2 anchorPosition);
        void ConnectFoots(Rigidbody2D connectedBody, Vector2 anchorPosition);
    }
}