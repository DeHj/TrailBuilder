using Interfaces;
using UnityEngine;

namespace Fabrics.RiderFabrics
{
    public abstract class RiderFabric : MonoBehaviour, IRiderFabric
    {
        public abstract IRider BuildRider();
    }
}