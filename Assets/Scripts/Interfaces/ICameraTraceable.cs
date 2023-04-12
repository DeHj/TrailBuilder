using UnityEngine;

namespace Interfaces
{
    public interface ICameraTraceable
    {
        /// <summary>
        ///     Returns global position for camera tracing 
        /// </summary>
        /// <returns></returns>
        Vector2 GetPosition();
    }
}