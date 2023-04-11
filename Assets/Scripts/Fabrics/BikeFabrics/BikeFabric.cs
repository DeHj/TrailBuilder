using Interfaces;
using UnityEngine;

namespace Fabrics.BikeFabrics
{
    public abstract class BikeFabric : MonoBehaviour, IBikeFabric
    {
        public abstract IBike BuildBike();
    }
}