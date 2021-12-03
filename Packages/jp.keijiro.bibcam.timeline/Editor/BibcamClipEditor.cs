using UnityEditor;
using UnityEngine;

namespace Bibcam.Timeline {

[CustomEditor(typeof(BibcamClip)), CanEditMultipleObjects]
class BibcamClipEditor : Editor
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

} // namespace Bibcam.Timeline
