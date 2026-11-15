using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleSpawner))]
public class ObstacleSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "m_Script", "schedule");

        SerializedProperty schedule = serializedObject.FindProperty("schedule");
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Schedule", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Arrival time is the song time when this obstacle reaches z = 0.", MessageType.Info);

        for (int i = 0; i < schedule.arraySize; i++)
        {
            SerializedProperty entry = schedule.GetArrayElementAtIndex(i);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    string typeName = entry.managedReferenceValue?.GetType().Name ?? "Missing event";
                    EditorGUILayout.LabelField($"{i + 1}. {typeName}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    {
                        schedule.DeleteArrayElementAtIndex(i);
                        break;
                    }
                }

                entry.isExpanded = true;
                EditorGUILayout.PropertyField(entry, GUIContent.none, true);
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Add ring")) AddEvent(schedule, new RingEvent());
            if (GUILayout.Button("Add wall")) AddEvent(schedule, new WallEvent());
            if (GUILayout.Button("Add bang")) AddEvent(schedule, new BangEvent());
        }

        serializedObject.ApplyModifiedProperties();
    }

    private static void AddEvent(SerializedProperty schedule, ObstacleEvent entry)
    {
        schedule.arraySize++;
        schedule.GetArrayElementAtIndex(schedule.arraySize - 1).managedReferenceValue = entry;
    }
}
