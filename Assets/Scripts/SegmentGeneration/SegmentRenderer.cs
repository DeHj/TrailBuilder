using System.Linq;
using UnityEngine;

namespace SegmentGeneration
{
    [RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
    public class SegmentRenderer : MonoBehaviour
    {
        public void Render()
        {
            var segmentCollider = GetComponent<EdgeCollider2D>();
            var segmentRenderer = GetComponent<LineRenderer>();

            segmentRenderer.useWorldSpace = false;

            segmentRenderer.positionCount = segmentCollider.points.Length;
            segmentRenderer.SetPositions(segmentCollider.points
                .Select(point => new Vector3(point.x, point.y, 0))
                .ToArray());
        }
    }
}