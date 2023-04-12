using Interfaces;
using UnityEngine;

namespace Fabrics
{
    public abstract class BikeFabric : MonoBehaviour
    {
        public abstract IBike BuildBike();
    }
}