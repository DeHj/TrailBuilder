using System.Linq;
using UnityEngine;

public class SegmentRenderer : MonoBehaviour
{
    public EdgeCollider2D segmentCollider;
    public LineRenderer segmentRenderer;

    public void Render()
    {
        segmentRenderer.positionCount = segmentCollider.points.Length;
        segmentRenderer.SetPositions(segmentCollider.points
            .Select(point => new Vector3(point.x, point.y, 0))
            .ToArray());
    }
}