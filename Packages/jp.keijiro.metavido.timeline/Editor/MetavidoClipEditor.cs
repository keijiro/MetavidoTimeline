using UnityEditor;
using UnityEngine;

namespace Metavido.Timeline {

[CustomEditor(typeof(MetavidoClip)), CanEditMultipleObjects]
class MetavidoClipEditor : Editor
{
    SerializedProperty _filePath;
    SerializedProperty _pathMode;

    void OnEnable()
    {
        _filePath = serializedObject.FindProperty("filePath");
        _pathMode = serializedObject.FindProperty("pathMode");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_filePath);
        EditorGUILayout.PropertyField(_pathMode);
        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Metavido.Timeline
