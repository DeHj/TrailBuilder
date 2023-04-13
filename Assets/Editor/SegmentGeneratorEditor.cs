using UnityEditor;
using UnityEngine;

namespace SegmentGeneration
{
    [CustomEditor(typeof(SegmentGenerator))]
    public class SegmentGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
            {
                var segmentGenerator = (SegmentGenerator)target;
                segmentGenerator.Generate();
            }
        }
    }
}