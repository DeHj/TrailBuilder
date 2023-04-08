using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Point = Models.Point;
using Edge = Models.Edge;
using Random = Unity.Mathematics.Random;

public class SegmentGenerator : MonoBehaviour
{
    public EdgeCollider2D segmentCollider;

    public float length;
    [Header("Angle of segment in degrees")]
    [Range(0, 75)]
    public float segmentGradient;
    
    [Header("Count of point per 1 meter of segment")]
    [Range(0.1f, 10)]
    public float detailingRange;

    [Range(0, 0.2f)]
    public float dispersion;

    public Point startPoint;

    public uint seed;

    public void Generate()
    {
        var segment = GenerateSegment();

        segmentCollider.points = segment
            .Select(point => new Vector2(point.x, point.y))
            .ToArray();
    }

    private Point[] GenerateSegment()
    {
        var r = new Random(seed);
        
        var pointCounts = (int) (length * detailingRange);
        var segmentPoints = new List<Point>(pointCounts - 1)
        {
            new(startPoint.x, startPoint.y),
            new(startPoint.x + length * math.cos(segmentGradient * Mathf.Deg2Rad), startPoint.y - length * math.sin(segmentGradient * Mathf.Deg2Rad))
        };

        var firstEdge = new Edge(segmentPoints[0], segmentPoints[1]);
        
        var edges = new SortedList<float, Edge>(pointCounts - 1, new EdgeComparer())
        {
            { firstEdge.Length, firstEdge }
        };

        for (int i = 0; i < pointCounts; i++)
        {
            var splittableEdge = edges.Last();
            var start = splittableEdge.Value.Start;
            var end = splittableEdge.Value.End;

            var ratio = r.NextFloat(0.01f, 0.99f);

            var newPoint = new Point(
                start.x + (end.x - start.x) * ratio,
                start.y + (end.y - start.y) * ratio + splittableEdge.Value.Length * dispersion * r.NextFloat(-1, 1));

            segmentPoints.Add(newPoint);
            edges.RemoveAt(edges.Count - 1);

            var leftEdge = new Edge(start, newPoint);
            var rightEdge = new Edge(newPoint, end);

            edges.Add(leftEdge.Length, leftEdge);
            edges.Add(rightEdge.Length, rightEdge);
        }

        // Compile segmentPoints to EdgeCollider
        segmentPoints.Sort((a, b) => a.x.CompareTo(b.x));

        return segmentPoints.ToArray();
    }

    private class EdgeComparer : IComparer<float>
    {
        public int Compare(float x, float y)
        {
            var result = x.CompareTo(y);
            return result == 0 ? 1 : result;
        }
    }
}
