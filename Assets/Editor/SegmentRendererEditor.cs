using System;
using SegmentGeneration;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(SegmentRenderer))]
    public class SegmentRendererEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Render"))
            {
                var segmentRenderer = (SegmentRenderer)target;
                segmentRenderer.Render();
                EditorUtility.SetDirty(target);
            }
        }
    }
}