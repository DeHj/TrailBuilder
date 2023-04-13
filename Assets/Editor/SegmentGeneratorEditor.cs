using SegmentGeneration;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SegmentGenerator))]
    public class SegmentGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
            {
                var segmentGenerator = (SegmentGenerator)target;
                segmentGenerator.Generate();
                EditorUtility.SetDirty(target);
            }
        }
    }
}