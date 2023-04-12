using UnityEngine;

namespace Interfaces
{
    public interface IBike : IDestroyable, ITransformable
    {
        (Rigidbody2D connectedBody, Vector2 anchorPosition) GetConnectionWithBar();
        (Rigidbody2D connectedBody, Vector2 anchorPosition) GetConnectionWithPedals();
    }
}