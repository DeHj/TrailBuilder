using UnityEditor;
using UnityEngine;

namespace SegmentGeneration
{
    [CustomEditor(typeof(SegmentRenderer))]
    public class SegmentRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Render"))
            {
                var segmentRenderer = (SegmentRenderer)target;
                segmentRenderer.Render();
            }
        }
    }
}