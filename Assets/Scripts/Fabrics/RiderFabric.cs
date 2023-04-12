using Interfaces;
using UnityEngine;

namespace Fabrics
{
    public abstract class RiderFabric : MonoBehaviour
    {
        public abstract IRider BuildRider();
    }
}